using System;
using System.Collections.Generic;
using System.Globalization;
using SystemFacade;
using ApplicationFacade.Application;
using TimeMeasurement;
using TMPro;
using UnityEngine;

public class TimeMeasurementHistory : MonoBehaviour
{
    public GameObject TimeMeasurementRowTemplate;
    public GameObject PanelTimeMeasurementHistory;
    public List<TimeMeasurementEntry> timeMeasurementEntries = new List<TimeMeasurementEntry>();
    private ConfigManager configManager;
    private ProjectManager projectManager;
    private int currentIndex = 0;

    void OnDestroy()
    {
        Timer.TimerReset -= OnTimerReset;
        Timer.TimerStopped -= OnTimerStopped;
        projectManager.FinishLoad -= ProjectManagerOnFinishLoad;
        projectManager.StartClose -= ProjectManagerOnStartClose;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        ResourceHandler.ReplaceResources();
        configManager = new ConfigManager();
        projectManager = new ProjectManager();
        configManager.OpenConfigFile(Paths.TempPath, "TimeMeasurements.xml", true);

        Timer.TimerStopped += OnTimerStopped;
        Timer.TimerReset += OnTimerReset;
        projectManager.FinishLoad += ProjectManagerOnFinishLoad;
        projectManager.StartClose += ProjectManagerOnStartClose;
    }

    private void ProjectManagerOnStartClose()
    {
        configManager.CloseConfigFile();
    }

    private void ProjectManagerOnFinishLoad()
    {
        for (int i = 0; i < 1000; i++)
        {
            
            string key = "TimeMeasurement" + i;
            TimeMeasurementEntry temp = new TimeMeasurementEntry();
            if (configManager.LoadData(key) == null)
            {
                break;
            }
            
            configManager.LoadData(key, temp);
            timeMeasurementEntries.Add(temp);
            Debug.Log("OnFinishLoad, Index: " + temp.Index);
            AddItemToList(temp);
            currentIndex++;
        }
        UpdateSize();
        currentIndex++;
        
    }


    private void OnTimerReset(float currentTime)
    {
        if (currentTime > 0)
        {
            currentIndex++;
        }
    }

    private void OnTimerStopped(float currentTime)
    {
        TimeMeasurementEntry timeMeasurementEntry = new TimeMeasurementEntry(currentIndex, DateTime.Now.ToString(), currentTime);
        if (currentIndex >= timeMeasurementEntries.Count)
        {
            timeMeasurementEntries.Add(timeMeasurementEntry);
            AddItemToList(timeMeasurementEntry);
            UpdateSize();
        }
        else
        {
            timeMeasurementEntries[currentIndex] = timeMeasurementEntry;
            ChangeItemInList(timeMeasurementEntry);
        }

        configManager.StoreData("TimeMeasurement" + currentIndex, timeMeasurementEntry, true);

    }

    private void AddItemToList(TimeMeasurementEntry item)
    {
        TimeMeasurementRowTemplate.transform.Find("Index/IndexText").GetComponent<TextMeshProUGUI>().text = item.Index.ToString();
        TimeMeasurementRowTemplate.transform.Find("CreatedAt/CreatedAtText").GetComponent<TextMeshProUGUI>().text = item.CreatedAt.ToString();
        TimeMeasurementRowTemplate.transform.Find("Duration/DurationText").GetComponent<TextMeshProUGUI>().text = item.Duration.ToString("0.00") + "s";
        GameObject field = Instantiate(TimeMeasurementRowTemplate, TimeMeasurementRowTemplate.transform.parent);
        field.SetActive(true);
    }
    
    private void ChangeItemInList(TimeMeasurementEntry item)
    {
        Transform field = TimeMeasurementRowTemplate.transform.parent.GetChild(item.Index + 1);
        field.Find("Index/IndexText").GetComponent<TextMeshProUGUI>().text = item.Index.ToString();
        field.Find("CreatedAt/CreatedAtText").GetComponent<TextMeshProUGUI>().text = item.CreatedAt.ToString();
        field.Find("Duration/DurationText").GetComponent<TextMeshProUGUI>().text = item.Duration.ToString("0.00") + "s";
    }

    private void UpdateSize()
    {
        float newHeight = System.Math.Max(180, 10 + (timeMeasurementEntries.Count) * 30);
        RectTransform contentBox = TimeMeasurementRowTemplate.transform.parent.GetComponent<RectTransform>();
        contentBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
    }
    
    // Update is called once per frame
    void Update()
    {
    }
    
}