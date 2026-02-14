using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {   

    public RawImage fillBar;
    public TextMeshProUGUI healthCnt;
    public GameObject state;
    public string stateName;

    private float curHealth, maxHealth;

    void Awake() {
        if (stateName == "Player") {
            state = GameObject.FindWithTag("Player");
        } else if (stateName == "Enemy") {
            state = GameObject.FindWithTag("Enemy");
        }
    }

    // Update is called once per frame
    void Update() {
        if (stateName == "Player") {
            curHealth = state.GetComponent<PlayerState>().curHealth;
            maxHealth = state.GetComponent<PlayerState>().maxHealth;
        } else if (stateName == "Enemy") {
            curHealth = state.GetComponent<RegularEnemyState>().curHealth;
            maxHealth = state.GetComponent<RegularEnemyState>().maxHealth;
        }

        float width = curHealth / maxHealth * 475;
        fillBar.rectTransform.sizeDelta = new Vector2(width, fillBar.rectTransform.sizeDelta.y);

        healthCnt.text = curHealth + " / " + maxHealth; 
    }
}