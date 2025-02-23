using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLevel : MonoBehaviour
{
    public Collider2D Trigger;
    public string SceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D Trigger)
    {
        SceneManager.LoadScene(SceneName);
    }
}
