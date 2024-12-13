using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    public int hour, day;

    private TMP_Text clockTime;
    private TMP_Text clockDay;
    private float lastTimestamp;
    private float gameHourInRealMinutes = 0.02f;

    void Start()
    {
        hour = 8;
        day = 1;
        lastTimestamp = Time.time;

        clockDay = gameObject.transform.Find("Date").GetComponent<TMP_Text>();
        clockTime = gameObject.transform.Find("Time").GetComponent<TMP_Text>();

        SetClockTime();
        SetClockDate();
    }

    void Update()
    {
        if (Time.time - lastTimestamp >= 60 * gameHourInRealMinutes)
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
