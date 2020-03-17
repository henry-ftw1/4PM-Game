using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns enemies at random points (in waves)
public class D_EnemySpawner : MonoBehaviour
{
    public static D_EnemySpawner current;

    [SerializeField]
    private int WaveNum = 1;
    //private int StageNumber = 1;
    public float EnemySpawnDelay = .3f;
    //public int EnemyCount = 1;
    //public int TotalEnemyCount = 1;
    //public int bossCount = 1;
    public GameObject YarnPrefab;
    public GameObject normalEnemyPrefab; //normal
    public GameObject rocketEnemyPrefab; //rocket
    public GameObject healerEnemyPrefab; //healer
    public GameObject CatBossPrefab; //rotating gun
    public GameObject MultiballBossPrefab; //multiball
    public GameObject CircleEnemyPrefab; //360 gun enemy
    public GameObject FinalBossPrefab;
    List<WaveDataScript> Waves = new List<WaveDataScript>();
    private D_GameManager gManager;
    public float SpawnXmin = -2f;
    public float SpawnXmax = 3f;

    [Header("Yarnball attributes")]
    public float YarnTimer = 60f;
    private float yTime = 20f;
    public float randYarnTime = 30f;
    public bool yarnSpawn = true;
    bool SpawningYarn = false;

    //private bool bossOne = false;
    void Awake()
    {
        if (current == null)
            current = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        gManager = GameObject.Find("GameManager").GetComponent<D_GameManager>();

        TextAsset WaveData = Resources.Load<TextAsset>("Waves");

        string[] arrData = WaveData.text.Split('\n');
        
        for (int i = 0; i < arrData.Length; i++)
        {
            string[] row = arrData[i].Split('\t');
            WaveDataScript wData = new WaveDataScript();
            int.TryParse(row[0], out wData.wave);
            int.TryParse(row[1], out wData.normalcount);
            int.TryParse(row[2], out wData.rocketcount);
            int.TryParse(row[3], out wData.catbosscount);
            int.TryParse(row[4], out wData.healercount);
            int.TryParse(row[5], out wData.multiballcount);
            int.TryParse(row[6], out wData.circlecount);
            int.TryParse(row[7], out wData.finalcount);
            Waves.Add(wData);
        }
        //Debug.Log(Waves[16].wave);

        yTime = YarnTimer;
        SpawningYarn = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // game manager checks if there should be next wave
    public void UpdateWave(int NextWave)
    {
        WaveNum = NextWave;
        gManager.SetSpawn(true);
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnYarn()
    {
        //Debug.Log("Called");
        while (this.isActiveAndEnabled && yarnSpawn)
        {
            yield return new WaitForSeconds(yTime);
            //Debug.Log("Spawnball");
            Instantiate(YarnPrefab, new Vector3(Random.Range(-4, 4), 15, 0), Quaternion.identity);
            //float newRandTime = Random.Range(-randYarnTime, randYarnTime);
            yTime = Random.Range(YarnTimer - randYarnTime, YarnTimer + randYarnTime);
        }
        yield return null;
    }

    IEnumerator SpawnEnemies()
    {
        WaveDataScript data = Waves[WaveNum];
        if(data.wave == 0)
        {
            Debug.Log("WIN");
            yield return new WaitForSeconds(2f);
            gManager.Win();
            gManager.SetSpawn(false);
            yield return null;
        }
        if(WaveNum >= 9 && !SpawningYarn)
        {
            SpawningYarn = true;
            StartCoroutine(SpawnYarn());
        }
        StartCoroutine(SpawnHealers(data));
        if (data.finalcount > 0)
            Instantiate(FinalBossPrefab, new Vector3(0, 8, 0), Quaternion.identity);
        for (int i = 0; i < data.normalcount; i++)
        {
            Instantiate(normalEnemyPrefab, new Vector3(Random.Range(SpawnXmin, SpawnXmax), 8, 0), Quaternion.identity);
            yield return new WaitForSeconds(EnemySpawnDelay);
        }
        for (int i = 0; i < data.rocketcount; i++)
        {
            Instantiate(rocketEnemyPrefab, new Vector3(Random.Range(SpawnXmin, SpawnXmax), 8, 0), Quaternion.identity);
            yield return new WaitForSeconds(EnemySpawnDelay);
        }
        for (int i = 0; i < data.circlecount; i++)
        {
            Instantiate(CircleEnemyPrefab, new Vector3(Random.Range(SpawnXmin, SpawnXmax), 8, 0), Quaternion.identity);
            yield return new WaitForSeconds(EnemySpawnDelay + 3f);
        }
        for (int i = 0; i < data.catbosscount; i++)
        {
            Instantiate(CatBossPrefab, new Vector3(Random.Range(SpawnXmin, SpawnXmax), 8, 0), Quaternion.identity);
            yield return new WaitForSeconds(EnemySpawnDelay + 4f);
        }
        for (int i = 0; i < data.multiballcount; i++)
        {
            Instantiate(MultiballBossPrefab, new Vector3(Random.Range(SpawnXmin, SpawnXmax), 8, 0), Quaternion.identity);
            yield return new WaitForSeconds(EnemySpawnDelay+4f);
        }
        gManager.SetSpawn(false);
        yield return null;
    }

    IEnumerator SpawnHealers(WaveDataScript data)
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < data.healercount; i++)
        {
            Instantiate(healerEnemyPrefab, new Vector3(Random.Range(SpawnXmin, SpawnXmax), 8, 0), Quaternion.identity);
            yield return new WaitForSeconds(EnemySpawnDelay+1f);
        }
        yield return null;
    }

    public void TurnoffYarn()
    {
        yarnSpawn = false;
    }
}
