using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ApplicationFacade;

public class AddStorage : MonoBehaviour
{

    // public UnityEngine.UI.Button button;
    Button addStorageButton;
    //public static int objectNumber = 1;

    void Update()
    {

        //To Ask in which mode modeHandler has value 
        //(Because modehandler is in different GameObject first find object with script then get whatever is need)
        ModeHandler modeHandler = GameObject.Find("SwitchModeButton").GetComponent<ModeHandler>();

        // Button shall be interactable only when in Editor Mode  
        if (modeHandler.Mode.Equals("EditorMode"))
        {
            addStorageButton.interactable = true;
        }
        else
        {
            addStorageButton.interactable = false;
        }

    }

    //create a new storage  (method in different script this one just gets it)
    void TaskOnClick()
    {
        // objectNumber++;
        //if (objectNumber==5) { objectNumber = 1; }
        //Debug.Log("You have clicked the button!");
        GameManager.GameWarehouse.CreateStorageRack();


    }

    void Start()
    {
        //initialize button as addStorageButton
        addStorageButton = GetComponent<Button>();
        //start = not editormode therefore not interactable at start
        addStorageButton.interactable = false;
        // addStorageButton.gameObject.SetActive(false);
        //do taskOnClick method when clicked
        addStorageButton.onClick.AddListener(TaskOnClick);
    }

}
