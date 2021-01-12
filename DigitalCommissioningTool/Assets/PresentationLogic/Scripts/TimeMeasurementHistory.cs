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

    /// <summary>
    /// Emtdfernt alle Eventfunktionen, wenn Objekt zerstört wird
    /// </summary>
    void OnDestroy()
    {
        Timer.TimerReset -= OnTimerReset;
        Timer.TimerStopped -= OnTimerStopped;
        GameManager.PManager.FinishLoad -= ProjectManagerOnFinishLoad;
        GameManager.PManager.StartClose -= ProjectManagerOnStartClose;
        GameManager.PManager.StartSave -= ProjectManagerOnStartSave;
        GameManager.PManager.ProjectCreated -= ProjectManagerOnProjectCreated;
    }

    /// <summary>
    /// Setzt alle Eventfunktionen, wenn das Skript geladen wird
    /// </summary>
    void OnValidate()
    {
        LogManager.WriteError( "testValidate" );
        //ResourceHandler.ReplaceResources();
        Timer.TimerStopped += OnTimerStopped;
        Timer.TimerReset += OnTimerReset;
        GameManager.PManager.FinishLoad += ProjectManagerOnFinishLoad;
        GameManager.PManager.StartClose += ProjectManagerOnStartClose;
        GameManager.PManager.StartSave += ProjectManagerOnStartSave;
        GameManager.PManager.ProjectCreated += ProjectManagerOnProjectCreated;
        configManager = new ConfigManager( );
    }

    
    void OnStart()
    {
        LogManager.WriteError( "test" );
        //ResourceHandler.ReplaceResources();
        Timer.TimerStopped += OnTimerStopped;
        Timer.TimerReset += OnTimerReset;
        GameManager.PManager.FinishLoad += ProjectManagerOnFinishLoad;
        GameManager.PManager.StartClose += ProjectManagerOnStartClose;
        GameManager.PManager.StartSave += ProjectManagerOnStartSave;
        GameManager.PManager.ProjectCreated += ProjectManagerOnProjectCreated;
        configManager = new ConfigManager( );
    }

    /// <summary>
    /// Schreibt die Änderungen in die XML Datei wenn Projekt gespeichert wird
    /// </summary>
    private void ProjectManagerOnStartSave()
    {
        if (configManager != null)
        {
            configManager.Flush();
        }
    }

    /// <summary>
    /// Erstellt ConfigManager Instanz wenn Projekt erstellt wird
    /// </summary>
    private void ProjectManagerOnProjectCreated()
    {
        LogManager.WriteError( "OnProjectCreatedCalled" );
        configManager.OpenConfigFile(Paths.TempPath, "TimeMeasurements.xml", true);
    }

    /// <summary>
    /// Schließt die Config Datei wenn Projekt geschlossen wird.
    /// </summary>
    private void ProjectManagerOnStartClose()
    {
        if (configManager != null)
        {
            configManager.AutoFlush = false;
            configManager.CloseConfigFile();
        }
    }

    /// <summary>
    /// Lädt eventuell vorhandene Zeitmessungseinträge und schreibt sie in die UI Tabelle
    /// </summary>
    private void ProjectManagerOnFinishLoad()
    {
        LogManager.WriteError( "OnFinishLoadCalled" );

        configManager.OpenConfigFile(Paths.TempPath, "TimeMeasurements.xml", true);

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
            AddItemToList(temp);
            currentIndex++;
        }

        UpdateSize();
    }


    /// <summary>
    /// Wird aufgerufen wenn der Timer Reset ausgeführt wird und passt den Eintrag in der Tabelle entsprechend an 
    /// </summary>
    /// <param name="currentTime">Aktuelle Zeit des Timers</param>
    /// <param name="buttonText">Text der auf dem Play/Pause/Resume Button aktuell angezeigt wird</param>
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

    /// <summary>
    /// Wird aufgerufen wenn Timer gestoppt wird und trägt aktuelle Zeit in die Tabelle ein
    /// </summary>
    /// <param name="currentTime">Aktuelle Zeit des Timers</param>
    /// <param name="buttonText">Text der auf dem Play/Pause/Resume Button aktuell angezeigt wird</param>
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

    /// <summary>
    /// Trägt Zeitmessung in die UI Tabelle ein
    /// </summary>
    /// <param name="item">Zeitmessung die eingetragen werden soll</param>
    private void AddItemToList(TimeMeasurementEntry item)
    {
        TimeMeasurementRowTemplate.transform.Find("Index/IndexText").GetComponent<TextMeshProUGUI>().text = item.Index.ToString();
        TimeMeasurementRowTemplate.transform.Find("CreatedAt/CreatedAtText").GetComponent<TextMeshProUGUI>().text = item.CreatedAt.ToString();
        TimeMeasurementRowTemplate.transform.Find("Duration/DurationText").GetComponent<TextMeshProUGUI>().text = item.Duration.ToString("0.00") + "s";
        GameObject field = Instantiate(TimeMeasurementRowTemplate, TimeMeasurementRowTemplate.transform.parent);
        field.SetActive(true);
    }

    /// <summary>
    /// Ändert den angegebenen Zeitmessungseintrag in der UI Tabelle ab
    /// </summary>
    /// <param name="item">Zeitmessung die angepasst werden soll</param>
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