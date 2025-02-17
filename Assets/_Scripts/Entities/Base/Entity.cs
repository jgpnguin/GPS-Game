using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity")]
    public int id { get; protected set; } = 0;

    public EntityHealth entityHealth;
    public EntityMovement entityMovement;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    // [Header("Status")]




    // Start is called before the first frame update 
    protected virtual void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }


    protected virtual void FixedUpdate()
    {
    }

    public virtual void ApplyKnockback(Vector2 kb)
    {
        rb.AddForce(kb, ForceMode2D.Impulse);
    }

}
