
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public bool upIsForward = false;
    public State state = State.Patrol;
    public Transform viewPoint;

    public Vector2 targetPos;


    [Header("Patrol")]
    public Transform[] patrolPoints;
    [SerializeField] private int patrolPointsInd = 0;
    public bool patrolRollover = false;
    private bool patrolForward = false;
    public float patrolSpeed = 3f;

    [Header("Targetting")]
    public float targetRange = 6f;
    // public bool targetOnlyForward = true;
    public Vector2 targetAngle = new Vector2(90, -90);


    [Header("Pursuit")]
    public float pursLostSightTime = 2f;
    public Coroutine pursLostSight;
    public float pursRange = 10f;
    public float pursSpeed = 5f;
    public float pursStopDist = 3f;

    [Header("A* Things")]
    public List<Vector3Int> aStarPath;
    public float aStarRegenMax = .5f;
    public float aStarRegenCur = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aStarRegenCur = Random.Range(0, aStarRegenMax);
    }

    // Update is called once per frame]
    void Update()
    {
        aStarRegenCur -= Time.deltaTime;
        if (rotateToMovment)
        {
            if (entity.rb.linearVelocity.magnitude > 4 || state == State.Pursuit)
            {
                if (upIsForward)
                {
                    // transform.up = entity.rb.linearVelocity;
                    transform.up = (Vector3)targetPos - transform.position;
                }
                else
                {
                    transform.right = (Vector3)targetPos - transform.position;
                    // transform.right = entity.rb.linearVelocity;
                }

            }
            else if (state == State.Patrol)
            {
                // entity.rb.SetRotation(0);
                transform.rotation = Quaternion.identity;
            }
        }
    }

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
                targetPos = LOSVec;
                state = State.Pursuit;
            }
            else if (patrolPoints.Length > 0)
            {
                // check if can view next patrol point, if hit obsticle do a* instead
                Vector2 patrolPointDist = patrolPoints[patrolPointsInd].position - viewPoint.position;
                if (Physics2D.Raycast(viewPoint.position, patrolPointDist, patrolPointDist.magnitude, LayerMask.GetMask("World")))
                {
                    if (aStarPath.Count > 0)
                    {
                        if (Vector2.Distance(targetPos, transform.position) <= 0.5f)
                        {
                            targetPos = CellToWorld(aStarPath[^1]);
                            aStarPath.RemoveAt(aStarPath.Count - 1);
                        }
                    }
                    else if (aStarRegenCur < 0)
                    {
                        aStarRegenCur = aStarRegenMax;
                        GenerateAStar(patrolPoints[patrolPointsInd].position);
                        if (aStarPath.Count > 0)
                        {
                            targetPos = CellToWorld(aStarPath[^1]);
                            aStarPath.RemoveAt(aStarPath.Count - 1);
                        }
                    }
                    MoveTowards(targetPos);
                }
                else
                {
                    targetPos = patrolPoints[patrolPointsInd].position;
                    // check if pass through patrol point
                    if (Vector2.Distance(targetPos, transform.position) < .5f)
                    {
                        if (patrolPoints.Length > 1)
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
                        else
                        {
                            entity.entityMovement.SlowDown();
                            // MoveTowards(patrolPoints[patrolPointsInd].position); 
                            // transform.position = patrolPoints[patrolPointsInd].position; 
                        }
                    }
                    else
                    {
                        MoveTowards(targetPos);
                    }
                }
            }
        }
        else if (state == State.Pursuit)
        {
            (hasLOS, LOSVec) = HasPlayerLOS(pursRange);
            if (hasLOS)
            {
                targetPos = LOSVec;
                if (pursLostSight != null)
                {
                    StopCoroutine(pursLostSight);
                    pursLostSight = null;
                }
                if (pursStopDist <= Vector2.Distance(targetPos, transform.position))
                {
                    MoveTowards(targetPos);
                }
                else
                {
                    entity.entityMovement.SlowDown();
                }
            }
            else
            {
                if (pursLostSight == null)
                {
                    pursLostSight = StartCoroutine(PursLostSightTimeTimer());
                }
                if (aStarRegenCur < 0)
                {
                    aStarRegenCur = aStarRegenMax;
                    GenerateAStar(Player.instance.transform.position);
                    if (aStarPath.Count > 0)
                    {
                        targetPos = CellToWorld(aStarPath[^1]);
                        aStarPath.RemoveAt(aStarPath.Count - 1);
                    }
                }
                if (aStarPath.Count > 0)
                {
                    if (Vector2.Distance(targetPos, transform.position) <= 0.5f)
                    {
                        targetPos = CellToWorld(aStarPath[^1]);
                        aStarPath.RemoveAt(aStarPath.Count - 1);
                    }
                }
                MoveTowards(targetPos);
            }
            // // entity.entityMovement.Move(targetPos - (Vector2)viewPoint.position); 
            // aStarRegenCur -= Time.deltaTime; 

        }
    }

    private IEnumerator PursLostSightTimeTimer()
    {
        yield return new WaitForSeconds(pursLostSightTime);
        aStarPath.Clear();
        state = State.Patrol;
    }

    private Vector2 CellToWorld(Vector3Int pos)
    {
        return TilemapQuery.instance.CellToWorld(pos) + new Vector2(0.5f, 0.5f);
    }

    private void MoveTowards(Vector2 targ)
    {
        entity.entityMovement.Move(targ - (Vector2)transform.position);
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

    private void GenerateAStar(Vector3 target, int maxCartesianDistance = 400)
    {
        // PathfindingNode.instancesCreated = 0; 
        aStarPath.Clear();
        SortedSet<PathfindingNode> toCheck = new SortedSet<PathfindingNode>(new PathfindingNode.NodeComparer()); // it's not a sorted multiset, this is where all your issues come in lmao
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        // bool = true if visited
        // Dictionary<Vector3Int, (PathfindingNode, bool)> costs = new Dictionary<Vector3Int, (PathfindingNode, bool)>();
        TilemapQuery tmq = TilemapQuery.instance;

        Vector3Int pos = tmq.WorldToCell(transform.position);
        // print(pos);
        Vector3Int targ = tmq.WorldToCell(target);
        // print(targ);

        int hCost = tmq.GetTileCostRem(targ, pos);
        int fCost = hCost;
        int gCost = 0;
        PathfindingNode cur = new PathfindingNode(fCost, gCost, hCost, pos, null);
        toCheck.Add(cur);
        // costs.Add(cur.pos, (cur, false));

        while (toCheck.Count > 0)
        {
            // print(toCheck.Count);
            cur = toCheck.First();
            if (cur.pos == targ)
            {
                while (cur != null)
                {
                    aStarPath.Add(cur.pos);
                    cur = cur.prev;
                }
                return;
            }
            // RonanTestDriver.instance.SpawnTest(CellToWorld(cur.pos));
            // print(cur.fCost);
            // print(cur.pos); 
            // print("-----");
            toCheck.Remove(cur);
            if (visited.Contains(cur.pos))
            // if (costs[cur.pos].Item2)
            {
                continue;
            }
            visited.Add(cur.pos);

            // costs.Add(cur.pos, (cur, true)); 
            // costs[cur.pos] = (cur, true);

            // left
            Vector3Int nextPos = cur.pos + Vector3Int.left;

            if (!tmq.HasTile(nextPos))
            {
                // bool visited = false;
                // if (costs.ContainsKey(nextPos))
                // {
                //     (next, visited) = costs[nextPos];
                // }
                // if (!visited)
                if (!visited.Contains(nextPos))
                {
                    hCost = cur.hCost;
                    gCost = cur.gCost + 1;
                    if (targ.x >= cur.pos.x)
                    {
                        hCost++;
                    }
                    else
                    {
                        hCost--;
                    }
                    fCost = hCost + gCost;

                    // if (fCost < maxCartesianDistance && (next == null || fCost < next.fCost))
                    if (fCost < maxCartesianDistance)
                    {
                        PathfindingNode next = new PathfindingNode(fCost, gCost, hCost, nextPos, cur);
                        toCheck.Add(next);
                    }
                }
            }

            // right
            nextPos = cur.pos + Vector3Int.right;

            if (!tmq.HasTile(nextPos))
            {
                if (!visited.Contains(nextPos))
                {
                    hCost = cur.hCost;
                    gCost = cur.gCost + 1;
                    if (targ.x <= cur.pos.x)
                    {
                        hCost++;
                    }
                    else
                    {
                        hCost--;
                    }
                    fCost = hCost + gCost;

                    if (fCost < maxCartesianDistance)
                    {
                        PathfindingNode next = new PathfindingNode(fCost, gCost, hCost, nextPos, cur);
                        toCheck.Add(next);
                    }
                }
            }

            // down 
            nextPos = cur.pos + Vector3Int.down;
            // print(nextPos); 

            if (!tmq.HasTile(nextPos))
            {
                if (!visited.Contains(nextPos))
                {

                    hCost = cur.hCost;
                    gCost = cur.gCost + 1;
                    if (targ.y >= cur.pos.y)
                    {
                        hCost++;
                    }
                    else
                    {
                        hCost--;
                    }
                    fCost = hCost + gCost;

                    if (fCost < maxCartesianDistance)
                    {
                        // print(nextPos);
                        // print(toCheck.ToCommaSeparatedString());
                        PathfindingNode next = new PathfindingNode(fCost, gCost, hCost, nextPos, cur);
                        toCheck.Add(next);
                        // if (toCheck.Add(next)) 
                        // { 
                        //     print("hats");
                        // }
                        // print(toCheck.ToCommaSeparatedString());
                    }
                }
            }

            // up
            nextPos = cur.pos + Vector3Int.up;

            if (!tmq.HasTile(nextPos))
            {
                if (!visited.Contains(nextPos))
                {
                    hCost = cur.hCost;
                    gCost = cur.gCost + 1;
                    if (targ.y <= cur.pos.y)
                    {
                        hCost++;
                    }
                    else
                    {
                        hCost--;
                    }
                    fCost = hCost + gCost;

                    if (fCost < maxCartesianDistance)
                    {
                        PathfindingNode next = new PathfindingNode(fCost, gCost, hCost, nextPos, cur);
                        toCheck.Add(next);
                    }
                }
            }


        }


    }

    // public class PathNode
    // {
    //     public PathNode prev; 
    //     public 
    // }


    // /// <summary>
    // /// Will generate a path for A* using raycasts, could be improved by using an actual minheap lmao
    // /// Reason for using raycasts rather than a grid is to allow moving tiles to be detected
    // /// Was this a good idea? probably not, wasted a lot of time and is probably really slow to compute
    // /// Better idea would be using a grid that updates when it detects that something is inside of a certain part
    // /// </summary>
    // /// <param name="end"></param>
    // public void GenerateAStar(Vector2Int end)
    // {
    //     Dictionary<(int, int), int> costTo = new Dictionary<(int, int), int>();

    //     // 
    //     // current estimated cost to end, pos
    //     SortedDictionary<(float, float, float), (int, int)> toVisit = new SortedDictionary<(float, float, float), (int, int)>();
    //     costTo.Add(((int)transform.position.x, (int)transform.position.y), 0);

    //     while (toVisit.Count > 0)
    //     {
    //         var lowest = toVisit.First();

    //         Vector2 castPos = new Vector2(lowest.Key.Item2, lowest.Key.Item3)

    //         // neighbors
    //         if (!Physics2D.Raycast(castPos, Vector2.up, 1, LayerMask.GetMask("World")))
    //         {
    //             float cost =
    //         }

    //     }
}




