using ApplicationFacade;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemFacade;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;
using ApplicationFacade.Application;
using UnityEngine.UI;

public class OpenProjectMenu : MonoBehaviour
{
    public GameObject template;
    public GameObject projectName;
    public GameObject projectPath;
    public GameObject projectCreated;
    public GameObject projectModified;
    private static List<string> projects = new List<string>();
    private int QueuedScene = -1;
    private int Frames = 0;
    private Image selectedBackgroundImage = null;
    private Color selectedColor;

    // Start is called before the first frame update
    void Start()
    {
        UpdateProjects();
        //SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
    public void Back()
    {
        gameObject.transform.parent.Find("MainPanel").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    private void UnloadScenes()
    {
        if (SceneManager.GetSceneByName("WarehouseWithMOSIM").isLoaded)
        {
            SceneManager.UnloadSceneAsync("WarehouseWithMOSIM");
        }
        if (SceneManager.GetSceneByName("DefaultWarehouse").isLoaded)
        {
            SceneManager.UnloadSceneAsync("DefaultWarehouse");
        }
    }
    public void OnClick(GameObject sender)
    {
        if (selectedBackgroundImage != null)
        {
            selectedBackgroundImage.color = new Color(0.960784f,0.960784f,0.960784f,1);
        }
        int index = sender.transform.GetSiblingIndex() - 1;
        UnloadScenes();

        GameManager.CloseProject();
        QueuedScene = index;
        SceneManager.LoadScene("DefaultWarehouse", LoadSceneMode.Additive);

        GameObject[] gameObjects = SceneManager.GetSceneByName("MainMenu").GetRootGameObjects();
        foreach (GameObject g in gameObjects)
        {
            if (g.name.Equals("Canvas"))
            {
                g.transform.Find("Background").gameObject.SetActive(false);
                g.SetActive(false);
            }
        }
    }

    public void OnClickDelete(Transform sender)
    {
        string projectName = sender.Find("Name/Text").GetComponent<TextMeshProUGUI>().text;
        GameManager.DeleteProject(projectName);
        projects.Remove(projectName);
        Destroy(sender.gameObject);
        UpdateProjects();
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name.Equals("DefaultWarehouse") && QueuedScene != -1)
        {
            GameManager.LoadProject(projects[QueuedScene]);
            QueuedScene = -1;
        }
    }

    void Update()
    {
        Frames += 1;

        if (Frames == 120)
        {
            UpdateProjects();

            Frames = 0;
        }
    }

    public void UpdateProjects()
    {
        if (ProjectManager.ProjectList != null && ProjectManager.ProjectList.Length != projects.Count)
        {
            foreach (ProjectData data in ProjectManager.ProjectList)
            {
                if (!projects.Contains(data.ProjectName))
                {
                    projects.Add(data.ProjectName);
                    projectName.GetComponent<TextMeshProUGUI>().text = data.ProjectName;
                    projectPath.GetComponent<TextMeshProUGUI>().text = data.ProjectPath;
                    projectCreated.GetComponent<TextMeshProUGUI>().text = data.DateCreated.ToString("dd/MM/yyyy");
                    projectModified.GetComponent<TextMeshProUGUI>().text = data.DateModified.ToString("dd/MM/yyyy");
                    GameObject item = Instantiate(template);
                    item.transform.SetParent(template.transform.parent);
                    item.SetActive(true);

                    float newHeight = System.Math.Max(420, ProjectManager.ProjectList.Length * 105);
                    RectTransform contentBox = template.transform.parent.GetComponent<RectTransform>();
                    contentBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
                }
            }

            ResourceHandler.ReplaceResources();
        }
    }

    public void HighlightNewProject(string projectName)
    {
        selectedColor = Color.cyan;
        selectedColor.a = 0.4f;
        selectedBackgroundImage = template.transform.parent.GetChild(projects.IndexOf(projectName) + 1).Find("Item Background").GetComponent<Image>();
        selectedBackgroundImage.color = selectedColor;
    }
}
