using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class D_SceneManager : MonoBehaviour
{
    public static D_SceneManager current;

    public bool isPaused;
    public bool isGameOver = false;
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    public GameObject youWinCanvas;
    public TextMeshProUGUI StageText;
    public TextMeshProUGUI StageClearText;
    private ScoreData scores;

    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        //change when importing to phone
        Screen.SetResolution(540, 960, false);
        Time.timeScale = 1;
        isPaused = false;
        try
        {
            pauseCanvas.SetActive(false);
            gameOverCanvas.SetActive(false);
            youWinCanvas.SetActive(false);
            StageText.gameObject.SetActive(false);
            StageClearText.gameObject.SetActive(false);
        }
        catch
        {
            Debug.Log("pause or game over canvases not assigned");
        }
        scores = new ScoreData();
        scores.waves = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            PauseOrResume();
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        DestryObjects();
        ResetPlayerPrefs();
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel(string level)
    {
        if(level == "MainMenu")
        {
            ResetPlayerPrefs();
            DestryObjects();
        }
        if (level == "Level1")
        {
            ResetPlayerPrefs();
        }
        SceneManager.LoadScene(level);
    }

    public void PauseOrResume()
    {
        try 
        {
            if (isPaused) 
            {
                Time.timeScale = 1;
                pauseCanvas.SetActive(false);
                isPaused = false;
            }
            else 
            {
                Time.timeScale = 0;
                pauseCanvas.SetActive(true);
                isPaused = true;
            }
        }
        catch 
        {
            Debug.Log("CANVAS PROBABLY NOT ASSIGNED");
        }
    }

    void DestryObjects()
    {
        try
        {
            Destroy(GameObject.Find("D_player"));
        }
        catch
        {
            Debug.Log("Unable to find player");
        }
        try
        {
            Destroy(GameObject.Find("GameManager"));
        }
        catch
        {
            Debug.Log("Unable to find GameManager");
        }
        try
        {
            GameObject[] parts = GameObject.FindGameObjectsWithTag("PlayerPart");
            foreach (GameObject part in parts)
                GameObject.Destroy(part);
        }
        catch
        {
            Debug.Log("Unable to find parts");
        }
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverCanvas.SetActive(true);
        gameOverCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(
            "Parts Collected\n" + PlayerPrefs.GetInt("ItemsCollected").ToString() + "\n" +
            "\nEnemies Destroyed\n" + PlayerPrefs.GetInt("EnemyKills").ToString() + "\n" +
            "\nFinal Score\n" + PlayerPrefs.GetInt("Score").ToString());
        ResetPlayerPrefs();
        isGameOver = true;
    }

    public void YouWin()
    {
        Time.timeScale = 0;
        youWinCanvas.SetActive(true);
        youWinCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(
            "Parts Collected\n" + PlayerPrefs.GetInt("ItemsCollected").ToString() + "\n" +
            "\nEnemies Destroyed\n" + PlayerPrefs.GetInt("EnemyKills").ToString() + "\n" +
            "\nFinal Score\n" + PlayerPrefs.GetInt("Score").ToString());
        ResetPlayerPrefs();
        isGameOver = true;
    }

    public void ShowStageText(int stageNum)
    {
        if (stageNum == 4)
            StageText.SetText("Final Stage");
        else
            StageText.SetText("Stage " + stageNum.ToString());
        if (StageText.IsActive())
        {
            StageText.gameObject.SetActive(false);
        }
        else
        {
            StageText.gameObject.SetActive(true);
        }
    }

    public void ShowStageClear()
    {
        if (StageClearText.IsActive())
        {
            StageClearText.gameObject.SetActive(false);
        }
        else
        {
            StageClearText.gameObject.SetActive(true);
        }
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("EnemyKills", 0);
        PlayerPrefs.SetInt("ItemsCollected", 0);
    }
}
