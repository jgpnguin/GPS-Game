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
    public Transform characterModel;
    private bool isFlipped = false;

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

        // Get the mouse position in world space
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Ensure it's in 2D space
        //aim by cursor logic
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);


        // Calculate the aim direction
        Vector3 aimDirection = (mousePos - transform.position).normalized;
        float rotZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (mousePos.x < transform.position.x)
        {
            characterModel.localScale = new Vector3(-1, 1, 1); // Flip player left
            gunModel.localScale = new Vector3(-1, -1, 1); // Flip gun properly
        }
        else
        {
            characterModel.localScale = new Vector3(1, 1, 1); // Flip player right
            gunModel.localScale = new Vector3(1, 1, 1);
        }

        // Rotate gun to follow the cursor
        gunModel.rotation = Quaternion.Euler(0, 0, rotZ);


        // Shooting logic
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            nextFire = Time.time + equippedGun.gunData.fireRate;
            equippedGun.Fire(aimDirection, gunModel);

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
