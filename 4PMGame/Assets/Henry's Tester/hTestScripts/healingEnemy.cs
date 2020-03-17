using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healingEnemy : D_GameEntity
{
    public float moveSpeed = 5f;
    public float moveSpeedDiff = 1f;
    //[SerializeField]
    //private float Health = 5f;
    //[SerializeField]
    //private float shootDelay = 1f;
    //public GameObject bulletPrefab;
    //public GameObject gunPrefab;
    //public GameObject shieldPrefab;
    public GameObject healthPrefab;
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

    public float healTimer = 3f;
    public float healAmount = 2f;

    public float points = 32f;
    public float pointsMultiplier = 2f;

    [Header ("Sounds")]
    [SerializeField]
    private AudioClip deathSound = null;

    //[SerializeField]
    //private D_BulletPooler pooler;
    // Start is called before the first frame update
    void Start()
    {
        hBarScript = GetComponent<HealthBar>();
        //pooler = GameObject.Find("GameManager").GetComponent<D_BulletPooler>();
        MovePoints = GameObject.Find("EnemyMovements");
        Transform parent = MovePoints.transform.Find("HealerEnemy");
        SpecificMovePoints = parent.GetComponentsInChildren<Transform>();

        if (SpecificMovePoints == null)
        {
            Debug.Log("NEED POINTS");
        }
        PointsLength = SpecificMovePoints.Length;
        //YPos = Random.Range(MoveToYPosRand - YPosDiff, MoveToYPosRand + YPosDiff);
        moveSpeed = Random.Range(moveSpeed - moveSpeedDiff, moveSpeed + moveSpeedDiff);
        StartCoroutine(Shoot());
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
            if (points > 20f)
            {
                points -= Time.deltaTime * pointsMultiplier;
            }
            else
            {
                points = 20f;
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
        spawnObject();
        audioManager.instance.playClip(deathSound);
        PlayerPrefs.SetInt("EnemyKills", PlayerPrefs.GetInt("EnemyKills") + 1);
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + (int)points);
        Destroy(this.gameObject);
    }

    void startExplosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(explosion, 1);
    }

    void spawnObject()
    {
        Instantiate(healthPrefab, transform.position, Quaternion.Euler(0, 0, -180));
    }

    protected override IEnumerator Shoot()
    {
        //Heals its allies
        yield return new WaitForSeconds(healTimer);
        while (this.isActiveAndEnabled)
        {
            GameObject[] allies = GameObject.FindGameObjectsWithTag("Enemy");
            for(int i = 0; i<allies.Length; i++)
            {
                allies[i].GetComponent<D_GameEntity>().increaseHealth(healAmount);
            }
            yield return new WaitForSeconds(healTimer);
        }
        yield return null;
    }

}
