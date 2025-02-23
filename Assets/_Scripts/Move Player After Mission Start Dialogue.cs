using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayerAfterMissionStartDialogue : MonoBehaviour
{
    public int lastIndex;
    public Entity player;
    public Dialogue Dialogue;
    public TextMeshProUGUI text;
    public float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        if ((text.text == Dialogue.lines[lastIndex]) && (player.transform.position.x <= 10f))
        {
            player.transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
    }
}
