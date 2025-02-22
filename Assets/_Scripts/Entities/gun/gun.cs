using UnityEngine;

public abstract class gun : MonoBehaviour
{
    public gunOS gunData;

    public abstract void Fire(Vector2 direction, Transform gunModel);
}
