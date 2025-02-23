using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [Header("Player Components")]
    public static Player instance;
    public Entity entity;
    public EntityHealth entityHealth;
    public PlayerWallCheck playerWallCheck;
    InputAction moveAction;

    [Header("Movement")]
    public float max_speed = 30f;
    public float acceleration = 15f;
    public float max_air_time = 1f;
    public float air_time = 0f;
    private Vector2 lastSafeWallPos = Vector2.zero;
    public float movementDampening = 0f;
    public float floatingDampening = 1f;

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
    void FixedUpdate()
    {
        // No movement if dead.
        if (entityHealth.dead == true)
        {
            return;
        }
        // Gets movement from user.
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if (playerWallCheck.can_move == true)
        {
            air_time += Time.deltaTime;

            Debug.Log(moveValue);

            // Stops the player from moving if they ran out of air time.
            if (air_time >= max_air_time)
            {
                playerWallCheck.can_move = false;
                rb.linearDamping = movementDampening;
            }
            else if (moveAction.IsPressed())
            {
                // Prevent player from moving faster than max_speed.
                Vector2 velocity = rb.linearVelocity;
                float magnitude = velocity.magnitude;
                
                if (magnitude <= max_speed)
                {
                    rb.linearDamping = movementDampening;
                    rb.AddForce(moveValue * acceleration);
                }
            }
            else
            {
                rb.linearDamping = floatingDampening;
            }
        }

        // Reset air time when near a wall.
        if (playerWallCheck.walls_count > 0)
        {
            air_time = 0;
            playerWallCheck.can_move = true;
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
