using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage; // Damage value passed from GunSO
    private float speed; // Speed value passed from GunSO
    private float lifetime; // Lifetime value passed from GunSO
    private int ownerID;
    private Rigidbody2D rb;

    public void Initialize(int damageValue, float speedValue, float lifetimeValue, int ownerID)
    {
        damage = damageValue;
        speed = speedValue;
        lifetime = lifetimeValue;
        this.ownerID = ownerID;
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed; // Set bullet velocity
        }

        Destroy(gameObject, lifetime); // Destroy bullet after lifetime
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); // Destroy the bullet
        }
        Entity entity = collision.GetComponent<Entity>();
        if (entity != null && entity.id != ownerID) // Ignore if the bullet hits its owner
        {
            // Decrease the entity's health by the bullet's damage
            entity.entityHealth.ChangeHealth(-damage); // Use ChangeHealth to apply damage
            Destroy(gameObject); // Destroy the bullet after hitting the target
        }
    }
}
