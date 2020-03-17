using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFireGun : genericPart
{
    private GameObject rocketBullet;
    public float shootDelay = 4f;
    private bool shooting = false;
    public float BulletAmount = 2f;
    public int bulletIndexInPooler = 3;
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
        //Move();
    }

    /***
    private Quaternion getEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return Quaternion.Euler(new Vector3(0,0,offset+90));
        Vector3 firstEnemy = enemies[0].transform.position;
        //Vector3 player = GameObject.Find("D_player").transform.position;
        //Vector3 targetPosFlattened = new Vector3(player.position.x, player.position.y, 0);

        //transform.LookAt(firstEnemy);

        //targetPos = target.position;
        Vector3 thisPos = transform.position;
        firstEnemy.x = firstEnemy.x - thisPos.x;
        firstEnemy.y = firstEnemy.y - thisPos.y;
        float angle = Mathf.Atan2(firstEnemy.y, firstEnemy.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, 0, angle + offset));
    }
    ***/
    protected override void Destroy()
    {
        //Destroy(this.gameObject);
    }
    protected override IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.7f);
        while (this.isActiveAndEnabled)
        {
            for (int i = 0; i < BulletAmount; i++)
            {
                rocketBullet = pooler.GetPooledObject(bulletIndexInPooler);
                if (rocketBullet == null)
                {
                    rocketBullet = pooler.PoolMoreBullets(bulletIndexInPooler);
                }
                rocketBullet.transform.position = transform.position;
                rocketBullet.transform.localRotation = transform.rotation;
                rocketBullet.SetActive(true);
                yield return new WaitForSeconds(0.5f);
            }

            //Instantiate(rapidFireBullet, transform.position, transform.rotation);
            yield return new WaitForSeconds(shootDelay);

        }
    }
}
