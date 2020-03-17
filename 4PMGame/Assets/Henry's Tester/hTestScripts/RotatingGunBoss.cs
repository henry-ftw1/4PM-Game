using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingGunBoss : D_GameEntity
{
    public float moveSpeed = 3f;
    public float moveSpeedDiff = 1f;
    [SerializeField]
    public GameObject bonePrefab;
    public GameObject explosionPrefab;
    private bool shooting = false;

    [Header("Movement")]
    public float YPos = 8f;

    public GameObject MovePoints;
    public Transform[] SpecificMovePoints;
    public bool AtPoint = true;
    public Vector3 point;
    private int PointsLength;
    public float MoveTime = 10f;
    public float MoveTimeMax = 10f;

    public float points = 200f;
    public float pointsMultiplier = 1.1f;

    [SerializeField]
    private D_BulletPooler pooler;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip deathSound = null;

    // Start is called before the first frame update
    void Start()
    {
        hBarScript = GetComponent<HealthBar>();
        pooler = GameObject.Find("SpawnerObj").GetComponent<D_BulletPooler>();
        moveSpeed = Random.Range(moveSpeed - moveSpeedDiff, moveSpeed + moveSpeedDiff);

        MovePoints = GameObject.Find("EnemyMovements");
        Transform parent = MovePoints.transform.Find("CatBoss");
        SpecificMovePoints = parent.GetComponentsInChildren<Transform>();
        
        if (SpecificMovePoints == null)
        {
            Debug.Log("NEED POINTS");
        }
        PointsLength = SpecificMovePoints.Length;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (!shooting)
        {
            StartCoroutine(Shoot());
            shooting = true;
        }
        if (SpawnInvTime > 0)
        {
            SpawnInvTime--;
        }
        else
        {
            if (points > 30f)
            {
                points -= Time.deltaTime * pointsMultiplier;
            }
            else
            {
                points = 30f;
            }
        }
    }

    protected override void Move()
    {
        // move enemy into position
        if (transform.position.y > YPos)
        {
            transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
        }
        else if (AtPoint)
        {
            int rand = Random.Range(1, PointsLength);
            point = new Vector3(SpecificMovePoints[rand].position.x, SpecificMovePoints[rand].position.y);
            AtPoint = false;
            //transform.position = Vector2.MoveTowards(transform.position, point, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, point, moveSpeed * Time.deltaTime);
            MoveTime--;
            if (MoveTime <= 0)
            {
                if (isAround(this.transform.position, point))
                {
                    AtPoint = true;
                    MoveTime = MoveTimeMax;
                }
            }
        }
    }
    protected override void Destroy()
    {
        startExplosion();
        spawnObject(new Vector3(-1,0,0));
        spawnObject(new Vector3(1,0,0));
        audioManager.instance.playClip(deathSound);
        PlayerPrefs.SetInt("EnemyKills", PlayerPrefs.GetInt("EnemyKills") + 1);
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + (int)points);
        Debug.Log(points);
        Destroy(this.gameObject);
    }

    void startExplosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(explosion, 3);
    }

    void spawnObject(Vector3 addPos)
    {
        Instantiate(bonePrefab, transform.position+addPos, Quaternion.Euler(0, 0, 0));
    }

    protected override IEnumerator Shoot()
    {
        yield return null;
    }
}
