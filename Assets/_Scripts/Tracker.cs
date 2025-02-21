using UnityEngine;

public class Tracker : MonoBehaviour
{
    public Transform host;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(host.position.x, host.position.y, transform.position.z);
    }
}
