using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchScript : MonoBehaviour
{
    public float doubleTapTime = 0.25f;
    float tapTimer = 0f;
    bool hasTapped = false;

    Camera cam;
    Rigidbody2D rb2d;
    Vector3 oldPosition = Vector3.zero;

    //Hardcoding starting Y value to avoid teleporting
    Vector3 moveTo = new Vector3(0f, -4f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void OnEnable() {
        cam = Camera.main;
        moveTo = new Vector3(0f, -4f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
            
        tapTimer -= Time.deltaTime;
        if (tapTimer < 0 || Time.timeScale == 0)
            hasTapped = false;

        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                oldPosition = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));

                if (hasTapped && tapTimer > 0) {
                    hasTapped = false;
                    DetachParts();
                }

                hasTapped = true;
                tapTimer = doubleTapTime;
            }
            if (touch.phase == TouchPhase.Moved) {
                Vector3 newPosition = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
                Vector3 velocity = (newPosition - oldPosition) * OptionsData.current.touchSensitivity;
                oldPosition = newPosition;
                moveTo = transform.position + velocity;
                if (moveTo.y > 6.9f)
                    moveTo.y = 6.9f;
                else if (moveTo.y < -6.9f)
                    moveTo.y = -6.9f;
                if (moveTo.x > 3.9f)
                    moveTo.x = 3.9f;
                else if (moveTo.x < -3.9f)
                    moveTo.x = -3.9f;
            }
        }
    }

    void FixedUpdate() {
        if (Input.touchCount > 0) {
            rb2d.MovePosition(moveTo);
        }
    }

    private void DetachParts() {
        List<loosePart> parts = new List<loosePart>();

        foreach (Transform child in this.transform) {
            if (child.gameObject.name == "HealParticle")
                continue;
            parts.Add(child.GetComponent<loosePart>());
            //child.GetComponent<loosePart>().flyAway(true);
        }
        for (int i = 0; i < parts.Count; ++i) {
            parts[i].flyAway(true);
        }
    }
}
