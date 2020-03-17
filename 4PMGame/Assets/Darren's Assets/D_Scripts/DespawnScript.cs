using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "cat ball yarn" || collision.gameObject.name == "cat ball yarn(Clone)") 
        {
            //Debug.Log(collision.gameObject.name);
            return;
        }
        if(collision.gameObject.tag != "Player" && collision.gameObject.tag != "Enemy" && collision.gameObject.tag!="PlayerBullet")
        {
            Destroy(collision.gameObject);
        }
    }
}
