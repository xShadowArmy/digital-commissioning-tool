using System.Collections;
using System.Collections.Generic;
using SystemFacade;
using UnityEngine;
using UnityEngine.UI;
using ApplicationFacade.Application;
using TMPro;

public class SaveProject : MonoBehaviour
{
    public Button SaveProjectButton;
    [SerializeField] private TMP_Text saveProjectText;
    void Start()
    {
        SaveProjectButton.onClick.AddListener(TaskOnClick);
        saveProjectText.text = StringResourceManager.LoadString("@Save");
    }

    void TaskOnClick()
    {
        GameManager.SaveProject(GameManager.OpenProjectData.ProjectName);
        print("Project '" + GameManager.OpenProjectData.ProjectName + "' was saved");
    }
}
