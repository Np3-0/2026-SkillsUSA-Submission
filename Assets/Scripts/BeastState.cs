using UnityEngine;

public class BeastState : MonoBehaviour
{
    public static BeastState Instance { get; set; }
    public float curHealth, maxHealth;
    public bool defeated;
    public float damageMult = 0.1f;

    private bool IsDefeated => defeated || curHealth <= 0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        curHealth = maxHealth;
    }

    void Update()
    {
        if (IsDefeated)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetHealth(float val)
    {
        curHealth = Mathf.Clamp(val, 0, maxHealth);
    }
}