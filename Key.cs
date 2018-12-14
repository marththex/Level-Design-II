using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour
{

    /// SOUNDS
    private AudioSource source;
    public AudioClip collectSound;
    private bool isCollected = false;
    private float waitToDestroyTime = 1.0f;
    //private Renderer rend;
    public bool hasParticleEffect = false;
    public GameObject particleEffect;
    //private LevelManager lm;
    private GameManager gm;

    // Use this for initialization
    void Start()
    {
       // lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
       // source = GetComponent<AudioSource>();
        //rend = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        

        if (other.tag == "Player" && !isCollected)
        {
            Debug.Log("Collected Key");
            //source.PlayOneShot(collectSound);
            isCollected = true;
            // rend.enabled = false;
            GameManager.addKey();
            if (hasParticleEffect)
            {
                Destroy(particleEffect);
            }
            StartCoroutine(Delay(waitToDestroyTime));
            //Destroy(this.gameObject);
        }

        if (other.tag == "NPC" && !isCollected)
        {

        }
    }

    IEnumerator Delay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
        Debug.Log("Destroyed Object");
    }
}