using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGunScript : genericPart
{
    private GameObject Bullet;
    public float shootDelay = 0.8f;
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
            for (int i = 0; i <= 11; i++)
            {
                GameObject bullet = pooler.GetPooledObject(0);
                if (bullet == null)
                {
                    bullet = pooler.PoolMoreBullets(0);
                }
                bullet.transform.position = transform.position;
                bullet.transform.localRotation = Quaternion.Euler(0, 0, -180);
                bullet.transform.Rotate(0, 0, i * 30);
                bullet.SetActive(true);
            }
            yield return new WaitForSeconds(shootDelay);
        }
    }
}
