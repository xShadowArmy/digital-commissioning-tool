using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ApplicationFacade.Application;

public class SaveProject : MonoBehaviour
{
    public Button SaveProjectButton;
    void Start()
    {
        SaveProjectButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        GameManager.SaveProject(GameManager.OpenProjectData.ProjectName);
    }
}
