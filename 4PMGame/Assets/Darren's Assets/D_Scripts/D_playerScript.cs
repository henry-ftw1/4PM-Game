using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// player script
public class D_playerScript : D_GameEntity
{
    public static D_playerScript current;

    public float moveSpeed = 10f;
    //[SerializeField]
    //private float Health = 15f;
    [SerializeField]
    private float iFrameTimeMax = 3f;
    private float iFrameTime;
    public bool invincible = false;
    [SerializeField]
    private float shootDelay = 0.5f;
    public GameObject bulletPrefab;

    private bool shooting = false;
    private Rigidbody2D rb2d;

    //[SerializeField]
    //private int ShootUnlock = 1; //2 add more bulets, 3 add rockets.

    [SerializeField]
    private D_BulletPooler pooler;
    private SpriteRenderer sRend;

    public CameraShake camShake;
    public float CameraShakeDuration;
    public float CameraShakeMagnitude;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip attachSound = null;
    [SerializeField]
    private AudioClip hitSound = null;
    [SerializeField]
    private AudioClip launchSound = null;

    void Awake()
    {
        if (current == null)
            current = this;
        else
        {
            Destroy(this.gameObject);
            return;
        }
        if (SceneManager.GetActiveScene().name == "TutorialScene")
            return;
        DontDestroyOnLoad(this.gameObject);
    }

    //private HealthBar hBarScript;
    // Start is called before the first frame update
    void Start()
    {
        AssignComponents();
        CameraShakeDuration = 0.2f;
        CameraShakeMagnitude = 0.3f;
    }

    void AssignComponents()
    {
        pooler = GameObject.Find("SpawnerObj").GetComponent<D_BulletPooler>();
        rb2d = GetComponent<Rigidbody2D>();
        hBarScript = GetComponent<HealthBar>();
        iFrameTime = iFrameTimeMax;
        sRend = this.gameObject.GetComponent<SpriteRenderer>();
        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pooler == null)
        {
            AssignComponents();
            hBarScript.AssignHealthBar();
            hBarScript.SetHealthBar(Health);
        }
        if (!shooting)
        {
            StartCoroutine(Shoot());
            shooting = true;
        }
        if (SpawnInvTime > 0)
        {
            SpawnInvTime--;
        }
        //if (Input.GetKey(KeyCode.E))
        //{
        //    Health = 100f;
        //}
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void UpdateHealthBar()
    {
        if(hBarScript == null)
            hBarScript.AssignHealthBar();
        hBarScript.SetHealthBar(Health);
    }

    protected override void Move()
    {
        Vector3 velocity = Vector3.zero;
        // move how finger moves, but for now use wasd
        if (Input.GetKey(KeyCode.W) && transform.position.y <= 6.9f)
        {
            velocity += Vector3.up;
            //transform.Translate(0, moveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x >= -3.9f)
        {
            velocity += transform.TransformDirection(Vector3.left);
            //transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.S) && transform.position.y >= -6.9f)
        {
            velocity += Vector3.down;
            //transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x <= 3.9f)
        {
            velocity += transform.TransformDirection(Vector3.right);
            //transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            DetachParts(true);
        }

        velocity = Vector3.Normalize(velocity);
        velocity *= moveSpeed;
        if (Input.touchCount <= 0) //Only move if no touch input.
            rb2d.MovePosition(transform.position + velocity * Time.deltaTime);
    }


    private void DetachParts(bool tapTriggered) {
        List<loosePart> parts = new List<loosePart>();

        foreach (Transform child in transform) {
            if (child.gameObject.name == "HealParticle")
                continue;
            parts.Add(child.GetComponent<loosePart>());
        }
        if (parts.Count > 0)
            audioManager.instance.playClip(launchSound);
        for (int i = 0; i < parts.Count; ++i) {
            if(tapTriggered)
                parts[i].flyAway(true);
            else
                parts[i].flyAway(false);
        }
    }

    public void PlayAttach()
    {
        audioManager.instance.playClip(attachSound);
    }

    /*
    public override void TakeHit(float dmg)
    {
        Health -= dmg;
        hBarScript.SetHealthBar(Health);
        if (Health <= 0)
        {
            Destroy();
        }
    }
    */
    /***
    public void increaseHealth(float h)
    {
        if(Health + h <= hBarScript.startHealth)
        {
            Health += h;
            hBarScript.SetHealthBar(Health);
        }
    }

    public void changeUnlock(int num)
    {
        ShootUnlock = num;
        shootDelay = (float)num / (float)10;
    }***/

    public void SetShoot(bool shoot)
    {
        shooting = shoot;
    }

    protected override void Destroy()
    {
        // call game over, put explosion effect later on?
        GameObject.Find("SceneManager").GetComponent<D_SceneManager>().GameOver();
    }
    protected override IEnumerator Shoot()
    {
        //spawn a bullet
        while (this.isActiveAndEnabled)
        {
            GameObject bullet = pooler.GetPooledObject(0);
            if (bullet == null)
            {
                bullet = pooler.PoolMoreBullets(0);
            }
            bullet.transform.position = transform.position;
            bullet.transform.localRotation = transform.rotation;
            bullet.SetActive(true);
            //Instantiate(bulletPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(shootDelay);
        }
    }

    IEnumerator IFrameFlash()
    {
        invincible = true;
        bool toggleSprite = true;
        while (iFrameTime > 0)
        {
            Color temp = sRend.color;
            if (toggleSprite)
            {
                temp.a = 0.5f;
                sRend.color = temp;
                toggleSprite = false;
            }
            else
            {
                temp.a = 1f;
                sRend.color = temp;
                toggleSprite = true;
            }
            iFrameTime--;
            yield return new WaitForSeconds(0.1f);
        }
        iFrameTime = iFrameTimeMax;
        sRend.color = new Color(1f, 1f, 1f, 1f);
        invincible = false;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "cat ball yarn" || collision.gameObject.name == "cat ball yarn(Clone)")
        {
            audioManager.instance.playClip(hitSound);
            StartCoroutine(IFrameFlash());
            TakeHit(2);
            DetachParts(false);
            StartCoroutine(camShake.Shake(CameraShakeDuration, CameraShakeMagnitude));
        }
        else if (collision.gameObject.tag == "EnemyBullet")
        {
            if (!invincible)
            {
                audioManager.instance.playClip(hitSound);
                StartCoroutine(IFrameFlash());
                TakeHit(1);
                Destroy(collision.gameObject);
                StartCoroutine(camShake.Shake(CameraShakeDuration, CameraShakeMagnitude));
            }
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit enemey");
            if (!invincible)
            {
                audioManager.instance.playClip(hitSound);
                StartCoroutine(IFrameFlash());
                TakeHit(1);
                StartCoroutine(camShake.Shake(CameraShakeDuration, CameraShakeMagnitude));
            }
        }
    }
}
