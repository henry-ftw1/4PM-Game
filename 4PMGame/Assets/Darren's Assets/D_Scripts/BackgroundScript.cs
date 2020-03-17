using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public float bgSpeed = 10f;
    [SerializeField]
    private float startYPos = 52.4f; //starting bg position
    [SerializeField]
    private float resetYPos = -51.9f; //when bg hits this y, then reset

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localPosition = new Vector3(this.transform.position.x, startYPos, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(new Vector3(0, -bgSpeed * Time.deltaTime, 0));
        if(this.transform.position.y < resetYPos)
        {
            this.transform.localPosition = new Vector3(this.transform.position.x, startYPos, 0);
        }
    }
}
