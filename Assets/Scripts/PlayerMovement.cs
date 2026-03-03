using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float IdleThreshold = 0.0001f;

    public enum DominantAxis
    {
        None,
        X,
        Y
    }

    public enum Direction4
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public float moveSpeed = 7.5f;
    private Rigidbody2D rb;
    private Vector2 rawMovement;
    private Vector2 movement;
    public Transform cameraPos;
    public DominantAxis dominantAxis;
    public Direction4 direction4;
    public Direction4 lastDirection4 = Direction4.Down;
    public int xDirection;
    public int yDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetMovement();
        UpdateDir();
    }

    void FixedUpdate()
    {
        if (PlayerState.Instance.canMove)
        {
            MovePlayer();
            FollowCamera();
            return;
        }

        rb.linearVelocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        SoundManager.Instance.PlaySound(SoundManager.Instance.encounterSound);
        FightLogic.StartFight();
    }

    private void GetMovement()
    {
        rawMovement.x = Input.GetAxisRaw("Horizontal");
        rawMovement.y = Input.GetAxisRaw("Vertical");

        movement = rawMovement.normalized;
        xDirection = (int)Mathf.Sign(rawMovement.x);
        yDirection = (int)Mathf.Sign(rawMovement.y);
    }

    private void UpdateDir()
    {
        float absX = Mathf.Abs(rawMovement.x);
        float absY = Mathf.Abs(rawMovement.y);

        if (rawMovement.sqrMagnitude < IdleThreshold)
        {
            dominantAxis = DominantAxis.None;
            direction4 = Direction4.None;
        }
        else if (absX > absY)
        {
            dominantAxis = DominantAxis.X;
            
            direction4 = rawMovement.x > 0f ? Direction4.Right : Direction4.Left;
        }
        else
        {
            dominantAxis = DominantAxis.Y;
            direction4 = rawMovement.y > 0f ? Direction4.Up : Direction4.Down;
        }

        if (direction4 != Direction4.None)
        {
            lastDirection4 = direction4;
        }

        PlayerState.Instance.dir = direction4 switch
        {
            Direction4.Up => 'w',
            Direction4.Down => 's',
            Direction4.Left => 'a',
            Direction4.Right => 'd',
            _ => PlayerState.Instance.dir
        };

        PlayerState.Instance.isMoving = PlayerState.Instance.canMove && direction4 != Direction4.None;
    }

    private void MovePlayer()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    private void FollowCamera()
    {
        if (cameraPos == null)
        {
            return;
        }

        cameraPos.position = new Vector3(transform.position.x, transform.position.y, cameraPos.position.z);
    }
}
