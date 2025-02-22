using UnityEditor;
using UnityEngine;
   


[CreateAssetMenu(fileName = "gunOS", menuName = "Scriptable Objects/gunOS")]
public class gunOS : ScriptableObject
{
    
    public float fireRate;
    public int damage;
    public GameObject bulletPrefab;
    public int bulletsNumber;
    public int maxBullets;
    public float reloadTime;
    public bulletOS bulletData;
    public bool isBoundce;
    public float force;
}
