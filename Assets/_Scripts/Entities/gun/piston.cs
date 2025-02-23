using UnityEngine;

public class piston : gun
{
    public Rigidbody2D playerRb;
    public AudioSource laserSound = null;

    private Entity ownerEntity; // Reference to the entity that owns this gun

    void Start()
    {
        // Get the owner's Entity component
        ownerEntity = GetComponentInParent<Entity>();
        if (ownerEntity == null)
        {
            Debug.LogError("Owner Entity not found!");
        }
    }
    public override void Fire(Vector2 direction, Transform gunModel)
    {
        if (laserSound != null)
        {
            laserSound.Play();
        }
        
        GameObject bullet = Instantiate(gunData.bulletPrefab, gunModel.position, Quaternion.identity);
        bullet.transform.right = direction;
        //bullet.GetComponent<attack>().ownerID = ownerID;
        if (gunData.isBoundce)
        {
            playerRb.AddForce(-direction * gunData.force, ForceMode2D.Impulse);
        }

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            // Pass damage from GunSO and bullet data from BulletOS
            bulletScript.Initialize(gunData.damage, gunData.bulletData.speed, gunData.bulletData.lifetime, ownerEntity.id, gunData.bulletData.kb);
        }
        else
        {
            Debug.LogError("Bullet prefab is missing Bullet script!");
        }
    }
}
