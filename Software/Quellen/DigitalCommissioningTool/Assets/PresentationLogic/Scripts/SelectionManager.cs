using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectionManager : MonoBehaviour
{

    public LayerMask mask;
    [HideInInspector] public bool selected = false;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;
    private GameObject controller;
    private WallEditor popUp;
    private Camera EditorModeCamera;
    private GameObject SwitchModeButton;
    private ModeHandler ModeHandler;
    private int pointerID = -1;

    public delegate void ShelveSelectedEventHandler(GameObject selectedObject, bool activs);

    public event ShelveSelectedEventHandler ShelveSelected;

    public delegate void ObjectSelectedEventHandler(Transform selectedObject);

    public static event ObjectSelectedEventHandler WallSelected;

    public static event ObjectSelectedEventHandler StorageSelected;
    
    public static event ObjectSelectedEventHandler MovableStorageSelected;

    public static event ObjectSelectedEventHandler LeftWallRimSelected;

    public static event ObjectSelectedEventHandler RightWallRimSelected;

    public static event ObjectSelectedEventHandler InnerWallSelected;

    public static event ObjectSelectedEventHandler AttachedInnerWallSelected;

    public Transform SelectedObject { get; private set; }

    /// <summary>
    /// Setzt vor Ausführung alle Selected Variablen auf null. 
    /// </summary>
    private void Awake()
    {
        WallSelected = null;
        StorageSelected = null;
        MovableStorageSelected = null;
        LeftWallRimSelected = null;
        RightWallRimSelected = null;
        InnerWallSelected = null;
        AttachedInnerWallSelected = null;
    }

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


    /// <summary>
    /// Prüft anhand eines Raycasts ob ein auswählbares Objekt angeklickt wurde und löst in diesem Fall das entsprechende Event aus.
    /// </summary>
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
                        tempObject.CompareTag("SelectableStorage") || tempObject.CompareTag( "SelectableContainer" ) || FindParentWithTag(ref tempObject, "SelectableStorage") != null ||
                        FindParentWithTag( ref tempObject, "SelectableContainer" ) != null
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
                                OnStorageSelected( null );
                                OnInnerWallSelected(SelectedObject);
                                break;
                            case "SelectableAttachedInnerWall":
                                OnAttachedInnerWallSelected(SelectedObject);
                                OnStorageSelected( null );
                                break;
                            case "SelectableStorage":
                                OnStorageSelected(SelectedObject);
                                break;
                            case "SelectableContainer":
                                OnStorageSelected( null );
                                OnMovableStorageSelected(SelectedObject);
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
        else if (ModeHandler.Mode.Equals("MosimMode"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (EventSystem.current.IsPointerOverGameObject(pointerID) == false)                    //checkt, ob Maus über Gui Element ist
                {
                    bool rayhit = Physics.Raycast(ray, out hit, 90.0f);
            
                    if ( rayhit && hit.transform != null )
                    {
                        Transform temp = hit.transform;
                        if ( hit.transform.tag.Contains("StorageBox") || hit.transform.CompareTag( "StorageContainer") )   //normaler physics ray cast, kann aber nicht UI elemente erkennen
                        {
                            GameObject g1 = hit.transform.gameObject;
                            OnShelveSelected(g1, true);
                        }
                        else if(temp.CompareTag("SelectableStorage") || FindParentWithTag(ref temp, "SelectableStorage") != null)
                        {
                            OnStorageSelected(temp);
                        }
                        else
                        {
                            GameObject g1 = null;
                            OnShelveSelected(g1, false);
                        }
                    }
                }

            }


        }
    }

    /// <summary>
    /// Aktiviert bzw. deaktiviert die UI Popups
    /// </summary>
    private void SetPopUps()
    {
        popUp.SetPopUp();
        popUp.SetPopUpScaleWall();
    }

    /// <summary>
    /// Entfernt die Auswahl des Objekts und setzt das Material zurück
    /// </summary>
    public void ResetSelection()
    {
        if (SelectedObject != null)
        {
            SelectedObject.Find("InvisibleWall").transform.GetComponent<Renderer>().material = defaultMaterial;
            selected = false;
            SelectedObject = null;
        }
    }

    /// <summary>
    /// Sucht in allen Elternelementen ein GameObject mit dem entsprechenden Tag und gibt dessen Transform zurück 
    /// </summary>
    /// <param name="child">
    ///    Objekt dessen Elternelemente durchsucht werden sollen
    /// </param>
    /// <param name="searchTag">
    ///    Tag nach dem gesucht werden soll
    /// </param>
    /// <returns></returns>
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
    /// <summary>
    /// Wird ausgelöst, wenn ein innere Wand ausgewählt wird
    /// </summary>
    /// <param name="selectedObject">Referenz der ausgewählten inneren Wand</param>
    private void OnInnerWallSelected(Transform selectedObject)
    {
        InnerWallSelected?.Invoke(SelectedObject);
    }
    /// <summary>
    /// Wird ausgelöst, wenn eine innere Wand, die an eine äußere angebracht ist ausgewählt wird
    /// </summary>
    /// <param name="selectedObject">Referenz der ausgewählten inneren Wand</param>
    private void OnAttachedInnerWallSelected(Transform selectedObject)
    {
        AttachedInnerWallSelected?.Invoke(SelectedObject);
    }
    /// <summary>
    /// Wird ausgelöst, wenn ein Regal ausgewählt wird
    /// </summary>
    /// <param name="selectedObject">Referenz der ausgewählte Regal</param>
    protected virtual void OnStorageSelected(Transform selectedObject)
    {
        StorageSelected?.Invoke(selectedObject);
    }
    /// <summary>
    /// Wird ausgelöst, wenn ein bewegbares Regal ausgewählt wird
    /// </summary>
    /// <param name="selectedObject">Referenz der ausgewählte bewegbare Regal</param>
    protected virtual void OnMovableStorageSelected(Transform selectedObject)
    {
        MovableStorageSelected?.Invoke(SelectedObject);
    }
    /// <summary>
    /// Wird ausgelöst, wenn ein Wandendstück ausgewählt wird
    /// </summary>
    /// <param name="selectedObject">Referenz der ausgewählten Wand</param>
    protected virtual void OnWallSelected(Transform selectedObject)
    {
        WallSelected?.Invoke(SelectedObject);
    }
    /// <summary>
    /// Wird ausgelöst, wenn linkes Wandendstück ausgewählt wird
    /// </summary>
    /// <param name="selectedObject">Referenz der ausgewählten Wand</param>
    protected virtual void OnLeftWallRimSelected(Transform selectedObject)
    {
        LeftWallRimSelected?.Invoke(SelectedObject);
    }
    /// <summary>
    /// Wird ausgelöst, wenn rechtes Wandendstück ausgewählt wird
    /// </summary>
    /// <param name="selectedObject">Referenz der ausgewählten Wand</param>
    protected virtual void OnRightWallRimSelected(Transform selectedObject)
    {
        RightWallRimSelected?.Invoke(SelectedObject);
    }
    /// <summary>
    /// Wird aufgerufen, wenn ein Kiste angeklickt wird
    /// </summary>
    /// <param name="source">Objektreferenz der ausgewählten Kiste, oder Null-Referenz bei Deaktivierung</param>
    /// <param name="active">Gib an, ob Tooltip aktiviert oder deaktiviert werden soll </param>
    protected void OnShelveSelected(GameObject source, bool active)
    {

        if (Input.GetMouseButtonDown(0))
        {
            ShelveSelected?.Invoke(source, active);
        }
        
    }
}