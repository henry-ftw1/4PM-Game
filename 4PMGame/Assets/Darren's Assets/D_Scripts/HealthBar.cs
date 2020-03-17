using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public float startHealth;

    // Start is called before the first frame update
    void Start()
    {
        AssignHealthBar();
    }

    public void SetHealthBar(float hp)
    {
        if (healthBar == null) return;
        healthBar.fillAmount = hp / startHealth;
    }

    public void AssignHealthBar()
    {
        if (this.gameObject.tag == "Player")
        {
            Transform parentHP = GameObject.Find("PlayerUI").transform;
            healthBar = parentHP.Find("HealthBG").Find("HealthBar").GetComponent<Image>();
            startHealth = this.gameObject.GetComponent<D_playerScript>().getStartHealth();
        }
        else if (this.gameObject.tag == "Enemy")
        {
            try
            {
                Transform canvas = transform.GetChild(0);
                healthBar = canvas.transform.GetChild(0).GetComponent<Image>();
            }
            catch
            {
                Debug.Log("CANVAS MUST BE FIRST CHILD, HP IMAGE MUST BE FIRST CHILD OF CANVAS");
            }
            startHealth = this.gameObject.GetComponent<D_GameEntity>().getStartHealth();
        }
        if (healthBar == null)
        {
            Debug.Log("PLEASE ASSIGN HEALTH BAR");
            return;
        }
        healthBar.fillAmount = startHealth;
    }
}
