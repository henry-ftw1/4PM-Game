using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalBossGun : D_GameEntity
{
    public GameObject bullet;
    public GameObject explosionPrefab;
    private float shootDelay = 2.5f;
    private bool shooting = false;
    [SerializeField]
    //private D_BulletPooler pooler;

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
    
    public float moveSpeed = 3f;
    public float moveSpeedDiff = 1f;



    // Start is called before the first frame update
    void Start()
    {
        //pooler = GameObject.Find("SpawnerObj").GetComponent<D_BulletPooler>();
        hBarScript = GetComponent<HealthBar>();

        moveSpeed = Random.Range(moveSpeed - moveSpeedDiff, moveSpeed + moveSpeedDiff);

        MovePoints = GameObject.Find("EnemyMovements");
        Transform parent = MovePoints.transform.Find("FinalBossGun");
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
        if(!shooting)
        {
           StartCoroutine(Shoot());
           print("Shooting");
           shooting = true;
        }
    }
    protected override void Destroy()
    {
        startExplosion();
        //spawnObject();
        //audioManager.instance.playClip(deathSound);
        //PlayerPrefs.SetInt("EnemyKills", PlayerPrefs.GetInt("EnemyKills") + 1);
        //PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + (int)points);
        //Debug.Log(points);
        Destroy(this.gameObject);
    }

    void startExplosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(explosion, 3);
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

    protected override IEnumerator Shoot()
    {
        while (this.isActiveAndEnabled)
        {

            for(int i = 0; i <= 3; i++)
            {
                Instantiate(bullet, transform.position, transform.rotation);
                Instantiate(bullet, (transform.position + new Vector3(1, 0, 0)), (Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z + 30)));
                Instantiate(bullet, (transform.position + new Vector3(-1, 0, 0)), (Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z - 30)));

                yield return new WaitForSeconds(1f);
            }

            yield return new WaitForSeconds(shootDelay);
            
        }
    }
}