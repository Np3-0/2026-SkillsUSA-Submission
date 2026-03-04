using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {   

    public RawImage fillBar;
    public TextMeshProUGUI healthCnt;
    public GameObject state;
    public string stateName;

    private float curHealth, maxHealth;

    private const float BarWidth = 475f;

    void Awake() {
        ResolveStateObject();
    }

    // Update is called once per frame
    void Update() {
        if (state == null)
        {
            ResolveStateObject();
            if (state == null)
            {
                return;
            }
        }

        if (!TryReadHealth(out curHealth, out maxHealth))
        {
            return;
        }

        if (fillBar != null && maxHealth > 0f)
        {
            float width = curHealth / maxHealth * BarWidth;
            fillBar.rectTransform.sizeDelta = new Vector2(width, fillBar.rectTransform.sizeDelta.y);
        }

        if (healthCnt != null)
        {
            healthCnt.text = curHealth + " / " + maxHealth;
        }
    }

    private void ResolveStateObject()
    {
        if (stateName == "Player")
        {
            state = GameObject.FindWithTag("Player");
        }
        else if (stateName == "Enemy")
        {
            state = GameObject.FindWithTag("Enemy");
        }
        else if (stateName == "Beast")
        {
            state = GameObject.FindWithTag("Beast");
        }
    }

    private bool TryReadHealth(out float currentHealth, out float maximumHealth)
    {
        currentHealth = 0f;
        maximumHealth = 0f;

        if (stateName == "Player")
        {
            PlayerState playerState = state.GetComponent<PlayerState>();
            if (playerState == null)
            {
                return false;
            }

            currentHealth = playerState.curHealth;
            maximumHealth = playerState.maxHealth;
            return true;
        }

        if (stateName == "Enemy")
        {
            if (RegularEnemyState.Instance != null && RegularEnemyState.Instance.defeated)
            {
                return false;
            }

            RegularEnemyState enemyState = state.GetComponent<RegularEnemyState>();
            if (enemyState == null)
            {
                return false;
            }

            currentHealth = enemyState.curHealth;
            maximumHealth = enemyState.maxHealth;
            return true;
        }

        if (stateName == "Beast")
        {
            if (BeastState.Instance != null && BeastState.Instance.defeated)
            {
                return false;
            }

            BeastState beastState = state.GetComponent<BeastState>();
            if (beastState == null)
            {
                return false;
            }

            currentHealth = beastState.curHealth;
            maximumHealth = beastState.maxHealth;
            return true;
        }

        return false;
    }
}