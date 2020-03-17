using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiballEnemy : D_GameEntity
{
    public float moveSpeed = 3f;
    public float moveSpeedDiff = 1f;
    [SerializeField]
    private float shootDelay = 1f;
    public GameObject bulletPrefab;
    public GameObject gunPrefab;
    public GameObject explosionPrefab;
    private bool shooting = false;

    [Header("Movement")]
    public float YPos = 8f;

    public GameObject MovePoints;
    public Transform[] SpecificMovePoints;
    private bool AtPoint = true;
    private Vector3 point;
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
        Transform parent = MovePoints.transform.Find("MultiballBoss");
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
        if(SpawnInvTime > 0)
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
        if(transform.position.y > YPos)
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
        spawnObject();
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

    void spawnObject()
    {
        Instantiate(gunPrefab, transform.position, Quaternion.Euler(0, 0, 0));
    }

    protected override IEnumerator Shoot()
    {
        //spawn a bullet
        yield return new WaitForSeconds(0.7f);
        while (this.isActiveAndEnabled)
        {
            for(int i = -2; i <= 2; i++)
            {
                GameObject bullet = pooler.GetPooledObject(5);
                if (bullet == null)
                {
                    bullet = pooler.PoolMoreBullets(5);
                }
                bullet.transform.position = transform.position;
                bullet.transform.localRotation = Quaternion.Euler(0, 0, -180);
                bullet.transform.Rotate(0, 0, i*15);
                bullet.SetActive(true);
            }
            yield return new WaitForSeconds(shootDelay);
        }
        yield return null;
    }
}

