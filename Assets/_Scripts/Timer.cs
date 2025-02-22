using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public string scene = "GameOver";

    public int timeRem = 0;

    void Start()
    {
        StartCoroutine(TickClock());
    }

    public IEnumerator TickClock()
    {
        while (timeRem > 0)
        {
            timer.text = TimeFormatted(timeRem);
            yield return new WaitForSeconds(1);
            timeRem--;
        }
        SceneSwitcher.SwitchScene(scene);
    }

    public string TimeFormatted(int time)
    {
        int sec = time % 60;
        string secStr = sec <= 9 ? "0" + sec : "" + sec;
        int min = time / 60;
        string minStr = min <= 9 ? "0" + min : "" + min;
        return $"{minStr}:{secStr}";
    }
}
