using TMPro;
using UnityEngine;

public class RonanTestDriver : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public Entity entity;
    public static RonanTestDriver instance;
    public GameObject test;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            entity.entityHealth.ChangeHealth(-15);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            // tmp.text = "this is a new text thing mhm yep";
            Player.instance.ReloadSafe();
        }
    }

    public void SpawnTest(Vector2 spawnLoc)
    {
        Instantiate(test, spawnLoc, Quaternion.identity);
    }
}
