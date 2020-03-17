using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loosePart : MonoBehaviour
{

    public float fallSpeed = 1f;
    public float lowRotationSpeed = 150f;
    public float highRotationSpeed = 200f;
    public float launchSpeed = 8f;
    public float detachSpeed = 2f;

    private Rigidbody2D rb;
    private FixedJoint2D joint;
    //private Vector2 vel = Vector2.zero;

    private Transform playerReference;
    [SerializeField]
    private bool attached = false;
    public bool playerEjected = false;
    public bool canAttach = true;

    [Header("Sounds")]
    public AudioClip hitSound = null;

    public float loosePartDamage = 3f;

    private bool pickedUpOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, -fallSpeed);
        rb.angularVelocity = Random.Range(lowRotationSpeed, highRotationSpeed);
        joint = GetComponent<FixedJoint2D>();
        //vel = new Vector2(0f, -fallSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (attached) {
            genericPart part = this.transform.GetComponent<genericPart>();
            if (other.tag == "EnemyBullet" && part != null)
            {
                if(part.partHealth <= 0) 
                {
                    flyAway(false);
                }
                else
                {
                    if (this.gameObject.name.Contains("BoneShield") || this.gameObject.name.Contains("HardBone"))
                        this.transform.GetComponent<shieldPart>().TakeHit(1);
                    else
                        this.transform.GetComponent<genericPart>().TakeHit(1);
                }
                other.gameObject.SetActive(false);
            }
            if (tag == "PlayerPart" && other.tag == "Player" && canAttach) {
                reverseTree();
                attachToPlayer(other.transform);
            }
        }
        else if (other.tag == "Player" && canAttach) 
        {
            attachToPlayer(other.transform);
            if (!pickedUpOnce)
            {
                PlayerPrefs.SetInt("ItemsCollected", PlayerPrefs.GetInt("ItemsCollected") + 1);
                pickedUpOnce = true;
            }
        }
        else if (other.tag == "EnemyBullet") {
            foreach (Transform child in transform) {
                child.GetComponent<loosePart>().flyAway(false);
            }
            if(this.gameObject.tag == "PlayerPart")
            {
                other.gameObject.SetActive(false);
            }
        }
        else if(other.tag == "Enemy" && this.gameObject.tag == "PlayerPart")
        {
            if(playerEjected) {
                other.gameObject.GetComponent<D_GameEntity>().TakeHit(loosePartDamage);
                audioManager.instance.playClip(hitSound);
            }
            //Destroy(this);
        }
    }

    private void attachToPlayer(Transform other) {
        playerEjected = false;
        tagTree(true);
        attached = true;
        transform.SetParent(other);

        ((Behaviour)GetComponent("Halo")).enabled = false;

        if (!playerReference) {
            playerReference = other;
            while (playerReference.parent != null)
                playerReference = playerReference.parent;
        }
        playerReference.GetComponent<D_playerScript>().PlayAttach();




        //AWAY FROM SHIP
        /*float newAngle = Mathf.Atan((transform.position.y - playerReference.position.y) / (transform.position.x - playerReference.position.x)) * Mathf.Rad2Deg - 90;
        if (transform.position.x - playerReference.position.x < 0)
            newAngle += 180;*/

        //SPECIFICALLY DIRECT GUNS FORWARD
        /*if (this.gameObject.name.Contains("Gun"))
        {
            newAngle = 0;
        }*/

        //rb.SetRotation(newAngle);
        //transform.eulerAngles = new Vector3(0, 0, Mathf.Atan((transform.position.y - playerReference.position.y) / (transform.position.x - playerReference.position.x)) * Mathf.Rad2Deg);


        //vel = Vector2.zero;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;

        joint.connectedBody = other.GetComponent<Rigidbody2D>();
        joint.enabled = true;

        this.GetComponent<ParticleSystem>().Play();
    }

    private void tagTree(bool toPlayer) {

        if (toPlayer)
        {

            //this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0f, 1f);
            gameObject.tag = "Player";
        }
        else
            gameObject.tag = "PlayerPart";

        foreach (Transform child in transform) {
            loosePart childPart = child.GetComponent<loosePart>();
            childPart.playerEjected = playerEjected;
            childPart.tagTree(toPlayer);
        }
    }

    private void reverseTree() {
        if (transform.parent != null && transform.parent != playerReference) {
            loosePart partScript = transform.parent.GetComponent<loosePart>();
            partScript.reverseTree();

            if (transform.parent.parent == null) {
                //partScript.vel = Vector2.zero;
                partScript.rb.velocity = Vector2.zero;
                partScript.rb.angularVelocity = 0;
                partScript.attached = true;
            }

            partScript.joint.connectedBody = rb;
            partScript.joint.enabled = true;

            Transform parentReference = transform.parent;
            transform.parent = null;
            parentReference.parent = transform;
        }
    }

    public void flyAway(bool wasEjected) {
        Vector2 launchVel = Vector2.zero;
        attached = false;
        tag = "PlayerPart";

        if (wasEjected) {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.9f, 0f);
            this.gameObject.GetComponent<TrailRenderer>().enabled = true;

            launchVel = Vector2.up * launchSpeed;
            //launchVel = new Vector2(Mathf.Cos(launchAngle), Mathf.Sin(launchAngle)) * launchSpeed;

            canAttach = false;
        }
        else {
            launchVel = new Vector2(transform.position.x - playerReference.position.x, transform.position.y - playerReference.position.y).normalized * detachSpeed;

            if (GetComponent<genericPart>().partHealth > 0) {
                tag = "Untagged";
                canAttach = true;
                ((Behaviour)GetComponent("Halo")).enabled = true;
            }
            else {
                canAttach = false;
                launchVel = launchVel * 2f;
            }
        }

        playerEjected = wasEjected;
        List<loosePart> parts = new List<loosePart>();
        foreach (Transform child in transform) {
            parts.Add(child.GetComponent<loosePart>());
        }
        for (int i = 0; i < parts.Count; ++i) {
            parts[i].flyAway(wasEjected);
        }
        //tagTree(false);
        transform.parent = null;
        //vel = launchVel;

        joint.enabled = false; //Parts will only ever have a single FixedJoint at a time (equivalent to parent in hierarchy), so no need to check list

        //Set velocity
        rb.velocity = launchVel;
        rb.angularVelocity = highRotationSpeed;
    }
}
