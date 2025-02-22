using UnityEngine;

public class LineRendererManager : MonoBehaviour
{
    public LineRenderer lr;
    public bool preserveTargetRelative = false;
    public Vector3 target;

    public void Start()
    {
        SetLaserActive(false);
    }

    public void Update()
    {
        lr.SetPosition(0, transform.position);
        if (preserveTargetRelative)
        {
            lr.SetPosition(1, transform.position + target);
        }
    }

    public void SetPositionGlobal(Vector3 position)
    {
        lr.SetPosition(1, position);
        target = position - transform.position;
    }

    public void SetLaserActive(bool state)
    {
        lr.enabled = state;
    }
}
