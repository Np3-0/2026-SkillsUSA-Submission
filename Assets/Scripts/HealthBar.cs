using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {   

    private RawImage fillBar;
    public TextMeshProUGUI healthCnt;
    public GameObject playerState;

    private float curHealth, maxHealth;

    void Awake() {
        fillBar = GameObject.Find("FillBar").GetComponent<RawImage>();
        playerState = GameObject.Find("Player").GetComponent<PlayerState>().gameObject;
    }

    // Update is called once per frame
    void Update() {
        curHealth = playerState.GetComponent<PlayerState>().curHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float width = (curHealth / 100) * 475;
        fillBar.rectTransform.sizeDelta = new Vector2(width, fillBar.rectTransform.sizeDelta.y);

        healthCnt.text = curHealth + " / " + maxHealth; 
    }
}