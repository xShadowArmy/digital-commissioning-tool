using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Delete : MonoBehaviour
{

    // public UnityEngine.UI.Button button;
    Button deleteButton;
    GameObject obj;



    void Update()
    {
        PickAndPlaceNew pick = GameObject.Find("MoveableStorageRack").GetComponent<PickAndPlaceNew>();
        PickAndPlaceNew pick2 = GameObject.Find("StorageRack").GetComponent<PickAndPlaceNew>();

        bool x = pick.isDragging;

        Debug.Log(x);

        if (pick.isDragging || pick2.isDragging) {
            deleteButton.gameObject.SetActive(true);
            if (pick.isDragging)
            {
                obj = pick.selected;
            }
            else {
                obj = pick2.selected;
            }
        }
        if (!pick.isDragging || pick2.isDragging) {
            deleteButton.gameObject.SetActive(false);
        }
    }

    //create a new storage  (method in different script this one just gets it)
    void TaskOnClick()
    {
        Destroy(obj);
    }

    void Start()
    {
        deleteButton = GetComponent<Button>();
        //not visible at start (inactive)
        deleteButton.gameObject.SetActive(false);
        //do taskOnClick method when clicked
        deleteButton.onClick.AddListener(TaskOnClick);

    }


}
