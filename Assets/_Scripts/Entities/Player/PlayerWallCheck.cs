using System.Collections;
using UnityEngine;

public class PlayerWallCheck : MonoBehaviour
{
    public int walls_count = 0;
    public bool can_move = true;
    public Collider2D wall_detector;


    // Detects and adds to counter how many walls are near.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Allow player to move when near a wall.
        if (collision.gameObject.layer == 7)
        {
            // Debug.Log("can_move = true");
            can_move = true;
            walls_count++;
            // lastSafeWallPos = transform.position; 
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
