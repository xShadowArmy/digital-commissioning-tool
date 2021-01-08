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
    // Start is called before the first frame update
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
    
    public void NewProject()
    {
        NewProjectPanel.SetActive(true);
    }
    public void OpenProject()
    {
        OpenProjectPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Settings()
    {
        SettingsPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Exit()
    {
        Debug.Log("Application.Quit() called");
        Application.Quit();
    }
}
