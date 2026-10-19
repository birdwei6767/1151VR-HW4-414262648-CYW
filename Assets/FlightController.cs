using UnityEngine;

public class FlightController : MonoBehaviour
{
    [Header("飛行速度設定")]
    public float forwardSpeed = 20f;
    public float controlSpeed = 15f;

    [Header("機頭擺動設定 (新增)")]
    public float leanXAngle = 20f;
    public float leanZAngle = 30f;
    public float tiltSpeed = 5f;

    [Header("音效設定")]
    public AudioSource engineAudio;
    public AudioClip crashSound;
    public AudioClip passSound;

    [Range(0f, 1f)] public float crashVolume = 0.8f; 
    [Range(0f, 1f)] public float passVolume = 0.8f;

    private AudioSource sfxAudio;

    void Start()
    {
        sfxAudio = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime, Space.Self);

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(moveX, moveY, 0) * controlSpeed * Time.deltaTime, Space.Self);

        float targetPitch = -moveY * leanXAngle;
        float targetRoll = -moveX * leanZAngle;

        Quaternion targetRotation = Quaternion.Euler(targetPitch, 0, targetRoll);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, tiltSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            sfxAudio.PlayOneShot(crashSound, crashVolume); 
            
            forwardSpeed = 0; 
            controlSpeed = 0;
            if (engineAudio != null) engineAudio.Stop(); 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PassZone"))
        {
            sfxAudio.PlayOneShot(passSound, passVolume); 
        }
    }
}