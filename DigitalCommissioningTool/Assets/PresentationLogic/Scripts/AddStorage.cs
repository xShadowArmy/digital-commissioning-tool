using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ApplicationFacade.Application;

public class AddStorage : MonoBehaviour
{
    Button addStorageButton;
    void Update()
    {

        //Button soll nur interagierbar sein wenn im editormodus ist:
        ModeHandler modeHandler = GameObject.Find("SwitchModeButton").GetComponent<ModeHandler>();              //Durch Modehandler herausfinden ob im Editormodus oder nicht
        if (modeHandler.Mode.Equals("EditorMode"))
        {
            addStorageButton.interactable = true;                                                               //wenn ja => Button = interagierbar ansonstem nicht
        }
        else
        {
            addStorageButton.interactable = false;
        }

    }

    //Wird ausgeführt wenn Button geklickt:
    void TaskOnClick()
    {
        GameManager.GameWarehouse.CreateStorageRack();              //Erstellt Regal
    }

    void Start()
    {
        //Button zugewisen, auf nicht interagierbar gesetzt, was onClick aufgerufen wird festgelegt:
        addStorageButton = GetComponent<Button>();                      
        addStorageButton.interactable = false;                          
        addStorageButton.onClick.AddListener(TaskOnClick);              
    }

}
