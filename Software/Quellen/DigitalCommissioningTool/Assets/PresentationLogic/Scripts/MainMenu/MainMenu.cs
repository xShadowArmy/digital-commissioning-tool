using System.Collections;
using System.Collections.Generic;
using SystemFacade;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ApplicationFacade;

public class MainMenu : MonoBehaviour
{
    public GameObject SettingsPanel;
    public GameObject NewProjectPanel;
    public GameObject OpenProjectPanel;
    public GameObject KeyBindPanel;
    /// <summary>
    /// Lädt "DefaultWarehouse" Szene, Einstellungen und lokalisierte Ressourcen.
    /// </summary>
    void Start()
    {
        if (!SceneManager.GetSceneByName("DefaultWarehouse").isLoaded)
        {
            SceneManager.LoadScene("DefaultWarehouse", LoadSceneMode.Additive);
        }
        GameObject[] gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject g in gameObjects)
        {
            if (g.name.Equals("CanvasTreeView"))
            {
                g.SetActive(!g.activeSelf);
            }
        }
        SettingsPanel.GetComponent<SettingsMenu>().LoadSettings();
        ResourceHandler.ReplaceResources(); 
    }
    /// <summary>
    /// Öffnet Fenster zum Erstellen eines neuen Projektes.
    /// </summary>
    public void NewProject()
    {
        NewProjectPanel.SetActive(true);
    }
    /// <summary>
    /// Öffnet Fenster zum Laden bestehender Projekte.
    /// </summary>
    public void OpenProject()
    {
        OpenProjectPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Öffnet Fenster zum Bearbeiten der Einstellungen.
    /// </summary>
    public void Settings()
    {
        SettingsPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Schließt die Anwendung.
    /// </summary>
    public void Exit()
    {
        Debug.Log("Application.Quit() called");
        Application.Quit();
    }
}
