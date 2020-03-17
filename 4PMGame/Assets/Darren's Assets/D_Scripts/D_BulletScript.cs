using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_BulletScript : D_Bullet
{
    public AudioClip hitSound;
    private Renderer myRenderer;
    private Collider2D myCollider;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.tag = "Bullet";
        myRenderer = GetComponent<Renderer>();
        myCollider = GetComponent<Collider2D>();
    }

    //check collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CollisionTimer > 0) return;
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<D_GameEntity>().TakeHit(Damage);
            if (hitSound) {
                audioManager.instance.playClip(hitSound);
            }
            this.gameObject.SetActive(false);

            //Destroy(this.gameObject);
        }
        else if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "EnemyBullet")
        {
            //do nothing
        }
        else if(collision.gameObject.tag == "Wall")
        {
            this.gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }
    }

}
