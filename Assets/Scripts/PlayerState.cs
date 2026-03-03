using UnityEngine;

public class PlayerState : MonoBehaviour {
    public static PlayerState Instance {get; set;}
    private Animator anim;
    public Sprite img;
    private string currentAnimationState;
    public float curHealth, maxHealth;
    public bool canMove = true;
    public bool isMoving;
    public char dir = 'd';
    
    void Awake() {
        if (Instance != null && Instance != this){
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start() {
        anim = GetComponent<Animator>();
        curHealth = maxHealth;
    }

    public void Update()
    {
        if (curHealth <= 0f)
        {
            Debug.Log("dead!");
        }

        UpdateAnimDir();
    }

    public void SetHealth(float val){
        curHealth = Mathf.Clamp(val, 0, maxHealth);
    }

    private void UpdateAnimDir()
    {
        if (anim == null)
        {
            return;
        }

        string direction = dir switch
        {
            'w' => "Up",
            's' => "Down",
            'a' => "Left",
            'd' => "Right",
            _ => "Down"
        };

        string targetState = isMoving ? direction : $"Idle_{direction}";

        if (currentAnimationState == targetState)
        {
            return;
        }

        anim.Play(targetState);
        currentAnimationState = targetState;
    }
}