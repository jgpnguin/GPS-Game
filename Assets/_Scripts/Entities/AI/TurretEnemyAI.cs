using System.Collections;
using UnityEngine;

public class TurretEnemyAI : MonoBehaviour
{
    public enum State
    {
        Search,
        PrepShoot,
        PrepSearch,
        Shooting,

    }


    public Transform viewPoint;
    public LineRendererManager lrMan;
    [SerializeField] private Vector2 target = Vector2.zero;
    [SerializeField] private State state = State.Search;

    [Header("Search")]
    public float fireRange = 5;

    [Header("Prep Shoot")]
    public float timePrepShoot = 1.5f;
    public float timePrepSearch = 1.5f;

    [Header("Shooting")]
    // public bool aimWhileFiring = true; 
    // negative for aim while firing
    public float timeShoot = 3f;

    void Update()
    {
        bool playerLOS;
        Vector2 playerLOSVec;
        (playerLOS, playerLOSVec) = HasPlayerLOS();
        if (playerLOS)
        {
            target = playerLOSVec;
        }
        switch (state)
        {
            case State.Search:
                if (playerLOS)
                {
                    state = State.PrepShoot;
                    lrMan.SetLaserActive(true);
                    StartCoroutine(PrepShootTime());
                }
                break;
            case State.PrepShoot:
                if (!playerLOS)
                {
                    state = State.PrepSearch;
                    StopCoroutine(PrepShootTime());
                    StartCoroutine(PrepSearchTime());
                }
                else
                {
                    lrMan.SetPositionGlobal(playerLOSVec);
                }
                break;
            case State.PrepSearch:
                if (playerLOS)
                {
                    state = State.PrepShoot;
                    StopCoroutine(PrepSearchTime());
                    StartCoroutine(PrepShootTime());
                }
                else
                {
                    lrMan.SetPositionGlobal(WallHitAttempt(target));
                }
                break;
            case State.Shooting:
                if (timeShoot > 0f)
                {
                    Debug.Log("shot without aiming");
                }
                else if (playerLOS)
                {
                    Debug.Log("shot with aiming");
                }
                else
                {
                    state = State.Search;
                }

                break;
        }
    }

    private IEnumerator PrepShootTime()
    {
        yield return new WaitForSeconds(timePrepShoot);
        lrMan.SetLaserActive(false);
        state = State.Shooting;
        StartCoroutine(ShootingTime());
    }

    private IEnumerator PrepSearchTime()
    {
        yield return new WaitForSeconds(timePrepSearch);
        lrMan.SetLaserActive(false);
        state = State.Search;
    }

    private IEnumerator ShootingTime()
    {
        if (timeShoot > 0f)
        {
            yield return new WaitForSeconds(timeShoot);
            state = State.Search;
        }
        else
        {
            yield return null;
        }
    }

    public (bool, Vector2) HasPlayerLOS() // remind me to add a fake player transform that follows player but is smoothed to make it seem like the camera can't completely just jump instantly
    {
        Vector2 dir = Player.instance.transform.position - viewPoint.position;
        RaycastHit2D hit = Physics2D.Raycast(viewPoint.position, dir, fireRange, LayerMask.GetMask("Entity", "World"));
        if (hit && hit.collider.gameObject == Player.instance.gameObject)
        {
            return (true, hit.point);
        }
        return (false, Vector2.zero);
    }

    public Vector3 WallHitAttempt(Vector2 target)
    {
        Vector2 dir = target - (Vector2)viewPoint.position;
        RaycastHit2D wall = Physics2D.Raycast(viewPoint.position, dir, 99f, LayerMask.GetMask("Entity", "World"));
        return wall ? wall.point : dir.normalized * 99f;
    }
}
