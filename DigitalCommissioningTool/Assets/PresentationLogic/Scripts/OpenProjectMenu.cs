using ApplicationFacade;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemFacade;
using UnityEngine;

public class OpenProjectMenu : MonoBehaviour
{
    public GameObject template;
    public GameObject projectName;
    public GameObject projectPath;
    public GameObject projectCreated;
    public GameObject projectModified;

    // Start is called before the first frame update
    void Start()
    {
        loadProjects();
    }

    void loadProjects() 
    {
        string[] paths = Directory.GetFiles(Paths.ProjectsPath, "*.prj");
        int i = 0;
        foreach (string path in paths)
        {
            GameManager.OpenProject(Path.GetFileName(path).TrimEnd(".prj".ToCharArray()));
            projectName.GetComponent<UnityEngine.UI.Text>().text = "Sample Name "+ ++i;
            projectPath.GetComponent<UnityEngine.UI.Text>().text = "C:/Sample/Path";
            projectCreated.GetComponent<UnityEngine.UI.Text>().text = "01.01.2020";
            projectModified.GetComponent<UnityEngine.UI.Text>().text = "02.02.2020";
            GameObject item = Instantiate(template);
            item.transform.SetParent(template.transform.parent);
            item.SetActive(true);
        }
        
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
}
