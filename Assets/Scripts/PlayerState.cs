using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour {
    public static PlayerState Instance {get; set;}
    public float curHealth, maxHealth;
    public bool canMove = true;
    
    void Awake() {
        if (Instance != null && Instance != this){
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start() {
        curHealth = maxHealth;
    }

    public void Update()
    {
        if (curHealth <= 0f)
        {
            Debug.Log("dead!");
        }
    }

    public void SetHealth(float val){
        curHealth = Mathf.Clamp(val, 0, maxHealth);
    }
}