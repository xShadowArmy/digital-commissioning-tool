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
    public Button Closed;
    [SerializeField] private TMP_Text saveProjectText;
    [SerializeField] private TMP_Text successfullySavedText;
    public SelectionManager selectionManager;
    public GameObject popUp;
    void Start()
    {
        SaveProjectButton.onClick.AddListener(TaskOnClick);
        saveProjectText.text = StringResourceManager.LoadString("@Save");
        popUp.SetActive(selectionManager.selected);
    }

    void TaskOnClick()
    {
        GameManager.SaveProject(GameManager.OpenProjectData.ProjectName);
        print("Project '" + GameManager.OpenProjectData.ProjectName + "' was saved");
        popUp.SetActive(true);
        successfullySavedText.text = StringResourceManager.LoadString("@SuccessfullySavedText");
    }
    /// <summary>
    /// Entfernt das UI Popup
    /// </summary>
    /// <param name="popUp"> Das Popup das geschlossen werden soll</param>
    public void close(GameObject popUp)
    {
        popUp.SetActive(false);
    }
}
