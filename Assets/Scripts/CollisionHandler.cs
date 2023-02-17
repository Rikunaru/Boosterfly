using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]float levelLoadDelay = 1f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    int currentSceneIndex = 0;
    bool isTransitioning = false;
    bool collisionDisabled = false;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessDebugKeys();
    }

    void OnCollisionEnter(Collision other)
    {
        if(isTransitioning || collisionDisabled){ return; }

        switch (other.gameObject.tag) 
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void ProcessDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
            Debug.Log("Toggled collision");
        }
    }

    void StartSuccessSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(success);

        successParticles.Play();

        isTransitioning = true;

        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    void StartCrashSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(crash);

        crashParticles.Play();

        isTransitioning = true;

        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void ReloadLevel()
    {
        currentSceneIndex = GetCurrentSceneIndex();
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadNextLevel()
    {
        currentSceneIndex = GetCurrentSceneIndex();
        int nextSceneIndex = currentSceneIndex  + 1;

        //check if next SceneIndex would be total number of scenes and if so make the first level the next one
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
