using UnityEngine;

public class piston : gun
{
    public Rigidbody2D playerRb;
    public override void Fire(Vector2 direction)
    {
        GameObject bullet = Instantiate(gunData.bulletPrefab, transform.position, Quaternion.identity);
        bullet.transform.right = direction;
        if (gunData.isBoundce)
        {
            playerRb.AddForce(-direction * gunData.force, ForceMode2D.Impulse);
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            // Pass damage from GunSO and bullet data from BulletOS
            bulletScript.Initialize(gunData.damage, gunData.bulletData.speed, gunData.bulletData.lifetime);
        }
        else
        {
            Debug.LogError("Bullet prefab is missing Bullet script!");
        }
    }
}
