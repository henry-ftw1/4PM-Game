using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    /***
     * Shake Coroutine
     * 
     * Input: Float Duration, Float Magnitude
     *   Duration: how long the shake will happen for
     *   Magnitude: the intensity of the shake
     * 
     * Returns: Null
    ***/
    public IEnumerator Shake(float duration, float magnitude)
    {
        try
        {
            if (PlayerPrefs.GetInt("Vibrate") == 1)
            {
                Debug.Log("Vibrate");
                Handheld.Vibrate();
            }
            else
                Debug.Log("No Vibrate");
        }
        catch
        {
            Debug.Log("No Vibrate");
        }
        //original position of camera
        Vector3 originalPos = transform.localPosition;
        Debug.Log("Shake");
        //time elapsed after shake starts
        float elapsed = 0.0f;

        //loop until duration
        while(elapsed < duration)
        {
            //set x and y values to a random position using magnitude
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            //change the position of camera
            transform.localPosition = new Vector3(x, y, originalPos.z);

            //add time to elapsed
            elapsed += Time.deltaTime;

            if (Time.timeScale == 0)
                elapsed = duration;

            yield return null;
        }
        //return the camera to original position after shake ends
        transform.localPosition = originalPos;
    }
}
