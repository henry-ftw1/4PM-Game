using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldPart : genericPart
{
    public Sprite bone1;
    public Sprite bone2;
    public Sprite bone3;

    SpriteRenderer srend;

    private void Start()
    {
        if (bone1 == null || bone2 == null || bone3 == null)
            Debug.Log("Add bone sprites");
        srend = GetComponent<SpriteRenderer>();
        srend.sprite = bone1;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Destroy()
    {

    }

    protected override IEnumerator Shoot()
    {
        return null;
    }

    public new void TakeHit(float dmg)
    {
        partHealth -= dmg;

        if (partHealth == 1)
            srend.sprite = bone2;
        else if (partHealth == 0)
            srend.sprite = bone3;

        if (partHealth <= 0)
        {
            Destroy();
        }
    }
}
