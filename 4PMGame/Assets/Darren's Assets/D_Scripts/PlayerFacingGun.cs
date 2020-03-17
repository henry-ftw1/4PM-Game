using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFacingGun : D_GameEntity
{
    protected GameObject bullet;
    public float offset = -90f;
    [SerializeField]
    protected float shootDelay = 0.5f;
    [SerializeField]
    protected float delayBetweenShots = 0.1f;
    protected bool shooting = false;
    public float BulletAmount = 3f;
    public int bulletIndexInPooler = 3;
    [SerializeField]
    protected D_BulletPooler pooler;


    // Start is called before the first frame update
    void Start()
    {
        pooler = GameObject.Find("SpawnerObj").GetComponent<D_BulletPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null && !shooting)
        {
            StartCoroutine(Shoot());
            shooting = true;
        }
        Move();
    }

    protected override void Move()
    {
        // Look at Player
        try
        {
            Vector3 player = GameObject.Find("D_player").transform.position;
            //Vector3 targetPosFlattened = new Vector3(player.position.x, player.position.y, 0);
            transform.LookAt(player);

            //targetPos = target.position;
            Vector3 thisPos = transform.position;
            player.x = player.x - thisPos.x;
            player.y = player.y - thisPos.y;
            float angle = Mathf.Atan2(player.y, player.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
        }
        catch
        {
            Debug.Log("Cannot find player");
        }
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
        //Destroy(this.gameObject);
    }

    protected override IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.7f);
        while (this.isActiveAndEnabled)
        {
            for(int i = 0; i < BulletAmount; i++)
            {
                bullet = pooler.GetPooledObject(bulletIndexInPooler);
                if (bullet == null)
                {
                    bullet = pooler.PoolMoreBullets(bulletIndexInPooler);
                }
                bullet.transform.position = transform.position;
                bullet.transform.localRotation = transform.rotation;
                bullet.SetActive(true);
                yield return new WaitForSeconds(delayBetweenShots);
            }

            //Instantiate(rapidFireBullet, transform.position, transform.rotation);
            yield return new WaitForSeconds(shootDelay);

        }
    }
}
