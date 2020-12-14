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
    public void LoadDefaultScene()
    {
        if (!SceneManager.GetSceneByName("WarehouseWithMOSIM").isLoaded)
        {
            SceneManager.LoadScene("WarehouseWithMOSIM", LoadSceneMode.Additive);
            GameObject.Find("Background").SetActive(false);
        }
        GameObject[] gameObjects = SceneManager.GetSceneByName("MainMenu").GetRootGameObjects();
        foreach (GameObject g in gameObjects)
        {
            if (g.name.Equals("Canvas"))
            {
                g.SetActive(false);
            }
        }
    }
}
