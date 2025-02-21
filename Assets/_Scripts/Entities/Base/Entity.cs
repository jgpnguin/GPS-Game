using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity")]
    public int id = 0;

    public EntityHealth entityHealth;
    public EntityMovement entityMovement;

    [Header("Components")]
    public Rigidbody2D rb;
    public Collider2D hitBox;
    public Animator animator;
    // [Header("Status")]
    [Header("Die")]
    public GameObject dieDestroyObj;
    public float dieDestroyTime = 3f;
    public bool disableHitboxWhileDying = true;
    public GameObject[] enableOnDie;





    // Start is called before the first frame update 
    protected virtual void Start()
    {
        entityHealth.OnDie += Die;
    }

    protected virtual void OnDestroy()
    {
        entityHealth.OnDie -= Die;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }


    protected virtual void Die()
    {
        if (disableHitboxWhileDying)
        {
            hitBox.enabled = false;
        }
        entityMovement.canMove = false;
        foreach (GameObject go in enableOnDie)
        {
            go.SetActive(true);
        }
        StartCoroutine(DieDestroyTimer());
    }

    private IEnumerator DieDestroyTimer()
    {
        yield return new WaitForSeconds(dieDestroyTime);
        Destroy(dieDestroyObj);
    }

    public virtual void ApplyKnockback(Vector2 kb)
    {
        rb.AddForce(kb, ForceMode2D.Impulse);
    }

}
