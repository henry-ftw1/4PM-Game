using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class genericPart : MonoBehaviour
{
    public float partHealth;
    public float maxHealth;
    [SerializeField]
    private Vector3 destroyedColor = new Vector3(0.5f, 0f, 0f);

    public void TakeHit(float dmg)
    {
        partHealth -= dmg;

        float colorRatio = partHealth/maxHealth;
        float curRed = destroyedColor.x + (1f - destroyedColor.x) * colorRatio;
        float curGreen = destroyedColor.y + (1f - destroyedColor.y) * colorRatio;
        float curBlue = destroyedColor.z + (1f - destroyedColor.z) * colorRatio;
        GetComponent<SpriteRenderer>().color = new Color(curRed, curGreen, curBlue);

        if (partHealth <= 0)
        {
            Destroy();
        }
    }
    protected abstract void Destroy();
    protected abstract IEnumerator Shoot();
    public float getStartHealth()
    {
        return partHealth;
    }

    public void setStartHealth(float newHealth)
    {
        partHealth = newHealth;
        maxHealth = newHealth;
    }
}
