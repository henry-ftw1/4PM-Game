using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : D_Bullet
{
    public float moveMultiplier = 1.025f;
    private float storeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 1f;
        storeSpeed = moveSpeed;
    }

    private void Update()
    {
        Move();
        if (CollisionTimer > 0)
        {
            CollisionTimer--;
        }
    }

    // Update is called once per frame
    protected void Move()
    {
        this.gameObject.transform.Translate(0, moveSpeed * Time.deltaTime, 0);
        moveSpeed = moveSpeed + moveMultiplier * Time.deltaTime;
        //Debug.Log("YES");
    }

    private void OnEnable()
    {
        moveSpeed = storeSpeed;
        //if(this.gameObject.tag == "PlayerBullet")
        //    transform.Rotate(new Vector3(0, 0, 180));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.tag == "PlayerBullet")
        {
            if (CollisionTimer > 0) return;
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<D_GameEntity>().TakeHit(Damage);
                //Destroy(this.gameObject);
                this.gameObject.SetActive(false);
            }
            else if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "EnemyBullet")
            {
                //do nothing
            }
            else if (collision.gameObject.tag == "Wall")
            {
                this.gameObject.SetActive(false);
                //Destroy(this.gameObject);
            }
        }
    }
}
