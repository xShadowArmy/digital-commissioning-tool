using System;
using System.Collections;
using System.Collections.Generic;
using SystemFacade;
using ApplicationFacade;
using UnityEngine;

public class TimeMeasurementHistory : MonoBehaviour
{

    public List<TimeMeasurementEntry> timeMeasurementEntries;
    // Start is called before the first frame update
    void Start()
    {
        Timer.TimerStopped += OnTimerStopped;
        //using (ConfigManager cman = new ConfigManager())
        //{
        //    cman.OpenConfigFile("TimeMeasurements.xml", true);
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        string key = "TimeMeasurement" + i;
        //        TimeMeasurementEntry temp = new TimeMeasurementEntry();
        //        if (cman.LoadData(key) == null)
        //        {
        //            break;
        //        }
        //        cman.LoadData(key, temp);
        //        timeMeasurementEntries.Add(temp);
        //    }
        //}
    }

    private void OnTimerStopped(float currentTime)
    {
        Debug.Log(currentTime);
        TimeMeasurementEntry timeMeasurementEntry = new TimeMeasurementEntry(timeMeasurementEntries.Count, DateTime.Now.ToString(), currentTime);
        timeMeasurementEntries.Add(timeMeasurementEntry);
        //using (ConfigManager cman = new ConfigManager())
        //{
        //  cman.OpenConfigFile("TimeMeasurements.xml", true);
        //  cman.StoreData("TimeMeasurement" + timeMeasurementEntries.Count, timeMeasurementEntry);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
