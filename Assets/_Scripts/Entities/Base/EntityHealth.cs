using UnityEngine;
using System.Collections;
using System;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] protected bool changingHealth = true;
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float healthChangeRate = 0f;
    public event Action OnDie;
    public bool dead = false;
    public Coroutine iFrames { get; protected set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    protected virtual void Update()
    {
        if (changingHealth)
        {
            ChangeHealth(healthChangeRate * Time.deltaTime, 0f);
        }
    }


    public virtual void ChangeHealth(float delta, float iFramesAddTime = 0.2f, bool ignoresIframes = false)
    {
        if (changingHealth && !dead)
        {
            if (delta > 0 || iFrames == null || ignoresIframes)
            {
                health += delta;
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                else if (health < 0)
                {
                    Die();
                }
                if (iFramesAddTime != 0f && iFrames == null)
                {
                    iFrames = StartCoroutine(IFrameSet(iFramesAddTime));
                }
            }
        }
    }


    protected IEnumerator IFrameSet(float iFrameAddTime)
    {
        yield return new WaitForSeconds(iFrameAddTime);
        iFrames = null;
    }

    public virtual void Die()
    {
        dead = true;
        OnDie?.Invoke();
    }
}
