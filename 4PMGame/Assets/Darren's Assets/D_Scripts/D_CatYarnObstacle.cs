using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_CatYarnObstacle : MonoBehaviour
{
    public float moveSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
        if(transform.position.y < -30f)
        {
            Destroy(this.gameObject);
        }
    }

    /***
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<D_playerScript>().TakeHit(1);
        }
    }
    ***/
}
