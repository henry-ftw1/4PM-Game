using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class multiballGun : genericPart
{   
    private GameObject Bullet;
    private float shootDelay = 0.5f;
    private bool shooting = false;
    [SerializeField]
    private D_BulletPooler pooler;


    // Start is called before the first frame update
    void Start()
    {
        pooler = GameObject.Find("SpawnerObj").GetComponent<D_BulletPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pooler == null)
        {
            pooler = GameObject.Find("SpawnerObj").GetComponent<D_BulletPooler>();
        }
        if (transform.parent != null && !shooting)
        {
            StartCoroutine(Shoot());
            shooting = true;
        }
    }
    protected override void Destroy()
    {
    }
    protected override IEnumerator Shoot()
    {
        while (this.isActiveAndEnabled)
        {
            for(int i = -2; i <= 2; i++)
            {
                Bullet = pooler.GetPooledObject(0);
                if(Bullet == null)
                {
                    Bullet = pooler.PoolMoreBullets(0);
                }
                Bullet.transform.position = transform.position;
                Bullet.transform.localRotation = transform.rotation;
                Bullet.transform.Rotate(0,0,i*15);
                Bullet.SetActive(true);
            }
            yield return new WaitForSeconds(shootDelay);
        }
    }
}

/*
public class multiballGun : D_GameEntity
{   
    private GameObject Bullet;
    private float shootDelay = 0.5f;
    private bool shooting = false;
    [SerializeField]
    private D_BulletPooler pooler;
    float curRot;
    float startRot;


    // Start is called before the first frame update
    void Start()
    {
        pooler = GameObject.Find("GameManager").GetComponent<D_BulletPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.parent != null && !shooting)
        {
            StartCoroutine(Shoot());
            shooting = true;
        }
    }

    protected override void Move()
    {

    }    protected override void Destroy()
    {
        Destroy(this.gameObject);
    }
    protected override IEnumerator Shoot()
    {
        while (this.isActiveAndEnabled)
        {
            for(int i = -2; i <= 2; i++)
            {
                Bullet = pooler.GetPooledObject(0);
                if(Bullet == null)
                {
                    Bullet = pooler.PoolMoreBullets(0);
                }
                Bullet.transform.position = transform.position;
                Bullet.transform.localRotation = transform.rotation;
                Bullet.transform.Rotate(0,0,i*15);
                Bullet.SetActive(true);
            }
            yield return new WaitForSeconds(shootDelay);
        }
    }
}
*/

