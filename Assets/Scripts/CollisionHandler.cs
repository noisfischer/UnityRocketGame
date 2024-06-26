using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadDelay = 1.0f;
    AudioSource rocketAudio;
    [SerializeField] AudioClip crashAudio;
    [SerializeField] AudioClip successAudio;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    bool bTransitioning = false;
    bool bGodMode = false;
    void Start()
    {
        rocketAudio = GetComponent<AudioSource>();
        bGodMode = false;
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            if (SceneManager.GetActiveScene().buildIndex <= SceneManager.loadedSceneCount)
            {
                NextLevel();
            }
            else
            {
                RestartGame();
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if(bGodMode == false)
            {
                bGodMode = true;
                Debug.Log("God Mode Activated");
            }
            else
            {
                bGodMode = false;
                Debug.Log("God Mode Deactivated");
            }
            
        }
        else if (Input.GetKey(KeyCode.R))
        {
            LoadLevel(0);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(bTransitioning || bGodMode) {return;}

        switch(other.gameObject.tag)
        {
            case "Friendly":
            {
                break;
            }
            case "Finish":
            {
                bTransitioning = true;
                rocketAudio.Stop();
                rocketAudio.PlayOneShot(successAudio);
                successParticles.Play();
                gameObject.GetComponent<Movement>().enabled = false;
                Invoke("NextLevel", loadDelay);
                break;
            }
            case "Final Level":
            {
                bTransitioning = true;
                rocketAudio.Stop();
                rocketAudio.PlayOneShot(successAudio);
                successParticles.Play();
                gameObject.GetComponent<Movement>().enabled = false;
                Invoke("RestartGame", loadDelay);
                break;
            }
            default:
                bTransitioning = true;
                rocketAudio.Stop();
                rocketAudio.PlayOneShot(crashAudio);
                crashParticles.Play();
                gameObject.GetComponent<Movement>().enabled = false; // disable movement
                Invoke("StartCrashSequence", loadDelay); // delay restart level a bit
                break;
        }
    }

    void StartCrashSequence()
    {
        LoadLevel(0); // restart current level
    }

    void NextLevel()
    {
        LoadLevel(1);
    }

    void RestartGame()
    {
        LoadLevel(-SceneManager.GetActiveScene().buildIndex);
    }

    void LoadLevel(int level)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex + level;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
