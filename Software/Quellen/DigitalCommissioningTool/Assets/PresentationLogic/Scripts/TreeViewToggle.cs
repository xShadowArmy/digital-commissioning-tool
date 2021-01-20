using ApplicationFacade.Application;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreeViewToggle : MonoBehaviour
{
    private GameObject TreeViewCanvas;
    private GameObject MainMenuCanvas;
    // Start is called before the first frame update
    void Start()
    {
        SearchCanvas();
    }
    void SearchCanvas()
    {
        GameObject[] gameObjects;
        if (SceneManager.GetSceneByName("MainMenu").isLoaded)
        {
            gameObjects = SceneManager.GetSceneByName("MainMenu").GetRootGameObjects();
            foreach (GameObject g in gameObjects)
            {
                if (g.name.Equals("Canvas"))
                {
                    MainMenuCanvas = g;
                }
                if (g.name.Equals("Main Camera"))
                {
                    g.SetActive(false);
                }
            }
            gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject g in gameObjects)
            {
                if (g.name.Equals("CanvasTreeView"))
                {
                    TreeViewCanvas = g;
                    TreeViewCanvas.SetActive(!MainMenuCanvas.activeSelf);
                }
            }
        }
    }
    private IEnumerator LoadMenu()
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone)
            yield return null;
        yield return new WaitForEndOfFrame();
        SearchCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyManager.ToogleMenu.Code) && GameManager.OpenProjectData != null)
        {
            if (!SceneManager.GetSceneByName("MainMenu").isLoaded)
            {
                StartCoroutine("LoadMenu");
            }
            if (TreeViewCanvas != null && MainMenuCanvas != null)
            {
                MainMenuCanvas.SetActive(!MainMenuCanvas.activeSelf);
                TreeViewCanvas.SetActive(!MainMenuCanvas.activeSelf);                
            }
        }
    }
}
