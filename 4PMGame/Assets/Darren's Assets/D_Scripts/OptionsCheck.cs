using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsCheck : MonoBehaviour
{
    private void OnEnable()
    {
        if (this.name == "Vibrate")
            CheckVibrate();
        else if (this.name == "Audio")
            CheckSound();
    }

    public void CheckVibrate()
    {
        Toggle ToggleChild = GetComponent<Toggle>();
        if (PlayerPrefs.GetInt("Vibrate") == 1)
            ToggleChild.isOn = true;
        else
            ToggleChild.isOn = false;
        //GameObject.Find("OptionsSettings").GetComponent<OptionsData>().Vibrate;
    }

    public void SetVibrate()
    {
        Toggle ToggleChild = GetComponent<Toggle>();
        int isTrue = 1;
        if (ToggleChild.isOn)
            isTrue = 1;
        else
            isTrue = 0;
        PlayerPrefs.SetInt("Vibrate", isTrue);
        Debug.Log(isTrue);
    }

    public void CheckSound()
    {
        Toggle ToggleChild = GetComponent<Toggle>();
        if (PlayerPrefs.GetInt("Sound") == 1)
            ToggleChild.isOn = true;
        else
            ToggleChild.isOn = false;
    }

    public void SetSound()
    {
        Toggle ToggleChild = GetComponent<Toggle>();
        int isTrue = 1;
        if (ToggleChild.isOn)
            isTrue = 1;
        else
            isTrue = 0;
        PlayerPrefs.SetInt("Sound", isTrue);
        audioManager aManager = GameObject.Find("AudioManager").GetComponent<audioManager>();
        aManager.StartStopMusic();
        Debug.Log(isTrue);
    }
}
