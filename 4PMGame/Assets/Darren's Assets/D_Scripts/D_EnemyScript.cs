using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy class script
public class D_EnemyScript : D_GameEntity
{
    public float moveSpeed = 3f;
    public float moveSpeedDiff = 1f;
    //[SerializeField]
    //private float Health = 5f;
    [SerializeField]
    protected float shootDelay = 1f;
    public GameObject bulletPrefab;
    public GameObject gunPrefab;
    public GameObject shieldPrefab;
    public GameObject healthPrefab;
    public GameObject explosionPrefab;

    private bool shooting = false;
    //private bool isMovingRight;
    private IEnumerator shootingCoroutine;
    /***
    [Header("X ping pong")]
    private float XposMin = -3.5f;
    private float XposMax = 3.5f;
    public float XPosRange = 1.5f; //cannot be greater than 3.5
    public float xPos;
    ***/
    [Header("Y Position And Movement")]
    /***
    [SerializeField]
    private float MoveToYPosRand = 5f;
    [SerializeField]
    private float YPosDiff = 2f;***/
    public float YPos = 8f;

    public GameObject MovePoints;
    public Transform[] SpecificMovePoints;
    protected bool AtPoint = true;
    protected Vector3 point;
    protected int PointsLength;
    public float MoveTime = 10f;
    public float MoveTimeMax = 10f;

    public float points = 32f;
    public float pointsMultiplier = 2f;

    [SerializeField]
    protected D_BulletPooler pooler;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip deathSound = null;

    // Start is called before the first frame update
    void Start()
    {
        hBarScript = GetComponent<HealthBar>();
        pooler = GameObject.Find("SpawnerObj").GetComponent<D_BulletPooler>();
        /***
        if (Random.Range(0, 2) == 1)
        {
            isMovingRight = true;
        }
        else
        {
            isMovingRight = false;
        }***/
        YPos = 8f;
        //YPos = Random.Range(MoveToYPosRand - YPosDiff, MoveToYPosRand + YPosDiff);
        moveSpeed = Random.Range(moveSpeed - moveSpeedDiff, moveSpeed + moveSpeedDiff);
        /***
        if (XPosRange > 3.5)
            XPosRange = 3.5f;
        xPos = this.transform.position.x;
        XPosRange = Random.Range(0, XPosRange);
        if(XPosRange < 1.5f)
        {
            XPosRange = 1.5f;
        }***/
        MovePoints = GameObject.Find("EnemyMovements");
        if (this.name.Contains("D_EnemyFacing"))
        {
            Transform parent = MovePoints.transform.Find("RocketEnemy");
            SpecificMovePoints = parent.GetComponentsInChildren<Transform>();
        }
        else if (this.name.Contains("D_EnemyCircle"))
        {
            Transform parent = MovePoints.transform.Find("CircleEnemy");
            SpecificMovePoints = parent.GetComponentsInChildren<Transform>();
        }
        else if (this.name.Contains("D_Enemy"))
        {
            Transform parent = MovePoints.transform.Find("NormalEnemy");
            SpecificMovePoints = parent.GetComponentsInChildren<Transform>();
        }
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
            shootingCoroutine = Shoot();
            StartCoroutine(shootingCoroutine);
            shooting = true;
        }
        if(SpawnInvTime > 0)
        {
            SpawnInvTime--;
        }
        else
        {
            if (points > 10f)
            {
                points -= Time.deltaTime * pointsMultiplier;
            }
            else
            {
                points = 10f;
            }
        }
    }

    protected override void Move()
    {
        // move enemy into position
        if(transform.position.y > YPos)
        {
            transform.Translate(0, -3 * Time.deltaTime, 0);
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
                if(isAround(this.transform.position, point))
                {
                    AtPoint = true;
                    MoveTime = MoveTimeMax;
                }
            }
        }
    }
    /*
    public override void TakeHit(float dmg)
    {
        Health -= dmg;
        if (Health <= 0)
        {
            Destroy();
        }
    }
    */
    protected override void Destroy()
    {
        startExplosion();
        spawnObject();
        audioManager.instance.playClip(deathSound);
        PlayerPrefs.SetInt("EnemyKills", PlayerPrefs.GetInt("EnemyKills") + 1);
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + (int)points);
        Destroy(this.gameObject);

        /*Sounds[0].Play();
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        StopCoroutine(shootingCoroutine);
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        Destroy(this.gameObject, Sounds[0].clip.length);*/
    }

    void startExplosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(explosion, 1);
    }
    
    void spawnObject()
    {
        float randNum = Random.Range(0.0f, 1.0f);
        if(randNum > 0.75 && randNum <= 0.92)
        {
            Instantiate(gunPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        }
        else if(randNum >= 0.2 && randNum <= 0.75)
        {
            Instantiate(shieldPrefab, transform.position, Quaternion.Euler(0, 0, -180));
        }
        else if(randNum > 0.92)
        {
            Instantiate(healthPrefab, transform.position, Quaternion.Euler(0, 0, -180));
        }
    }

    protected override IEnumerator Shoot()
    {
        //spawn a bullet
        yield return new WaitForSeconds(0.7f);
        while (this.isActiveAndEnabled)
        {
            GameObject bullet = pooler.GetPooledObject(1);
            if (bullet == null)
            {
                bullet = pooler.PoolMoreBullets(1);
            }
            bullet.transform.position = transform.position;
            bullet.transform.localRotation = Quaternion.Euler(0, 0, -180);
            bullet.SetActive(true);
            //Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, -180));
            yield return new WaitForSeconds(shootDelay);
        }
        yield return null;
    }

    
}
