using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBlackerController : MonoBehaviour
{
    public Image screenCoverImg;
    public static ScreenBlackerController instance;
    public float speed = 1f;
    private Coroutine processing;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Two screen hiders detected, destroying second");
            Destroy(gameObject);
        }
    }


    public void SetScreenHideAndUnhide(float timeWaitHidden = 0.25f, float speedHide = 1f, float speedUnhide = 1f)
    {
        if (processing != null)
        {
            StopCoroutine(processing);
        }
        processing = StartCoroutine(HideAndUnhide(timeWaitHidden, speedHide, speedUnhide));
    }

    private IEnumerator HideAndUnhide(float timeWaitHidden, float speedHide, float speedUnhide)
    {
        speed = speedHide;
        while (screenCoverImg.color.a < 1)
        {
            Color color = screenCoverImg.color;
            color.a += speed * Time.deltaTime;
            screenCoverImg.color = color;
            yield return null;
        }
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(timeWaitHidden);
        speed = speedUnhide;
        while (screenCoverImg.color.a > 0)
        {
            Color color = screenCoverImg.color;
            color.a -= speed * Time.deltaTime;
            screenCoverImg.color = color;
            yield return null;
        }
        Time.timeScale = 1f;
    }
}
