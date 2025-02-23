using System.Collections;
using UnityEngine;

public class BombBird : MonoBehaviour
{
    public GameObject explosion;
    public EntityHealth entityHealth;
    public static float explodeDist = 1f;
    public static float explosionDelay = 1f;
    public bool explosionNotQueued = true;


    void FixedUpdate()
    {
        if (explodeDist >= Vector2.Distance(Player.instance.transform.position, transform.position) && explosionNotQueued && !entityHealth.dead)
        {
            StartCoroutine(QueueExplosion());
        }
    }

    IEnumerator QueueExplosion()
    {
        explosionNotQueued = false;
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    private void Explode()
    {
        if (!entityHealth.dead)
        {
            explosion.SetActive(true);
            entityHealth.Die();
        }
    }
}
