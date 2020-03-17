using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public D_GameManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        gManager = GameObject.Find("GameManager").GetComponent<D_GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.y > 7.5)
        {
            transform.Translate(new Vector3(0, -moveSpeed * Time.deltaTime, 0));
        }
    }
}
