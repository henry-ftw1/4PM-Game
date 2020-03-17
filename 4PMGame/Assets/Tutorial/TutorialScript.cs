using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialScript : MonoBehaviour
{
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject text4;
    public GameObject Enemy1;
    bool tutorialStart = false;
    // Start is called before the first frame update
    void Start()
    {
        text1.SetActive(false);
        text2.SetActive(false);
        text3.SetActive(false);
        text4.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(tutorialStart == false)
        {
            StartCoroutine(TutorialCoroutine());
            tutorialStart = true;
        }
    }

    public IEnumerator TutorialCoroutine()
    {
        text1.SetActive(true);
        yield return new WaitForSeconds(7f);
        text1.SetActive(false);
        Instantiate(Enemy1, new Vector3(0f, 9f, 0f), Quaternion.identity);
        text2.SetActive(true);
        yield return new WaitForSeconds(7f);
        text2.SetActive(false);
        Instantiate(Enemy1, new Vector3(0f, 9f, 0f), Quaternion.identity);
        text3.SetActive(true);
        yield return new WaitForSeconds(7f);
        text3.SetActive(false);

        while (true)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
            {
                text4.SetActive(true);
                yield return new WaitForSeconds(4f);
                text4.SetActive(false);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveOffScreen>().IsNotInPos();
                break;
            }
            yield return null;
        }
        //yield return null;
    }
}
