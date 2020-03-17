using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_CircleEnemy : D_EnemyScript
{
    [Header("360 Enemy Stats")]
    public float burstDelay = 0.5f;
    public int burstAmount = 3;
    public int pointIndex = 1;

    protected override void Move()
    {
        // move enemy into position
        if (transform.position.y > YPos)
        {
            transform.Translate(0, -3 * Time.deltaTime, 0);
        }
        else if (AtPoint)
        {
            point = new Vector3(SpecificMovePoints[pointIndex].position.x, SpecificMovePoints[pointIndex].position.y);
            AtPoint = false;
            if (pointIndex == PointsLength-1)
            {
                pointIndex = 1;
            }
            else
            {
                pointIndex++;
            }
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
        yield return new WaitForSeconds(0.7f);
        while (this.isActiveAndEnabled)
        {
            for (int burst = 0; burst <= burstAmount; burst++)
            {
                for (int i = 0; i <= 11; i++)
                {
                    GameObject bullet = pooler.GetPooledObject(1);
                    if (bullet == null)
                    {
                        bullet = pooler.PoolMoreBullets(1);
                    }
                    bullet.transform.position = transform.position;
                    bullet.transform.localRotation = Quaternion.Euler(0, 0, -180);
                    bullet.transform.Rotate(0, 0, i * 30);
                    bullet.SetActive(true);
                }
                yield return new WaitForSeconds(burstDelay);
            }
            yield return new WaitForSeconds(shootDelay);
        }
    }
}
