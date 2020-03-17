using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsData : MonoBehaviour
{
    public static OptionsData current;
    public Toggle toggle;
    public bool Vibrate = true;
    public float touchSensitivity = 1.0f;
    void Awake()
    {
        if (current == null)
            current = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
        PlayerPrefs.SetInt("Vibrate", 1);
        DontDestroyOnLoad(this.gameObject);
    }
}
