using System.Collections;
using System.Collections.Generic;
using SystemFacade;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        ReplaceResources();
    }
    void ReplaceResources()
    {
        GameObject[] labels = GameObject.FindGameObjectsWithTag("Resource");

        foreach (GameObject label in labels)
        {
            string key = label.GetComponent<UnityEngine.UI.Text>().text.TrimStart('<').TrimEnd('>');
            string localizedText = StringResourceManager.LoadString("@" + key);
            label.GetComponent<UnityEngine.UI.Text>().text = localizedText;
        }
    }
    public void NewProject()
    {
        LoadDefaultScene();
    }
    public void OpenProject()
    {
        LoadDefaultScene();
    }
    public void Settings()
    {
        gameObject.transform.parent.Find("SettingsPanel").gameObject.SetActive(true);
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
