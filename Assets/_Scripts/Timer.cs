using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public string scene = "GameOver";

    public int timeRem = 600;
    public int timeRemFlicker = 60;
    public Color flicker1 = Color.red;
    public float flicker1Time = 0.25f;
    public Color flicker2 = Color.clear;
    public float flicker2Time = 0.25f;
    public Coroutine flickering;

    void Start()
    {
        StartCoroutine(TickClock());
    }

    public IEnumerator Flicker()
    {
        while (true)
        {
            timer.color = flicker1;
            yield return new WaitForSeconds(flicker1Time);
            timer.color = flicker2;
            yield return new WaitForSeconds(flicker2Time);
        }
    }

    public IEnumerator TickClock()
    {
        while (timeRem > 0)
        {
            timer.text = TimeFormatted(timeRem);
            yield return new WaitForSeconds(1);
            timeRem--;
            if (flickering == null && timeRem <= timeRemFlicker)
            {
                flickering = StartCoroutine(Flicker());
            }
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
