using ApplicationFacade;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ApplicationFacade.Application;

public class NewProjectMenu : MonoBehaviour
{
    public GameObject InputField;
    // Start is called before the first frame update
    void Start()
    {
        ResourceHandler.ReplaceResources();
    }
    public void Back()
    {
        gameObject.SetActive(false);
    }
    public void Continue()
    {
        string input = InputField.GetComponent<TMP_InputField>().text;
        foreach (ProjectData data in ProjectManager.ProjectList)
        {
            if (data.ProjectName.Equals(input))
            {
                Debug.Log("Projekt mit diesem Namen existiert bereits");
                input = "";
            }
        }
        if (!input.Equals(""))
        {
            GameManager.CreateProject(input);
        }        
        gameObject.SetActive(false);
        gameObject.transform.parent.gameObject.SetActive(false);
        gameObject.transform.parent.Find("Background").gameObject.SetActive(false);
    }
}
