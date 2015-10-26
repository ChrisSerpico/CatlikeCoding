using System;
using UnityEngine;

public class ClockAnimator : MonoBehaviour
{
    // the hands of the clock
    public Transform hours, minutes, seconds;

    // constants for converting time to degrees
    private const float 
        HOURS_TO_DEGREES = 360f/12f,
        MINUTES_TO_DEGREES = 360f/60f, 
        SECONDS_TO_DEGREES = 360f/60f;

    // whether the clock is digital (hands only show discrete steps) 
    // or analog (hands are all at relative positions)
    public bool analog; 


    private void Update()
    {
        if (analog)
        {
            // Get the current time
            TimeSpan timespan = DateTime.Now.TimeOfDay;

            // rotate arms
            hours.localRotation = Quaternion.Euler(0f, 0f, (float) timespan.TotalHours * -HOURS_TO_DEGREES);
            minutes.localRotation = Quaternion.Euler(0f, 0f, (float) timespan.TotalMinutes * -MINUTES_TO_DEGREES);
            seconds.localRotation = Quaternion.Euler(0f, 0f, (float) timespan.TotalSeconds * -SECONDS_TO_DEGREES);
        }
        else
        {
            // The current time
            DateTime time = DateTime.Now;

            // rotate arms
            hours.localRotation = Quaternion.Euler(0f, 0f, time.Hour * -HOURS_TO_DEGREES);
            minutes.localRotation = Quaternion.Euler(0f, 0f, time.Minute * -MINUTES_TO_DEGREES);
            seconds.localRotation = Quaternion.Euler(0f, 0f, time.Second * -SECONDS_TO_DEGREES);
        }
    }
}