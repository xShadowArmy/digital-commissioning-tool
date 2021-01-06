using System;
using System.Collections.Generic;
using SystemFacade;
using TimeMeasurement;
using UnityEngine;




public class TimeMeasurementHistory : MonoBehaviour
{
    
    private List<TimeMeasurementEntry> timeMeasurementEntries = new List<TimeMeasurementEntry>();
    private ConfigManager cman = new ConfigManager();

    // Start is called before the first frame update
    void Start()
    {
        Timer.TimerStopped += OnTimerStopped;
        Timer.TimerReset += OnTimerReset;
        cman.OpenConfigFile(Paths.TempPath, "TimeMeasurements.xml", true);
        for (int i = 1; i < 1000; i++)
        {
            string key = "TimeMeasurement" + i;
            TimeMeasurementEntry temp = new TimeMeasurementEntry();
            if (cman.LoadData(key) == null)
            {
                break;
            }

            cman.LoadData(key, temp);
            timeMeasurementEntries.Add(temp);
        }
    }


    private void OnTimerReset(float currentTime)
    {
        for (int i = 1; i <= timeMeasurementEntries.Count; i++)
        {
            string key = "TimeMeasurement" + i;
            cman.RemoveData(key);
        }

        timeMeasurementEntries.Clear();
    }

    private void OnTimerStopped(float currentTime)
    {
        TimeMeasurementEntry timeMeasurementEntry = new TimeMeasurementEntry(timeMeasurementEntries.Count, DateTime.Now.ToString(), currentTime);
        timeMeasurementEntries.Add(timeMeasurementEntry);
        cman.StoreData("TimeMeasurement" + timeMeasurementEntries.Count, timeMeasurementEntry);
    }

    // Update is called once per frame
    void Update()
    {
    }
    
}