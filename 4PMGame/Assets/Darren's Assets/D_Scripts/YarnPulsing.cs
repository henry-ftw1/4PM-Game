using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnPulsing : MonoBehaviour
{
    float xScale;
    float yScale;
    public float maxScale = 4.7f;
    public float scaleScale = 0.02f;
    bool grow = true;
    // Start is called before the first frame update
    void Start()
    {
        xScale = this.transform.localScale.x;
        yScale = this.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (grow)
            this.transform.localScale += new Vector3(scaleScale * Time.deltaTime, scaleScale * Time.deltaTime, 0);
        else
            this.transform.localScale -= new Vector3(scaleScale * Time.deltaTime, scaleScale * Time.deltaTime, 0);

        if (this.transform.localScale.x >= maxScale)
        {
            grow = false;
        }
        else if (this.transform.localScale.x <= xScale)
            grow = true;
    }
}
