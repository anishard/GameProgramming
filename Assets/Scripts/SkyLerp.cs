using System.Collections;
using UnityEngine;

public class SkyLerp : MonoBehaviour
{

    public Material morning;
    public Material day;
    public Material evening;
    public Material night;

    private Renderer rend;
    private float duration; // in seconds
    private bool isTransitioning;

    void Start()
    {
        duration = 60 * Clock.gameHourInRealMinutes; // 1 game hour
        isTransitioning = false;

        rend = GetComponent<Renderer>();
        rend.material = GetMaterialFromHour(Clock.hour);
    }

    void Update()
    {
        if (!isTransitioning)
        {
            if (Clock.hour == 10) // to day at 10am
                StartCoroutine(Transition());

            if (Clock.hour == 19) // to evening at 7pm
                StartCoroutine(Transition());

            if (Clock.hour == 21) // to night at 9pm
                StartCoroutine(Transition());

            if (Clock.hour == 5) // to morning at 5am
                StartCoroutine(Transition());
        }
    }

    Material GetMaterialFromHour(int h)
    {
        if (h >= 5 && h < 10) return morning;

        if (h >= 10 && h < 19) return day;

        if (h >= 19 && h < 21) return evening;

        else return night;
    }

    IEnumerator Transition()
    {
        isTransitioning = true;

        Material material1 = GetMaterialFromHour(Clock.hour - 1);
        Material material2 = GetMaterialFromHour(Clock.hour);

        float curTime = 0;

        while (curTime < duration)
        {
            float t = curTime / duration;
            rend.material.Lerp(material1, material2, t);
            curTime += Time.deltaTime;
            yield return null;
        }

        rend.material = material2;

        isTransitioning = false;
    }
}