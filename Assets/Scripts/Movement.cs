using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustForce = 1000f;
    [SerializeField] float rotationForce = 200f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainBoosterParticles;
    [SerializeField] ParticleSystem LeftBoosterParticles;
    [SerializeField] ParticleSystem RightBoosterParticles;

    Rigidbody rb;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }


    void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * thrustForce * Time.deltaTime);
            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            if(!mainBoosterParticles.isPlaying)
            {
                mainBoosterParticles.Play();
            }
        }
        else
        {
            audioSource.Stop();
            mainBoosterParticles.Stop();
        }
    }


    void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationForce);
            if(!RightBoosterParticles.isPlaying)
            {
                RightBoosterParticles.Play();
            }
        }
        else if(Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationForce);
            if(!LeftBoosterParticles.isPlaying)
            {
                LeftBoosterParticles.Play();
            }
        }
        else
        {
            RightBoosterParticles.Stop();
            LeftBoosterParticles.Stop();
        }

    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; //freezing rotation to enable manual rotating
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //unfreezing so physics can take over
    }
}
