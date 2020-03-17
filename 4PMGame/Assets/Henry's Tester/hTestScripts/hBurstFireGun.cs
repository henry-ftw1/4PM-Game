using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hBurstFireGun : D_GameEntity
{
    //public float gunHealth = 10f;
    public GameObject bulletPrefab;
    private float shootDelay = 0.3f;
    private bool shooting = false;
    public int numBullets = 3;
    float bulletCooldownTime = 0f;



    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.parent != null && transform.parent.name == "player") && !shooting && (Time.time - bulletCooldownTime > 10) )
        {
            numBullets = 3;
            StartCoroutine(Shoot());
            shooting = true;
        }
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
        Destroy(this.gameObject);
    }
    protected override IEnumerator Shoot()
    {
        while (this.isActiveAndEnabled)
        {
            for(int i = 0; i < numBullets; i++)
            {
                Instantiate(bulletPrefab, transform.position, transform.rotation);
                yield return new WaitForSeconds(shootDelay);
            }
            bulletCooldownTime = Time.time;
            shooting = false;
            yield return null;
        }
    }
}
