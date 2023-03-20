using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayTime = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
 

    AudioSource audioSource;
    bool isTransitioning = false;
    bool collisionDisabled = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update() 
    {   
        RespondToDebugKeys();  
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; //toggle Collision
            if(collisionDisabled){
                Debug.Log("Collision OFF");
            }
            else{
                Debug.Log("Collision ON");
            }
        }
    }

    void OnCollisionEnter(Collision other) 
    {
        if(isTransitioning || (collisionDisabled && other.gameObject.tag != "Finish")){ return; }


        switch(other.gameObject.tag)
        {
            case "Finish":
                StartSuccessSequence();
                break;
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            default:
                StartCrashSequence();
                break;
        }                
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", delayTime);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash); 
        crashParticles.Play();      
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delayTime);
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; 
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; 
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);

    }

    
    // void DisableCollisions()
    // {
    //     if (Input.GetKey(KeyCode.C))
    //     {
    //         LoadNextLevel();
    //     }
    // }
}
