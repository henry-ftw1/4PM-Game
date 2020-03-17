using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hHealthPack : MonoBehaviour
{
    public float healthIncrease;
    // Start is called before the first frame update

    [Header("Sounds")]
    [SerializeField]
    private AudioClip deathSound = null;
    void Start()
    {
        healthIncrease = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == null)
        {
            //do nothing
        }
        if(collision.gameObject.tag == "Player")
        {
            if(collision.transform.GetComponent<D_playerScript>() != null)
            {
                D_playerScript player = collision.transform.GetComponent<D_playerScript>();
                player.increaseHealth(healthIncrease);
                audioManager.instance.playClip(deathSound);
                this.gameObject.SetActive(false);
            }
        }
        else if(collision.gameObject.tag == "Wall")
        {
            this.gameObject.SetActive(false);
        }
    }
}
