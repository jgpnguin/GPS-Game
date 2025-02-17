using System.Collections;
using UnityEngine;

public class TurretEnemyAI : MonoBehaviour
{
    public Transform viewPoint;
    [SerializeField] private float fireRange = 5;


    public bool HasPlayerLOS()
    {
        Vector2 dir = Player.instance.transform.position - viewPoint.position;
        return Physics2D.Raycast(viewPoint.position, dir, fireRange, LayerMask.GetMask("Entity", "World"));
    }

}
