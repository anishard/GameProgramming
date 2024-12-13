using System.Collections;
using UnityEngine;

public class SkyLerp : MonoBehaviour
{
    enum TransitionTime
    {
        Morning = 6,  // 6am
        Day = 9,      // 9am
        Evening = 19, // 7pm
        Night = 21    // 9pm
    }

    public Material morning;
    public Material day;
    public Material evening;
    public Material night;

    private float duration; // in seconds
    private Renderer rend;
    private Light lightSource;
    private Color dayColor;
    private Color nightColor;
    private bool isTransitioning;

    void Start()
    {
        duration = 60 * Clock.gameHourInRealMinutes; // 1 game hour
        isTransitioning = false;

        rend = GetComponent<Renderer>();
        lightSource = FindObjectOfType<Light>();

        dayColor = CreateColor(255, 244, 214);
        nightColor = CreateColor(121, 152, 255);

        (Material, Color) data = GetDataFromHour(Clock.hour);
        rend.material = data.Item1;
        lightSource.color = data.Item2;

        Clock.hour = 18;

        StartCoroutine(Transition());
    }

    void Update()
    {
        if (!isTransitioning)
        {
            if (
                Clock.hour == (int)TransitionTime.Morning  // transition to morning
             || Clock.hour == (int)TransitionTime.Day      // to day
             || Clock.hour == (int)TransitionTime.Evening  // to evening
             || Clock.hour == (int)TransitionTime.Night    // to night
             )
                StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        isTransitioning = true;

        Material material1 = GetDataFromHour(Clock.hour - 1).Item1;
        Material material2 = GetDataFromHour(Clock.hour).Item1;

        bool addDayColor = Clock.hour == (int)TransitionTime.Morning;
        bool addNightColor = Clock.hour == (int)TransitionTime.Night;

        float curTime = 0;

        while (curTime < duration)
        {
            float t = curTime / duration;

            rend.material.Lerp(material1, material2, t);

            if (addDayColor)
                lightSource.color = Color.Lerp(nightColor, dayColor, t);

            if (addNightColor)
                lightSource.color = Color.Lerp(dayColor, nightColor, t);

            curTime += Time.deltaTime;
            yield return null;
        }

        rend.material = material2;

        isTransitioning = false;
    }

    Color CreateColor(int r, int g, int b)
    {
        return new Color(r / 255f, g / 255f, b / 255f);

    }

    (Material, Color) GetDataFromHour(int h)
    {
        if (h >= (int)TransitionTime.Morning && h < (int)TransitionTime.Day)
            return (morning, dayColor);

        if (h >= (int)TransitionTime.Day && h < (int)TransitionTime.Evening)
            return (day, dayColor);

        if (h >= (int)TransitionTime.Evening && h < (int)TransitionTime.Night)
            return (evening, dayColor);

        else return (night, nightColor);
    }
}