﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;

    [HideInInspector] public bool selected = false;
    private GameObject controller;
    private WallEditor popUp;
    private Camera EditorModeCamera;
    private GameObject SwitchModeButton;
    private ModeHandler ModeHandler;

    public delegate void ObjectSelectedEventHandler(Transform selectedObject);

    public static event ObjectSelectedEventHandler WallSelected;

    public static event ObjectSelectedEventHandler StorageSelected;
    
    public static event ObjectSelectedEventHandler LeftWallRimSelected;
    
    public static event ObjectSelectedEventHandler RightWallRimSelected;
    
    

    public Transform SelectedObject { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("GameController");
        popUp = controller.GetComponent<WallEditor>();
        EditorModeCamera = GameObject.FindGameObjectWithTag("EditorModeCamera").GetComponent<Camera>();
        SwitchModeButton = GameObject.Find("SwitchModeButton");
        ModeHandler = SwitchModeButton.GetComponent<ModeHandler>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (ModeHandler.Mode.Equals("EditorMode"))
        {
            Ray ray = EditorModeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit))
                {
                    Transform tempObject = hit.transform;
                    if (tempObject.CompareTag("SelectableWall") || tempObject.CompareTag("SelectableWindow") || tempObject.CompareTag("SelectableDoor"))
                    {
                        if (SelectedObject != null)
                        {
                            SelectedObject.Find("InvisibleWall").transform.GetComponent<Renderer>().material =
                                defaultMaterial;
                            selected = false;
                            popUp.SetPopUpScaleWall();
                            popUp.SetPopUp();
                        }

                        SelectedObject = tempObject;
                        Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                        invisibleWall.GetComponent<Renderer>().material = selectedMaterial;
                        selected = true;
                        OnWallSelected(SelectedObject);
                        popUp.SetPopUp();
                    }
                    else if (tempObject.CompareTag("LeftWallRim"))
                    {
                        if (SelectedObject != null)
                        {
                            SelectedObject.Find("InvisibleWall").transform.GetComponent<Renderer>().material =
                                defaultMaterial;
                            selected = false;
                            popUp.SetPopUp();
                        }
                        
                        SelectedObject = tempObject;
                        Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                        invisibleWall.GetComponent<Renderer>().material = selectedMaterial;
                        selected = true;
                        OnLeftWallRimSelected(SelectedObject);
                    }
                    else if (tempObject.CompareTag("RightWallRim"))
                    {
                        if (SelectedObject != null)
                        {
                            SelectedObject.Find("InvisibleWall").transform.GetComponent<Renderer>().material =
                                defaultMaterial;
                            selected = false;
                            popUp.SetPopUp();
                        }
                        SelectedObject = tempObject;
                        Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                        invisibleWall.GetComponent<Renderer>().material = selectedMaterial;
                        selected = true;
                        OnRightWallRimSelected(SelectedObject);
                    }
                    else if (tempObject.CompareTag("SelectableStorage"))
                    {
                        if (SelectedObject != null)
                        {
                            SelectedObject.Find("InvisibleWall").transform.GetComponent<Renderer>().material =
                                defaultMaterial;
                            selected = false;
                        }
                        SelectedObject = tempObject;
                        Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                        invisibleWall.GetComponent<Renderer>().material = selectedMaterial;
                        selected = true;
                        OnStorageSelected(SelectedObject);
                    }
                    else if (tempObject.parent != null && tempObject.parent.CompareTag("SelectableStorage"))
                    {
                        if (SelectedObject != null)
                        {
                            SelectedObject.Find("InvisibleWall").transform.GetComponent<Renderer>().material =
                                defaultMaterial;
                            selected = false;
                        }
                        SelectedObject = tempObject.parent;
                        Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                        invisibleWall.GetComponent<Renderer>().material = selectedMaterial;
                        selected = true;
                        OnStorageSelected(SelectedObject);
                    }
                    else if (SelectedObject != null)
                    {
                        Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                        invisibleWall.GetComponent<Renderer>().material = defaultMaterial;
                        selected = false;
                        popUp.SetPopUp();
                        popUp.SetPopUpScaleWall();
                    }
                }
                else if (Input.GetMouseButtonUp(0) && SelectedObject != null)
                {
                    Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                    invisibleWall.GetComponent<Renderer>().material = defaultMaterial;
                    selected = false;
                    popUp.SetPopUp();
                    popUp.SetPopUpScaleWall();
                }
            }
        }
    }

    protected virtual void OnStorageSelected(Transform selectedObject)
    {
        StorageSelected?.Invoke(SelectedObject);
    }
    
    protected virtual void OnWallSelected(Transform selectedObject)
    {
        WallSelected?.Invoke(SelectedObject);
    }
    
    protected virtual void OnLeftWallRimSelected(Transform selectedObject)
    {
        LeftWallRimSelected?.Invoke(SelectedObject);
    }
    
    protected virtual void OnRightWallRimSelected(Transform selectedObject)
    {
        RightWallRimSelected?.Invoke(SelectedObject);
    }
    
    
    
    
}