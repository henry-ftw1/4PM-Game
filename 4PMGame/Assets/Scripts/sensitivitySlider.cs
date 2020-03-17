using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sensitivitySlider : MonoBehaviour
{
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = OptionsData.current.touchSensitivity;
        slider.onValueChanged.AddListener(delegate {updateSensitivity();});
    }

    void updateSensitivity() {
        OptionsData.current.touchSensitivity = slider.value;
    }
}
