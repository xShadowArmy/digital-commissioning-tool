using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;
    public LayerMask mask;
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

    public static event ObjectSelectedEventHandler InnerWallSelected;
    
    public static event ObjectSelectedEventHandler AttachedInnerWallSelected;

    public Transform SelectedObject { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("GameController");
        popUp = controller.GetComponent<WallEditor>();
        EditorModeCamera = GameObject.FindGameObjectWithTag("EditorModeCamera").GetComponent<Camera>();
        SwitchModeButton = GameObject.Find("SwitchModeButton");
        ModeHandler = SwitchModeButton.GetComponent<ModeHandler>();
        Physics.autoSyncTransforms = true;
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
                    if (tempObject.CompareTag("SelectableWall") || tempObject.CompareTag("SelectableWindow") || tempObject.CompareTag("SelectableDoor") || tempObject.CompareTag("LeftWallRim") ||
                        tempObject.CompareTag("RightWallRim") || tempObject.CompareTag("SelectableInnerWall") || tempObject.CompareTag("SelectableAttachedInnerWall") ||
                        tempObject.CompareTag("SelectableStorage") || FindParentWithTag(ref tempObject, "SelectableStorage") != null
                    )
                    {
                        if (SelectedObject != null)
                        {
                            SelectedObject.Find("InvisibleWall").transform.GetComponent<Renderer>().material = defaultMaterial;
                        }

                        SelectedObject = tempObject;
                        Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                        invisibleWall.GetComponent<Renderer>().material = selectedMaterial;
                        selected = true;
                        
                        switch (tempObject.tag)
                        {
                            case "SelectableWall":
                                OnWallSelected(SelectedObject);
                                break;
                            case "SelectableWindow":
                                OnWallSelected(SelectedObject);
                                break;
                            case "SelectableDoor":
                                OnWallSelected(SelectedObject);
                                break;
                            case "LeftWallRim":
                                OnLeftWallRimSelected(SelectedObject);
                                break;
                            case "RightWallRim":
                                OnRightWallRimSelected(SelectedObject);
                                break;
                            case "SelectableInnerWall":
                                OnInnerWallSelected(SelectedObject);
                                break;
                            case "SelectableAttachedInnerWall":
                                OnAttachedInnerWallSelected(SelectedObject);
                                break;
                            case "SelectableStorage":
                                OnStorageSelected(SelectedObject);
                                break;
                        }
                        
                        SetPopUps();
                    }
                    else if (SelectedObject != null)
                    {
                        Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                        invisibleWall.GetComponent<Renderer>().material = defaultMaterial;
                        selected = false;
                        SetPopUps();
                    }
                }
                else if (Input.GetMouseButtonUp(0) && SelectedObject != null)
                {
                    Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                    invisibleWall.GetComponent<Renderer>().material = defaultMaterial;
                    selected = false;
                    SetPopUps();
                }
            }
        }
    }

    private void SetPopUps()
    {
        popUp.SetPopUp();
        popUp.SetPopUpScaleWall();
    }

    public void ResetSelection()
    {
        if (SelectedObject != null)
        {
            SelectedObject.Find("InvisibleWall").transform.GetComponent<Renderer>().material = defaultMaterial;
            selected = false;
            SelectedObject = null;
        }
    }

    private Transform FindParentWithTag(ref Transform child, string searchTag)
    {
        Transform temp = child;
        while (temp.parent != null)
        {
            if (temp.parent.CompareTag(searchTag))
            {
                child = temp.parent;
                return child;
            }
            temp = temp.parent;
        }
        return null;
    }
    
    private void OnInnerWallSelected(Transform selectedObject)
    {
        InnerWallSelected?.Invoke(SelectedObject);
    }
    
    private void OnAttachedInnerWallSelected(Transform selectedObject)
    {
        AttachedInnerWallSelected?.Invoke(SelectedObject);
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