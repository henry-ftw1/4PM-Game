using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class D_Bullet : MonoBehaviour
{
    [SerializeField]
    protected float moveSpeed = 5f;
    [SerializeField]
    protected float CollisionTimer = 20f;
    public float Damage = 1f;
    void Update()
    {
        bulletMove();
        if (CollisionTimer > 0)
        {
            CollisionTimer--;
        }
    }
    protected void bulletMove()
    {
        this.gameObject.transform.Translate(0, moveSpeed*Time.deltaTime, 0);
    }
}
