using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 7.5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
    }

    void FixedUpdate()
    {
        if (GlobalState.canMove)
        {
            rb.linearVelocity = movement * moveSpeed;
        } 
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.encounterSound);
            FightLogic.StartFight();
        }
        
    }
}
