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
        GameManager.PManager.FinishLoad -= ProjectManagerOnFinishLoad;
        GameManager.PManager.StartClose -= ProjectManagerOnStartClose;
        GameManager.PManager.StartSave -= ProjectManagerOnStartSave;
        GameManager.PManager.ProjectCreated -= ProjectManagerOnProjectCreated;
    }

    // Start is called before the first frame update
    void OnValidate()
    {
        //ResourceHandler.ReplaceResources();
        Timer.TimerStopped += OnTimerStopped;
        Timer.TimerReset += OnTimerReset;
        GameManager.PManager.FinishLoad += ProjectManagerOnFinishLoad;
        GameManager.PManager.StartClose += ProjectManagerOnStartClose;
        GameManager.PManager.StartSave += ProjectManagerOnStartSave;
        GameManager.PManager.ProjectCreated += ProjectManagerOnProjectCreated;
    }

    private void ProjectManagerOnStartSave()
    {
        if (configManager != null)
        {
            configManager.Flush();
        }
    }

    private void ProjectManagerOnProjectCreated()
    {
        configManager = new ConfigManager();
        configManager.OpenConfigFile(Paths.TempPath, "TimeMeasurements.xml", true);
    }

    private void ProjectManagerOnStartClose()
    {
        if (configManager != null)
        {
            configManager.AutoFlush = false;
            configManager.CloseConfigFile();
        }
    }

    private void ProjectManagerOnFinishLoad()
    {
        configManager = new ConfigManager();
        configManager.OpenConfigFile(Paths.TempPath, "TimeMeasurements.xml", true);

        for (int i = 1; i < 1000; i++)
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


    private void OnTimerReset(float currentTime, string buttonText)
    {
        if (currentTime > 0)
        {
            if (buttonText.Equals("Stop"))
            {
                OnTimerStopped(currentTime, buttonText);
            }
            
            currentIndex++;
        }
    }

    private void OnTimerStopped(float currentTime, string buttonText)
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