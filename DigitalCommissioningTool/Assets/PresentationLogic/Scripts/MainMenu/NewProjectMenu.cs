using ApplicationFacade;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        GameManager.CreateProject(InputField.GetComponent<TMP_InputField>().text);  
        gameObject.SetActive(false);
        gameObject.transform.parent.Find("MainPanel").gameObject.SetActive(false);
        gameObject.transform.parent.Find("Background").gameObject.SetActive(false);
    }
}
