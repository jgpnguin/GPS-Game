using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player instance;
    InputAction moveAction;

    [Header("Movement")]
    public bool can_move = true;
    public int walls_count = 0;
    public float max_move_time = 3;
    public float move_time = 0;

    [Header("Components")]
    public Rigidbody2D rb;
    public CircleCollider2D wall_detector;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Gets movement from user.
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if (moveAction.IsPressed() && can_move == true)
        {
            // Move in the direction of their movement.
            rb.AddForce(moveValue);
            
            // Only increase time when not near a wall.
            if (move_time < max_move_time && walls_count == 0)
            {
                move_time += Time.deltaTime;
            }
            // Stop player from moving when away from a wall for x amount of time.
            else if (move_time >= max_move_time)
            {
                // Debug.Log("can_move = false");
                can_move = false;
                move_time = 0;
            }
        }
    }

    // Detects and adds to counter how many walls are near.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Allow player to move when near a wall.
        if (collision.gameObject.layer == 7)
        {
            // Debug.Log("can_move = true");
            can_move = true;
            walls_count++;
        }
    }

    // Detects and removes from counter how many walls are near.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            walls_count--;
        }
    }
}
