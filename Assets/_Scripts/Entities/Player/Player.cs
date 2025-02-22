using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Player Components")]
    public static Player instance;
    public Entity entity;
    public PlayerWallCheck playerWallCheck;
    InputAction moveAction;

    [Header("Movement")]
    public float max_move_time = 3;
    public float move_time = 0;
    private Vector2 lastSafeWallPos = Vector2.zero;

    [Header("Components")]
    public Rigidbody2D rb;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Two Players Detected, deleting second");
            DestroyImmediate(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lastSafeWallPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        // I recommend only using AddForce ever in FixedUpdate 
        // unless the force is in impulse mode for an instantaneous application  
        // or if you are multiplying by Time.deltaTime to prevent frame rate dependant movement speed

        // Gets movement from user. 
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if (moveAction.IsPressed() && playerWallCheck.can_move == true)
        {
            // Move in the direction of their movement.
            rb.AddForce(moveValue);

            // Only increase time when not near a wall.
            if (move_time < max_move_time && playerWallCheck.walls_count == 0)
            {
                move_time += Time.deltaTime;
            }
            // Stop player from moving when away from a wall for x amount of time.
            else if (move_time >= max_move_time)
            {
                // Debug.Log("can_move = false");
                playerWallCheck.can_move = false;
                move_time = 0;
            }
        }
    }
    public void ReloadSafe()
    {
        transform.position = lastSafeWallPos;
        rb.linearVelocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("World"))
        {
            lastSafeWallPos = transform.position;
        }
    }
}
