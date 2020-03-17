using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_EnemyBulletScript : D_Bullet
{
    // Start is called before the first frame update
    void Start()
    {
//        this.gameObject.tag = "Bullet";
    }
    //check collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
/*
        if (CollisionTimer > 0) return;
        if (collision.gameObject.tag == "Player")
        {
            try
            {
                collision.gameObject.GetComponent<D_playerScript>().TakeHit(1f);
            }
            catch
            {
                // Bug is when parts attach and the bullet tries to call the script from the part and not the player.
                Debug.Log("Fix Me Get Component");
            }
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Bullet")
        {
            //do nothing
        }
        else
            Destroy(this.gameObject);
*/
    }
}
