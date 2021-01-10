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
        TMP_InputField input = InputField.GetComponent<TMP_InputField>();

        if ( ProjectManager.ProjectList != null )
        {
            foreach (ProjectData data in ProjectManager.ProjectList)
            {
                if (data.ProjectName.Equals(input.text))
                {
                    Debug.Log("Projekt mit diesem Namen existiert bereits");
                    input.text = "";
                }
            }
        }

        if (!input.text.Equals(""))
        {
            GameManager.CreateProject(input.text);
            input.text = "";
            gameObject.SetActive(false);
            gameObject.transform.parent.gameObject.SetActive(false);
            gameObject.transform.parent.Find("Background").gameObject.SetActive(false);
        }
    }
}
