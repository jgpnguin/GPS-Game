using UnityEngine;

public class Spike : MonoBehaviour
{
    public const float damage = 20f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Entity hitBoi = collision.GetComponent<Entity>();
        if (hitBoi != null)
        {
            hitBoi.entityHealth.ChangeHealth(-damage, 1f, true);
            if (hitBoi.gameObject == Player.instance.gameObject && !hitBoi.entityHealth.dead)
            {
                ScreenBlackerController.instance.SetScreenHideAndUnhideResetChar();
            }

        }
    }
}
