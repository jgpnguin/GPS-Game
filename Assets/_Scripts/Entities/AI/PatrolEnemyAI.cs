using UnityEngine;

public class PatrolEnemyAI : MonoBehaviour
{ // remind me to make an enemy that is just an alerter

    public enum State
    {
        Pursuit,
        Patrol
    }

    public Entity entity;
    public bool rotateToMovment = false;
    public State state = State.Patrol;
    public Transform viewPoint;

    [Header("Pursuit")]
    public float pursLostSightTime = 2f;
    public float pursRange = 5f;
    public float pursSpeed = 5f;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    [SerializeField] private int patrolPointsInd = 0;
    public bool patrolRollover = false;
    public bool patrolForward = false;
    public float patrolSpeed = 3f;

    [Header("Targetting")]
    public float targetRange = 4f;
    // public bool targetOnlyForward = true;
    public Vector2 targetAngle = new Vector2(90, -90);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool hasLOS;
        Vector2 LOSVec;
        if (state == State.Patrol)
        {
            (hasLOS, LOSVec) = HasPlayerLOS(targetRange);
            // Debug.Log(hasLOS);
            if (hasLOS)
            {
                state = State.Pursuit;
            }
            else
            {
                entity.entityMovement.Move(patrolPoints[patrolPointsInd].position - transform.position);
                // check if pass through patrol point
                if (Vector2.Distance(patrolPoints[patrolPointsInd].position, transform.position) < .5f)
                {
                    if (patrolForward)
                    {
                        patrolPointsInd++;
                        if (patrolPointsInd >= patrolPoints.Length)
                        {
                            if (patrolRollover)
                            {
                                patrolPointsInd = 0;
                            }
                            else
                            {
                                patrolForward = false;
                                patrolPointsInd -= 2;
                            }
                        }
                    }
                    else
                    {
                        if (patrolPointsInd <= 0)
                        {
                            if (patrolRollover)
                            {
                                patrolPointsInd = patrolPoints.Length - 1;
                            }
                            else
                            {
                                patrolForward = true;
                                patrolPointsInd++;
                            }
                        }
                        else
                        {
                            patrolPointsInd--;
                        }
                    }
                }
            }
        }
        else if (state == State.Pursuit)
        {
            (hasLOS, LOSVec) = HasPlayerLOS(pursRange);
            if (hasLOS)
            {
                entity.entityMovement.Move(LOSVec);
            }
            else
            {
                entity.entityMovement.Move(Player.instance.transform.position - viewPoint.position);
            }


        }
    }

    private (bool, Vector2) HasPlayerLOS(float range)
    {
        Vector2 dir = Player.instance.transform.position - viewPoint.position;
        RaycastHit2D hit = Physics2D.Raycast(viewPoint.position, dir, range, LayerMask.GetMask("Entity", "World"));
        if (hit && hit.collider.gameObject == Player.instance.gameObject)
        {
            float angle = Mathf.Atan2(hit.point.y - viewPoint.position.y, hit.point.x - viewPoint.position.x) * Mathf.Rad2Deg - viewPoint.eulerAngles.z;
            if (angle < -180)
            {
                angle += 360;
            }
            else if (angle > 180)
            {
                angle -= 360;
            }
            // Debug.Log(angle);
            return (angle <= targetAngle.x && angle >= targetAngle.y, hit.point);
        }
        return (false, Vector2.zero);
    }


}
