using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class for player and enemies
public abstract class D_GameEntity : MonoBehaviour
{
    [SerializeField]
    protected float Health;
    [SerializeField]
    protected float StartHealth;
    [SerializeField]
    protected HealthBar hBarScript;
    [SerializeField]
    protected float SpawnInvTime;
    protected abstract void Move();
    protected AudioSource[] Sounds;
    //public abstract void TakeHit(float dmg);
    public void TakeHit(float dmg)
    {
        if (SpawnInvTime > 0)
            return;
        Health -= dmg;
        hBarScript.SetHealthBar(Health);
        if (Health <= 0)
        {
            Destroy();
        }
    }
    public void increaseHealth(float h)
    {
        StartCoroutine(healEffect());
        if (Health + h <= hBarScript.startHealth)
        {
            Health += h;
            hBarScript.SetHealthBar(Health);
        }
    }
    protected abstract void Destroy();
    protected abstract IEnumerator Shoot();
    public float getStartHealth()
    {
        return StartHealth;
    }
    public float getHealth()
    {
        return Health;
    }

    protected IEnumerator healEffect()
    {
        ParticleSystem healEmittor = transform.Find("HealParticle").gameObject.GetComponent<ParticleSystem>();
        if (!healEmittor.isPlaying)
        {
            healEmittor.Play();
        }
        yield return new WaitForSeconds(1f);
        if (!healEmittor.isStopped)
        {
            healEmittor.Stop();
        }
        yield return null;
    }

    protected bool isAround(Vector3 pos, Vector3 nextPoint)
    {
        if (pos.x >= nextPoint.x - 0.1 && pos.x <= nextPoint.x + 0.1)
        {
            if (pos.y >= nextPoint.y - 0.1 && pos.y <= nextPoint.y + 0.1)
            {
                return true;
            }
        }
        return false;
    }
}
