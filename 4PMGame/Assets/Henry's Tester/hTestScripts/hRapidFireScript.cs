using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hRapidFireScript : genericPart
{
    //public float gunHealth = 5f;
    private GameObject rfBullet;
    private float shootDelay = 0.3f;
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
        if(pooler == null)
        {
            pooler = GameObject.Find("SpawnerObj").GetComponent<D_BulletPooler>();
        }
        if(transform.parent != null && !shooting)
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
            rfBullet = pooler.GetPooledObject(2);
            if(rfBullet == null)
            {
                rfBullet = pooler.PoolMoreBullets(2);
            }
            rfBullet.transform.position = transform.position;
            rfBullet.transform.localRotation = transform.rotation;
            rfBullet.SetActive(true);

            //Instantiate(rapidFireBullet, transform.position, transform.rotation);
            yield return new WaitForSeconds(shootDelay);
            
        }
    }
}
