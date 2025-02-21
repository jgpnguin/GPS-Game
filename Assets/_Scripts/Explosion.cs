using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float effectRadius = 1f;
    public float explosionStrength = 5f;
    public int ownerID = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created 
    void OnEnable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, effectRadius, LayerMask.GetMask("Entity"));
        foreach (Collider2D hit in hits)
        {
            Entity hitEntity = hit.GetComponent<Entity>();
            Debug.Log($"Explosion hit: {hit.name}");
            if (hitEntity != null && hitEntity.id != ownerID)
            {
                Vector2 relPos = hitEntity.transform.position - transform.position;
                hitEntity.ApplyKnockback(relPos.normalized * explosionStrength);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
