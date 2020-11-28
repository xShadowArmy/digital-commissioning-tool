using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProjectMenu : MonoBehaviour
{
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
        gameObject.SetActive(false);
        gameObject.transform.parent.Find("MainPanel").gameObject.SetActive(false);
        gameObject.transform.parent.Find("MainPanel").GetComponent<MainMenu>().LoadDefaultScene();
    }
}
