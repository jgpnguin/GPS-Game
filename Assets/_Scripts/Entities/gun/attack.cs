using Unity.VisualScripting;
using UnityEngine;

public class attack : MonoBehaviour
{
    // cameraview initialization
    private Camera mainCam;
    private Vector3 mousePos;
    public gun equippedGun;
    public Transform gunModel;
    public int ownerID = 0;
    public float nextFire = 0.0f;

    //temp
    //public GameObject bulletPrefab;
    //public Transform gun;
    //public float speed=100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if (equippedGun == null)
        {
            Debug.LogError("No gun component found on this GameObject!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        //aim by cursor logic
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        //get the direction by the mouse cursor
        Vector2 direction = (mousePos - transform.position).normalized;

        //check weather to shoot or not
        if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
        {
            nextFire = Time.time + equippedGun.gunData.fireRate;
            equippedGun.Fire(direction, gunModel);
        }

    }

    /*public void fire(Vector2 direction)
    {
        GameObject bullet = Instantiate(eqiuppedWeapon.bulletPrefab,gun.position, Quaternion.identity);
        bullet.transform.right = direction;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();


        if (rb != null)
        {
            rb.linearVelocity = direction* eqiuppedWeapon.speed;
            if (eqiuppedWeapon.isBoundce)
            {
                this.rb.AddForce(-direction * force, ForceMode2D.Impulse);
            }

           
        }

    }*/
}
