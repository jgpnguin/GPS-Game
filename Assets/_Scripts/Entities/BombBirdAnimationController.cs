using UnityEngine;

public class BombBirdAnimationController : MonoBehaviour
{
    public Animator animator;
    public PatrolEnemyAI patrolEnemyAI;
    public EntityHealth turretHealth;

    void Start()
    {
        turretHealth.OnDie += Die;
    }

    void Oestroy()
    {
        turretHealth.OnDie -= Die;
    }

    public void Die()
    {
        // dead = true;
        animator.SetBool("Dead", true);
    }



    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Flying", patrolEnemyAI.state == PatrolEnemyAI.State.Pursuit || patrolEnemyAI.entity.rb.linearVelocity.magnitude > PatrolEnemyAI.SPEED_TO_ROT);
    }
}
