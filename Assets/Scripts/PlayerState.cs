using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour {
    public static PlayerState Instance {get; set;}
    public float curHealth, maxHealth;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            curHealth -= 10f;
            Debug.Log("Health: " + curHealth);
        }
        if (curHealth <= 0f)
        {
            Debug.Log("dead!");
        }
    }

    public void SetHealth(float val){
        curHealth = val;
    }
}