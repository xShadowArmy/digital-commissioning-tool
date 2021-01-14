using ApplicationFacade;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ApplicationFacade.Application;

public class NewProjectMenu : MonoBehaviour
{

    public GameObject OpenProjectPanel;
    public GameObject InputField;

    private OpenProjectMenu openProjectMenu;
    
    void Start()
    {
        ResourceHandler.ReplaceResources();
        openProjectMenu = OpenProjectPanel.GetComponent<OpenProjectMenu>();
    }
    /// <summary>
    /// Event für Zurücktaste. Schließt das aktuelle Fenster.
    /// </summary>
    public void Back()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Liest Projektnamen aus und überprüft Gültigkeit. Erstellt Projekt wenn Gültig.
    /// </summary>
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
            if (string.IsNullOrEmpty(GameManager.PManager.ProjectName))
            {
                GameManager.CreateProject(input.text);
                input.text = "";
                gameObject.SetActive(false);
                gameObject.transform.parent.gameObject.SetActive(false);
                gameObject.transform.parent.Find("Background").gameObject.SetActive(false);
            }
            else
            {
                GameManager.CloseProject();
                GameManager.CreateProject(input.text);
                openProjectMenu.UpdateProjects();
                gameObject.SetActive(false);
                OpenProjectPanel.SetActive(true);
                ResourceHandler.ReplaceResources();
                openProjectMenu.HighlightNewProject(input.text);
            }
            
        }
    }
}
