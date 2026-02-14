using System.Collections;
using UnityEngine;


public class RegularEnemyState : MonoBehaviour {
    public static RegularEnemyState Instance {get; set;}
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

    public void SetHealth(float val){
        curHealth = Mathf.Clamp(val, 0, maxHealth);
    }
}