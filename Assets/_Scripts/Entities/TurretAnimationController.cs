using UnityEngine;

public class TurretAnimationController : MonoBehaviour
{
    public Animator animator;
    public TurretEnemyAI turretEnemyAI;
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
        animator.SetBool("Firing", turretEnemyAI.GetState() == TurretEnemyAI.State.Shooting);
    }
}
