using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveOffScreen : MonoBehaviour
{
    public bool IsStartOfGame = true;
    public bool isInPosition = false;
    public float speedmult = 5f;
    public float speedMax = 5f;
    public float speedMin = 0.5f;

    public string playScene = "Level1";

    D_playerScript pScript;
    PlayerTouchScript touchScript;
    Rigidbody2D rb2d;

    GameObject HPText;
    GameObject HPBar;
    // Start is called before the first frame update
    void Start()
    {
        pScript = gameObject.GetComponent<D_playerScript>();
        touchScript = gameObject.GetComponent<PlayerTouchScript>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        HPText = GameObject.Find("PlayerUI").transform.Find("HPText").gameObject;
        HPBar = GameObject.Find("PlayerUI").transform.Find("HealthBG").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInPosition)
        {
            ScriptsEnable(false);
            this.GetComponent<BoxCollider2D>().enabled = false;
            if (IsStartOfGame)
            {
                bringPlayerIn();
            }
            else
            {
                bringPlayerOut();
            }
        }
    }

    public void bringPlayerIn()
    {
        pScript.UpdateHealthBar();
        if (this.transform.position.y <= -4f)
        {
            rb2d.MovePosition(transform.position + new Vector3(0, 3 * speedmult * Time.deltaTime, 0));
            //transform.Translate(0, 3 * speedmult * Time.deltaTime, 0);
            speedmult -= 0.15f;
            if (speedmult < speedMin)
                speedmult = speedMin;
        }
        else
        {
            isInPosition = true;
            IsStartOfGame = false;
            ScriptsEnable(true);
            pScript.SetShoot(false);
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void bringPlayerOut()
    {
        if (this.transform.position.y <= 11f)
        {
            rb2d.MovePosition(transform.position + new Vector3(0, 3 * speedmult * Time.deltaTime, 0));
            //transform.Translate(0, 3 * speedmult * Time.deltaTime, 0);
            speedmult += 0.1f;
            if (speedmult > speedMax)
                speedmult = speedMax;
        }
        else
        {
            this.rb2d.MovePosition(new Vector3(0f, -12f, 0f));
            //this.transform.position = new Vector3(0f, -12f, 0f);
            IsStartOfGame = true;
            GameObject.Find("GameManager").GetComponent<D_GameManager>().setStart(IsStartOfGame);
            NextScene(playScene);
        }
    }

    void NextScene(string SceneName)
    {
        this.transform.localScale = new Vector3(1f,1f,1f);
        GameObject.Find("SceneManager").GetComponent<D_SceneManager>().LoadLevel(SceneName);
    }

    void ScriptsEnable(bool value)
    {
        if (HPText == null && HPBar == null)
        {
            HPText = GameObject.Find("PlayerUI").transform.Find("HPText").gameObject;
            HPBar = GameObject.Find("PlayerUI").transform.Find("HealthBG").gameObject;
        }
        HPText.SetActive(value);
        HPBar.SetActive(value);
        pScript.enabled = value;
        touchScript.enabled = value;
    }

    public void IsNotInPos()
    {
        isInPosition = false;
    }

    public void NextSceneName(string name)
    {
        playScene = name;
    }

    public void SendPlayerToMars()
    {
        ScriptsEnable(false);
        this.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(ShrinkPlayer());
    }

    IEnumerator ShrinkPlayer()
    {
        float time = 0f;
        while (time < 15f)
        {
            if (this.transform.position.y <= 2f)
            {
                rb2d.MovePosition(transform.position + new Vector3(0, 4 * Time.deltaTime, 0));
            }
            else
            {
                rb2d.MovePosition(transform.position + new Vector3(0, Time.deltaTime, 0));
                if (this.transform.localScale.x > 0.3f && this.transform.localScale.y > 0.3f)
                {
                    this.transform.localScale = new Vector3(this.transform.localScale.x - Time.deltaTime, this.transform.localScale.y - Time.deltaTime, 0);
                }
                else
                    break;
            }
            time += 0.01f;
            yield return new WaitForFixedUpdate();
        }
        GameObject.Find("SceneManager").GetComponent<D_SceneManager>().YouWin();
        yield return null;
    }
}
