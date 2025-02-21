using System.Collections;
using UnityEngine;

public class BombBird : MonoBehaviour
{
    public GameObject explosion;
    public EntityHealth entityHealth;
    public static float explodeDist = 1f;
    public static float explosionDelay = 1f;
    public Coroutine explosionQueued = null;


    void FixedUpdate()
    {
        if (explodeDist >= Vector2.Distance(Player.instance.transform.position, transform.position) && explosionQueued == null)
        {
            explosionQueued = StartCoroutine(QueueExplosion());
        }
    }

    IEnumerator QueueExplosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    private void Explode()
    {
        explosion.SetActive(true);
        entityHealth.Die();
        Destroy(this);
    }
}
