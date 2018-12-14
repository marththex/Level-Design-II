using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{

    /// SOUNDS
    private AudioSource source;
    public AudioClip collectSound;
    public static bool isCollected = false;
    private float waitToDestroyTime = 1.0f;
    //private Renderer rend;
    public bool hasParticleEffect = false;
    public GameObject particleEffect;
    private LevelManager lm;

    // Use this for initialization
    void Start()
    {
        // lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        // source = GetComponent<AudioSource>();
        //rend = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("You entered the box");
        if (other.tag == "Player" && GameManager.numKeysCollected == 2)
        {
            Debug.Log("You may escape");
            isCollected = true;
            //SceneManager.LoadScene("WinScreen", LoadSceneMode.Single);

        }
    }


}