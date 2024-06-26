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
    bool transitioning = false;

    void Start()
    {
        rocketAudio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        if(transitioning) {return;}

        switch(other.gameObject.tag)
        {
            case "Friendly":
            {
                break;
            }
            case "Finish":
            {
                transitioning = true;
                rocketAudio.Stop();
                rocketAudio.PlayOneShot(successAudio);
                successParticles.Play();
                gameObject.GetComponent<Movement>().enabled = false;
                Invoke("NextLevel", loadDelay);
                break;
            }
            case "Final Level":
            {
                transitioning = true;
                rocketAudio.Stop();
                rocketAudio.PlayOneShot(successAudio);
                successParticles.Play();
                gameObject.GetComponent<Movement>().enabled = false;
                Invoke("RestartGame", loadDelay);
                break;
            }
            default:
                transitioning = true;
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
        // particles
        LoadLevel(0); // restart current level
    }

    void NextLevel()
    {
        // particles
        LoadLevel(1);
    }

    void RestartGame()
    {
        // particles
        LoadLevel(-SceneManager.GetActiveScene().buildIndex);
    }

    void LoadLevel(int level)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex + level;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
