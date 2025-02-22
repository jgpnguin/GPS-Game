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


    public void SetScreenHideAndUnhideResetChar(float timeWaitHidden = 0.25f, float timeHide = 0.5f, float timeUnhide = 0.5f)
    {
        if (processing != null)
        {
            StopCoroutine(processing);
        }
        timeHide += .001f;
        timeUnhide += .001f;
        processing = StartCoroutine(HideAndUnhideResetChar(timeWaitHidden, 1 / timeHide, 1 / timeUnhide));
    }

    private IEnumerator HideAndUnhideResetChar(float timeWaitHidden, float speedHide, float speedUnhide)
    {
        Time.timeScale = 0f;
        speed = speedHide;
        while (screenCoverImg.color.a < 1)
        {
            Color color = screenCoverImg.color;
            color.a += speed * Time.unscaledDeltaTime;
            screenCoverImg.color = color;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(timeWaitHidden);
        Player.instance.ReloadSafe();
        Time.timeScale = 1f;
        speed = speedUnhide;
        while (screenCoverImg.color.a > 0)
        {
            Color color = screenCoverImg.color;
            color.a -= speed * Time.unscaledDeltaTime;
            screenCoverImg.color = color;
            yield return null;
        }
    }
}
