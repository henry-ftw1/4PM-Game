using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    float[] FPSList = new float[5];
    int index = 0;

    TextMeshProUGUI display;

    // Start is called before the first frame update
    void Start()
    {
        display = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        FPSList[index] = Time.deltaTime;

        display.SetText("FPS: " + (1f/averageList()).ToString("F2"));

        if (index < 4) ++index;
        else index = 0;
    }

    float averageList() {
        float counter = 0f;
        for (int i = 0; i < 5; ++i) {
            counter = counter + FPSList[i];
        }
        return counter / 5f;
    }
}
