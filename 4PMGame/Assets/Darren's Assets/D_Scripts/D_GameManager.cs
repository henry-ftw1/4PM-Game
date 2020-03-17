using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manages the game loop and state
public class D_GameManager : MonoBehaviour
{
    public static D_GameManager current;
    public int WaveNumber = 0;
    public int LevelNumber = 0;
    public D_EnemySpawner enemySpawner;
    [SerializeField]
    private bool Spawning = false;
    [SerializeField]
    private bool isGameOver = false;
    public D_SceneManager sManager;
    private bool isStart = true;

    public string[] Scenes = {"Level1", "Level2", "Level3", "Level4"};

    [Header("Background Info")]
    public float[] backgroundColors;
    public int colorListIndex;
    Color nextColor;
    Color camColor;
    bool changeColor = true;

    void Awake()
    {
        if (current == null)
            current = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
        if (SceneManager.GetActiveScene().name == "TutorialScene")
            return;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        sManager = GameObject.Find("SceneManager").GetComponent<D_SceneManager>();
        enemySpawner = GameObject.Find("SpawnerObj").GetComponent<D_EnemySpawner>();
        colorListIndex = 0;
        isStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (sManager == null)
            sManager = GameObject.Find("SceneManager").GetComponent<D_SceneManager>();
        if (enemySpawner == null)
            enemySpawner = GameObject.Find("SpawnerObj").GetComponent<D_EnemySpawner>();
        if (isStart)
        {
            StartCoroutine(LevelText());
            isStart = false;
        }
        else if (!Spawning && !isGameOver)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
            {
                if (WaveNumber % 7 == 0)
                {
                    WaveNumber++;
                    Spawning = true;
                    //show stage clear
                    Debug.Log("Start");
                    StartCoroutine(LevelClear());
                }
                else
                    nextWave();
            }
            if(changeColor == true)
            {
                StartCoroutine(lerpBackground());
                changeColor = false;
            }
        }
    }

    void nextWave()
    {
        WaveNumber++;
        enemySpawner.UpdateWave(WaveNumber);
        Debug.Log("Wave : " + WaveNumber);
    }

    public void SetSpawn(bool isSpawning)
    {
        Spawning = isSpawning;
    }

    public void setStart(bool start)
    {
        isStart = start;
    }

    public void Win()
    {
        isGameOver = true;
        PlayerMoveOffScreen moveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveOffScreen>();
        moveScript.SendPlayerToMars();
        //sManager.YouWin();
    }

    public void changeBackgroundColor()
    {
        // change background color
        //float[] newColor = backgroundColors[colorListIndex];
        nextColor = new Color(backgroundColors[colorListIndex] / 255f,
            backgroundColors[colorListIndex + 1] / 255f, backgroundColors[colorListIndex + 2] / 255f);
        camColor = Camera.main.GetComponent<Camera>().backgroundColor;
        //Camera.main.GetComponent<Camera>().backgroundColor = Color.Lerp(camColor, nextColor, 3f);
        changeColor = true;
        colorListIndex += 3;
        if (colorListIndex >= backgroundColors.Length)
            colorListIndex = 0;
    }

    IEnumerator lerpBackground()
    {
        float startTime = 0f;
        float endTime = 3f;
        while (startTime < endTime)
        {
            startTime += Time.deltaTime;
            Camera.main.GetComponent<Camera>().backgroundColor = Color.Lerp(camColor, nextColor, startTime/endTime);
            yield return null;
        }
        yield return null;
    }

    // show the level text when a new level starts
    IEnumerator LevelText()
    {
        LevelNumber++;
        Spawning = true;
        changeBackgroundColor();
        //D_SceneManager sManager = GameObject.Find("SceneManager").GetComponent<D_SceneManager>();
        yield return new WaitForSeconds(2f);
        sManager.ShowStageText(LevelNumber);
        yield return new WaitForSeconds(2f);
        sManager.ShowStageText(LevelNumber);

        yield return new WaitForSeconds(3f);
        nextWave();
    }

    IEnumerator LevelClear()
    {
        sManager.ShowStageClear();
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 500);
        sManager.ShowStageClear();
        yield return new WaitForSeconds(3f);

        try
        {
            GameObject.Find("Cat Yarn Ball").GetComponent<CircleCollider2D>().enabled = false;
        }
        catch
        {
            Debug.Log("No yarn ball");
        }

        PlayerMoveOffScreen moveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveOffScreen>();
        Debug.Log(LevelNumber);
        moveScript.NextSceneName(Scenes[LevelNumber]);
        moveScript.IsNotInPos();
    }
}