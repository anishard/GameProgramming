using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    public static int hour = 7, day = 1;
    public static float gameHourInRealMinutes = 0.02f;//1f;
    public static int TotalHours {
        get { return day * 24 + hour; }
    }

    private TMP_Text clockTime;
    private TMP_Text clockDay;
    private float lastTimestamp;
    private static float pauseDuration;
    private static bool isPaused;
    private static bool stopPause;

    void Awake()
    {
        lastTimestamp = Time.time;
        pauseDuration = 0;
        isPaused = false;
        stopPause = false;

        clockDay = gameObject.transform.Find("Date").GetComponent<TMP_Text>();
        clockTime = gameObject.transform.Find("Time").GetComponent<TMP_Text>();

        SetClockTime();
        SetClockDate();
    }

    void Update()
    {
        if (isPaused && stopPause)
        {
            lastTimestamp += pauseDuration;
            isPaused = false;
            stopPause = false;
        }

        if (!isPaused && (Time.time - lastTimestamp >= 60 * gameHourInRealMinutes))
        {
            hour++;
            lastTimestamp = Time.time;
            SetClockTime();

            if (hour == 24)
            {
                day++;
                hour = 0;
                SetClockDate();
            }
        }
    }

    public static void Pause()
    {
        pauseDuration = Time.time;
        isPaused = true;
    }

    public static void Resume()
    {
        pauseDuration = Time.time - pauseDuration;
        stopPause = true;
    }

    private void SetClockDate()
    {
        clockDay.text = $"Day {day}";
    }

    private void SetClockTime()
    {
        int clockHour = hour;
        string period = hour < 12 || hour == 24 ? "AM" : "PM";

        if (hour == 0 || hour == 12)
            clockHour = 12;
        else if (hour > 12)
            clockHour = hour - 12;

        clockTime.text = $"{clockHour}:00 {period}";
    }
}
