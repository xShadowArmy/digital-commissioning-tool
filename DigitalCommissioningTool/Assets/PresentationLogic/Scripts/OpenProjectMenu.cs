using ApplicationFacade;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemFacade;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;

public class OpenProjectMenu : MonoBehaviour
{
    public GameObject template;
    public GameObject projectName;
    public GameObject projectPath;
    public GameObject projectCreated;
    public GameObject projectModified;
    private List<string> projects = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        loadProjects();
    }

    void loadProjects()
    {
        //Manually add prebuild example scene
        projectName.GetComponent<TextMeshProUGUI>().text = "Example scene";
        projects.Add("Example scene");
        GameObject exampleScene = Instantiate(template);
        exampleScene.transform.SetParent(template.transform.parent);
        exampleScene.SetActive(true);

        string[] paths = Directory.GetFiles(Paths.ProjectsPath, "*.prj");
        int i = 0;
        foreach (string path in paths)
        {
            string fileName = Path.GetFileName(path).TrimEnd(".prj".ToCharArray());
            projects.Add(Path.GetFileName(path).TrimEnd(".prj".ToCharArray()));
            GameManager.LoadProject(Path.GetFileName(path).TrimEnd(".prj".ToCharArray()));
            projectName.GetComponent<TextMeshProUGUI>().text = GameManager.OpenProjectData.ProjectName;
            projectPath.GetComponent<TextMeshProUGUI>().text = GameManager.OpenProjectData.ProjectPath;
            projectCreated.GetComponent<TextMeshProUGUI>().text = GameManager.OpenProjectData.DateCreated.ToString("dd/MM/yyyy");
            projectModified.GetComponent<TextMeshProUGUI>().text = GameManager.OpenProjectData.DateModified.ToString("dd/MM/yyyy");
            GameObject item = Instantiate(template);
            item.transform.SetParent(template.transform.parent);
            item.SetActive(true);
            GameManager.CloseProject();
        }
        SceneManager.UnloadSceneAsync("DefaultWarehouse");
        SceneManager.LoadScene("DefaultWarehouse", LoadSceneMode.Additive);
        float newHeight = System.Math.Max(420, paths.Length * 105);
        RectTransform contentBox = template.transform.parent.GetComponent<RectTransform>();
        contentBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
        ResourceHandler.ReplaceResources();
    }
    public void Back()
    {
        gameObject.transform.parent.Find("MainPanel").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void OnClick(GameObject sender)
    {
        int index = sender.transform.GetSiblingIndex() - 1;
        //prebuild example scene
        if (index == 0)
        {
            if (!SceneManager.GetSceneByName("WarehouseWithMOSIM").isLoaded)
            {
                SceneManager.LoadScene("WarehouseWithMOSIM", LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync("DefaultWarehouse");
            }
        }
        else
        {
            //GameManager.CloseProject();
            //SceneManager.UnloadSceneAsync("DefaultWarehouse");
            //SceneManager.LoadScene("DefaultWarehouse", LoadSceneMode.Additive);
            GameManager.LoadProject(projects[index]);
        }
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
}
