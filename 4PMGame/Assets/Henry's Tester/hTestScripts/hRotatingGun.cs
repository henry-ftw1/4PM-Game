using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hRotatingGun : D_GameEntity
{   
    private GameObject rfBullet;
    private float shootDelay = 0.5f;
    private bool shooting = false;
    [SerializeField]
    private D_BulletPooler pooler;
    private bool flipRot;
    float curRot;
    float startRot;


    // Start is called before the first frame update
    void Start()
    {
        pooler = GameObject.Find("SpawnerObj").GetComponent<D_BulletPooler>();


        curRot = this.transform.localEulerAngles.z;
        startRot = curRot;        

        if(startRot > 180)
            flipRot = true;
        else
            flipRot = false;
        //Debug.Log("StartRot" + startRot + "Name: " + name + " flipRot?: " + flipRot);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.parent != null && !shooting)
        {
            StartCoroutine(Shoot());
            shooting = true;
        }
        rotateGun();
    }

    protected override void Move()
    {

    }
    /*
    public override void TakeHit(float dmg)
    {
        gunHealth -= dmg;
        if (gunHealth <= 0)
        {
            Destroy();
        }
    }
    */
    protected override void Destroy()
    {
    }
    private void rotateGun()
    {
        curRot = this.transform.localEulerAngles.z;
        if(curRot >= 90 && !flipRot)
        {
            transform.Rotate(0, 0, Time.deltaTime * 50);
            if(curRot >= 240)
                flipRot = !flipRot;
        }
        if(curRot <= 270 && flipRot)
        {
            transform.Rotate(0, 0, -Time.deltaTime * 50);
            if(curRot <= 120)
                flipRot = !flipRot;
        }
    }
    protected override IEnumerator Shoot()
    {
        while (this.isActiveAndEnabled)
        {
            rfBullet = pooler.GetPooledObject(5);
            if(rfBullet == null)
            {
                rfBullet = pooler.PoolMoreBullets(5);
            }
            rfBullet.transform.position = transform.position;
            rfBullet.transform.localRotation = transform.rotation;
            rfBullet.SetActive(true);
            
            //Instantiate(rapidFireBullet, transform.position, transform.rotation);
            yield return new WaitForSeconds(shootDelay);
            
        }
    }
}
