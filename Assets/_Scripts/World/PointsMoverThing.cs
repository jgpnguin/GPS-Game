using System.Collections;
using UnityEngine;

public class PointsMoverThing : MonoBehaviour
{

    [System.Serializable]
    public struct StopPoint
    {
        public Transform pos;
        public float time;
    }
    public StopPoint[] stopPointsAndTime;
    // public float[] stopPointsTime;
    public int stopPointsInd = 0;
    public bool rollover = false;
    private bool forward = false;
    public Rigidbody2D rb;
    public float speed = .05f;
    private Coroutine waiting = null;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        Vector2 distance = stopPointsAndTime[stopPointsInd].pos.position - transform.position;
        if (distance.magnitude < 0.01f && waiting == null)
        {
            waiting = StartCoroutine(WaitForRollover(stopPointsAndTime[stopPointsInd].time));
        }
        else
        {
            if (distance.magnitude < speed)
            {
                rb.MovePosition((Vector2)transform.position + distance);
            }
            else
            {
                rb.MovePosition((Vector2)transform.position + (distance.normalized * speed));
            }
        }

    }

    private IEnumerator WaitForRollover(float time)
    {
        yield return new WaitForSeconds(time);
        if (stopPointsAndTime.Length > 1)
        {
            if (forward)
            {
                stopPointsInd++;
                if (stopPointsInd >= stopPointsAndTime.Length)
                {
                    if (rollover)
                    {
                        stopPointsInd = 0;
                    }
                    else
                    {
                        forward = false;
                        stopPointsInd -= 2;
                    }
                }
            }
            else
            {
                if (stopPointsInd <= 0)
                {
                    if (rollover)
                    {
                        stopPointsInd = stopPointsAndTime.Length - 1;
                    }
                    else
                    {
                        forward = true;
                        stopPointsInd++;
                    }
                }
                else
                {
                    stopPointsInd--;
                }
            }
        }
        waiting = null;

    }
}
