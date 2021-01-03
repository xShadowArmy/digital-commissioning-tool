using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SystemFacade;
using ApplicationFacade;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WallEditor : MonoBehaviour
{
    public GameObject popUp;


    private int numberOfWalls;
    [SerializeField] private Text addWindowText;
    [SerializeField] private Text addDoorText;
    [SerializeField] private Text addWallText;
    [SerializeField] private Text addInnerWallText;
    [SerializeField] private SelectionManager selectionManager;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material transparentMaterial;


    [SerializeField] private InputField inputNumberOfWalls;
    [SerializeField] private GameObject popUpScaleWall;


    [SerializeField] private GameObject DoorPrefab;
    [SerializeField] private GameObject WallPrefab;
    [SerializeField] private GameObject WindowPrefab;

    [SerializeField] private Camera EditorModeCamera;
    [SerializeField] private GameObject ObjectSpawn;

    public LayerMask mask;
    private bool mouseButtonPressed = false;
    private Vector3 oldMousePos = new Vector3(0, 0, 0);
    private Transform SelectedObjectTransform;
    private bool innerWallSelected = false;
    public Text myText;
    private ModeHandler ModeHandler;
    private GameObject SwitchModeButton;
    private GameObject AddInnerWallButton;


    private List<Collider> objectsInRange = new List<Collider>();

    // Start is called before the first frame update

    void Start()
    {
        addWindowText.text = StringResourceManager.LoadString("@AddWindowText");
        addDoorText.text = StringResourceManager.LoadString("@AddDoorText");
        addWallText.text = StringResourceManager.LoadString("@AddWallText");
        addInnerWallText.text = StringResourceManager.LoadString("@AddInnerWallText");
        SelectedObjectTransform = selectionManager.SelectedObject;
        popUp.SetActive(selectionManager.selected);
        SelectionManager.WallSelected += OnWallSelected;
        SelectionManager.LeftWallRimSelected += OnLeftWallRimSelected;
        SelectionManager.RightWallRimSelected += OnRightWallRimSelected;
        SelectionManager.InnerWallSelected += OnInnerWallSelected;
        SelectionManager.AttachedInnerWallSelected += OnAttachedInnerWallSelected;
        Physics.autoSyncTransforms = true;

        AddInnerWallButton = GameObject.Find("AddInnerWallButton");
        SwitchModeButton = GameObject.Find("SwitchModeButton");
        ModeHandler = SwitchModeButton.GetComponent<ModeHandler>();
    }

    /// <summary>
    /// Sorgt dafür, dass Innenwände per Maus ausgewählt und verschoben/gedreht werden können. Während dem Bewegen wird nach einer Außenwand gesucht, an die die Wand angedockt werden kann.
    /// </summary>
    private void Update()
    {
        if (ModeHandler.Mode.Equals("EditorMode"))
        {
            AddInnerWallButton.SetActive(true);
        }
        else
        {
            AddInnerWallButton.SetActive(false);
        }

        SelectedObjectTransform = selectionManager.SelectedObject;
        if (innerWallSelected)
        {
            Ray ray = EditorModeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Drag
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                Vector3 mousePos = new Vector3(hit.point.x, SelectedObjectTransform.localScale.y / 2, hit.point.z);

                if (mousePos != oldMousePos)
                {
                    oldMousePos = mousePos;
                    SelectedObjectTransform.position = mousePos;
                }

                Collider[] colliders = Physics.OverlapBox(SelectedObjectTransform.position, SelectedObjectTransform.localScale / 2, SelectedObjectTransform.rotation);
                foreach (Collider collider1 in colliders)
                {
                    if (collider1.gameObject != SelectedObjectTransform.gameObject && collider1.transform.rotation != SelectedObjectTransform.rotation && collider1.CompareTag("SelectableWall"))
                    {
                        Transform invisibleWall = collider1.transform.Find("InvisibleWall").transform;
                        if (!objectsInRange.Contains(collider1))
                        {
                            objectsInRange.Add(collider1);
                            break;
                        }
                    }
                }

                foreach (Collider collider1 in objectsInRange.ToArray())
                {
                    if (!colliders.Contains(collider1))
                    {
                        Transform invisibleWall = collider1.transform.Find("InvisibleWall").transform;
                        invisibleWall.GetComponent<Renderer>().material = transparentMaterial;
                        objectsInRange.Remove(collider1);
                    }
                    else
                    {
                        Transform invisibleWall = collider1.transform.Find("InvisibleWall").transform;
                        invisibleWall.GetComponent<Renderer>().material = greenMaterial;
                    }
                }
            }

            //Place
            if (Input.GetKeyUp(KeyCode.Return))
            {
                //Snap to other Wall
                if (objectsInRange.Count > 0)
                {
                    Transform snapObject = objectsInRange[0].transform;
                    Vector3 direction = new Vector3();

                    Debug.Log(snapObject.name);
                    if (Math.Abs(SelectedObjectTransform.rotation.eulerAngles.y - 180.0f) < 1f || Math.Abs(SelectedObjectTransform.rotation.eulerAngles.y) < 1f)
                    {
                        if (snapObject.parent.CompareTag("NorthWall"))
                        {
                            direction = Vector3.right;
                        }
                        else if (snapObject.parent.CompareTag("SouthWall"))
                        {
                            direction = Vector3.left;
                        }

                        Debug.Log("0/180 " + direction);
                        SelectedObjectTransform.position = snapObject.position + Vector3.Scale(new Vector3(SelectedObjectTransform.localScale.z / 2.0f + SelectedObjectTransform.localScale.x / 2.0f, 0, 0), direction);
                    }
                    else
                    {
                        if (snapObject.parent.CompareTag("EastWall"))
                        {
                            direction = Vector3.back;
                        }
                        else if (snapObject.parent.CompareTag("WestWall"))
                        {
                            direction = Vector3.forward;
                        }

                        Debug.Log("270/90 " + direction);
                        SelectedObjectTransform.position = snapObject.position + Vector3.Scale(new Vector3(0, 0, SelectedObjectTransform.localScale.z / 2.0f + SelectedObjectTransform.localScale.x / 2.0f), direction);
                    }

                    SelectedObjectTransform.tag = "SelectableAttachedInnerWall";
                    SelectedObjectTransform.parent = snapObject.parent.Find("InnerWall");
                    foreach (Collider collider1 in objectsInRange)
                    {
                        Transform invisibleWall = collider1.transform.Find("InvisibleWall").transform;
                        invisibleWall.GetComponent<Renderer>().material = transparentMaterial;
                    }
                }

                innerWallSelected = false;
                selectionManager.ResetSelection();
            }

            //Rotate
            if (Input.GetKeyUp(KeyCode.R))
            {
                SelectedObjectTransform.Rotate(Vector3.up * 90);
            }
        }
    }


    private void OnInnerWallSelected(Transform selectedObject)
    {
        innerWallSelected = true;
    }

    private void OnAttachedInnerWallSelected(Transform selectedObject)
    {
        popUp.SetActive(false);
        popUpScaleWall.SetActive(true);
        innerWallSelected = false;
    }

    private void OnRightWallRimSelected(Transform selectedObject)
    {
        popUp.SetActive(false);
        popUpScaleWall.SetActive(true);
        innerWallSelected = false;
    }

    private void OnLeftWallRimSelected(Transform selectedObject)
    {
        popUp.SetActive(false);
        popUpScaleWall.SetActive(true);
        innerWallSelected = false;
    }

    private void OnWallSelected(Transform selectedObject)
    {
        popUp.SetActive(true);
        popUpScaleWall.SetActive(false);
        innerWallSelected = false;
    }

    /// <summary>
    /// Fügt Innenwand hinzu und passt den Tag entsprechend an.
    /// </summary>
    public void OnAddInnerWallButtonClicked()
    {
        Ray ray = EditorModeCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20))
        {
            GameObject temp = Instantiate(WallPrefab, hit.point + new Vector3(0, 1.6f, 0), ObjectSpawn.transform.rotation);
            temp.tag = "SelectableInnerWall";
        }
    }

    /// <summary>
    /// Ersetzt das ausgewählte Wandelement durch ein Fensterelement
    /// </summary>
    public void OnAddWindowClick()
    {
        SelectedObjectTransform = selectionManager.SelectedObject;
        if (!SelectedObjectTransform.gameObject.CompareTag("SelectableWindow"))
        {
            if (SelectedObjectTransform.gameObject.CompareTag("SelectableDoor"))
            {
                Transform parent = SelectedObjectTransform.parent;
                Destroy(SelectedObjectTransform.gameObject);
                Instantiate(WindowPrefab, SelectedObjectTransform.position, SelectedObjectTransform.rotation, parent);
            }
            else
            {
                Collider[] colliders = Physics.OverlapSphere(SelectedObjectTransform.position, SelectedObjectTransform.localScale.x + 0.1f);
                bool foundRightWallElement = false;
                bool foundLeftWallElement = false;
                GameObject rightWallElement = null;
                GameObject leftWallElement = null;
                Transform parent = SelectedObjectTransform.parent;

                if (colliders.Length > 1)
                {
                    foreach (Collider collider in colliders)
                    {
                        Transform colliderTransform = collider.gameObject.transform;

                        // Prüfen ob es dieselbe Wand ist
                        if (colliderTransform != SelectedObjectTransform && colliderTransform.rotation == SelectedObjectTransform.rotation && !collider.gameObject.CompareTag("SelectableDoor") && !collider.gameObject.CompareTag("SelectableWindow"))
                        {
                            Vector3 relativePoint = SelectedObjectTransform.InverseTransformPoint(colliderTransform.position);
                            if (relativePoint.x < 0.0 && !collider.gameObject.CompareTag("LeftWallRim") && !collider.gameObject.CompareTag("RightWallRim") && !collider.gameObject.CompareTag("SelectableAttachedInnerWall"))
                            {
                                foundRightWallElement = true;
                                rightWallElement = collider.gameObject;
                            }
                            else if (!collider.gameObject.CompareTag("LeftWallRim") && !collider.gameObject.CompareTag("RightWallRim"))
                            {
                                foundLeftWallElement = true;
                                leftWallElement = collider.gameObject;
                            }
                        }
                    }
                }

                if (foundRightWallElement)
                {
                    GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(rightWallElement));
                    GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(SelectedObjectTransform.gameObject));
                    GameManager.GameWarehouse.CreateWindow(SelectedObjectTransform.position + SelectedObjectTransform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2.0f)), SelectedObjectTransform.rotation,
                        SelectedObjectTransform.localScale);
                }
                else if (foundLeftWallElement)
                {
                    GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(leftWallElement));
                    GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(SelectedObjectTransform.gameObject));
                    GameManager.GameWarehouse.CreateWindow(leftWallElement.transform.position + leftWallElement.transform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2.0f)), SelectedObjectTransform.rotation,
                        SelectedObjectTransform.localScale);
                }
                else
                {
                    Debug.Log("Not enough Space for Door");
                }
            }
        }

        close(popUp);
    }

    /// <summary>
    /// Ersetzt das ausgewählte Wandelement durch ein Türelement
    /// </summary>
    public void OnAddDoorClick()
    {
        SelectedObjectTransform = selectionManager.SelectedObject;
        if (!SelectedObjectTransform.gameObject.CompareTag("SelectableDoor"))
        {
            if (SelectedObjectTransform.gameObject.CompareTag("SelectableWindow"))
            {
                Transform parent = SelectedObjectTransform.parent;
                Destroy(SelectedObjectTransform.gameObject);
                Instantiate(DoorPrefab, SelectedObjectTransform.position, SelectedObjectTransform.rotation, parent);
            }
            else
            {
                Collider[] colliders = Physics.OverlapSphere(SelectedObjectTransform.position, SelectedObjectTransform.localScale.x + 0.1f);
                bool foundRightWallElement = false;
                bool foundLeftWallElement = false;
                GameObject rightWallElement = null;
                GameObject leftWallElement = null;
                Transform parent = SelectedObjectTransform.parent;

                if (colliders.Length > 1)
                {
                    foreach (Collider collider in colliders)
                    {
                        Transform colliderTransform = collider.gameObject.transform;

                        // Prüfen ob es dieselbe Wand ist
                        if (colliderTransform != SelectedObjectTransform && colliderTransform.rotation == SelectedObjectTransform.rotation && !collider.gameObject.CompareTag("SelectableDoor") && !collider.gameObject.CompareTag("SelectableWindow"))
                        {
                            Vector3 relativePoint = SelectedObjectTransform.InverseTransformPoint(colliderTransform.position);
                            if (relativePoint.x < 0.0 && !collider.gameObject.CompareTag("LeftWallRim") && !collider.gameObject.CompareTag("RightWallRim") && !collider.gameObject.CompareTag("SelectableAttachedInnerWall"))
                            {
                                foundRightWallElement = true;
                                rightWallElement = collider.gameObject;
                            }
                            else if (!collider.gameObject.CompareTag("LeftWallRim") && !collider.gameObject.CompareTag("RightWallRim"))
                            {
                                foundLeftWallElement = true;
                                leftWallElement = collider.gameObject;
                            }
                        }
                    }
                }

                if (foundRightWallElement)
                {
                    GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(rightWallElement));
                    GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(SelectedObjectTransform.gameObject));
                    GameManager.GameWarehouse.CreateDoor(SelectedObjectTransform.position + SelectedObjectTransform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2.0f)), SelectedObjectTransform.rotation,
                        SelectedObjectTransform.localScale, DoorType.Door);
                }
                else if (foundLeftWallElement)
                {
                    GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(leftWallElement));
                    GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(SelectedObjectTransform.gameObject));
                    GameManager.GameWarehouse.CreateDoor(leftWallElement.transform.position + leftWallElement.transform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2.0f)), SelectedObjectTransform.rotation,
                        SelectedObjectTransform.localScale, DoorType.Door);
                }
                else
                {
                    Debug.Log("Not enough Space for Door");
                }
            }
        }

        close(popUp);
    }

    /// <summary>
    /// Ersetzt das ausgewählte Wandelement durch ein Wandelement
    /// </summary>
    public void OnAddWallClick()
    {
        SelectedObjectTransform = selectionManager.SelectedObject;
        if (!SelectedObjectTransform.gameObject.CompareTag("SelectableWall"))
        {
            Transform parent = SelectedObjectTransform.parent;
            if (SelectedObjectTransform.CompareTag("SelectableDoor") || SelectedObjectTransform.CompareTag("SelectableWindow"))
            {
                Vector3 position = SelectedObjectTransform.position;
                Vector3 localScale = SelectedObjectTransform.localScale;
                Quaternion rotation = SelectedObjectTransform.rotation;
                Debug.Log(SelectedObjectTransform.parent.tag);
                Debug.Log(SelectedObjectTransform.name);
                switch (SelectedObjectTransform.parent.tag)
                {
                    case "NorthWall":
                        Debug.Log("NorthWall");
                        GameManager.GameWarehouse.CreateWall(position - SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, SelectedObjectTransform.localScale, WallFace.North, WallClass.Outer);
                        GameManager.GameWarehouse.CreateWall(position + SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, SelectedObjectTransform.localScale, WallFace.North, WallClass.Outer);
                        break;

                    case "EasthWall":
                        Debug.Log("EastWall");
                        GameManager.GameWarehouse.CreateWall(position - SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, SelectedObjectTransform.localScale, WallFace.East, WallClass.Outer);
                        GameManager.GameWarehouse.CreateWall(position + SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, SelectedObjectTransform.localScale, WallFace.East, WallClass.Outer);
                        break;

                    case "SouthWall":
                        Debug.Log("SouthWall");
                        GameManager.GameWarehouse.CreateWall(position - SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, SelectedObjectTransform.localScale, WallFace.South, WallClass.Outer);
                        GameManager.GameWarehouse.CreateWall(position + SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, SelectedObjectTransform.localScale, WallFace.South, WallClass.Outer);
                        break;

                    case "WestWall":
                        Debug.Log("WestWall");
                        GameManager.GameWarehouse.CreateWall(position - SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, SelectedObjectTransform.localScale, WallFace.West, WallClass.Outer);
                        GameManager.GameWarehouse.CreateWall(position + SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, SelectedObjectTransform.localScale, WallFace.West, WallClass.Outer);
                        break;
                    default:
                        Debug.Log("DefCase");
                        break;
                }

                Destroy(SelectedObjectTransform.gameObject);
            }
        }

        close(popUp);
    }

    /// <summary>
    /// Setzt den Text für das UI Popup zur Anpassung der Wandelemente
    /// </summary>
    public void SetPopUp()
    {
        SelectedObjectTransform = selectionManager.SelectedObject;
        string wand = null;
        string s = "";
        if (SelectedObjectTransform.parent != null)
        {
            s = SelectedObjectTransform.parent.name;
        }

        switch (s)
        {
            case "WallNorth":
                wand = "Nordwand";
                break;
            case "WallEast":
                wand = "Ostwand";
                break;
            case "WallSouth":
                wand = "Südwand";
                break;
            case "WallWest":
                wand = "Westwand";
                break;
            default:
                wand = "Wand";
                break;
        }

        myText.text = "Geben Sie die gewünschte Länge von der " + wand + " ein";

        if (selectionManager != null && !selectionManager.selected)
        {
            popUp.SetActive(selectionManager.selected);
        }
    }


    /// <summary>
    /// Setzt den Text für das UI Popup zur Anpassung der Wandlänge
    /// </summary>
    public void SetPopUpScaleWall()
    {
        SelectedObjectTransform = selectionManager.SelectedObject;
        string wand = null;
        string s = "";
        if (SelectedObjectTransform.parent != null)
        {
            s = SelectedObjectTransform.parent.name;
        }

        switch (s)
        {
            case "WallNorth":
                wand = "Nordwand";
                break;
            case "WallEast":
                wand = "Ostwand";
                break;
            case "WallSouth":
                wand = "Südwand";
                break;
            case "WallWest":
                wand = "Westwand";
                break;
            default:
                wand = "Wand";
                break;
        }

        myText.text = "Geben Sie die gewünschte Länge zum Erweitern der " + wand + " ein";

        if (selectionManager != null && !selectionManager.selected)
        {
            popUpScaleWall.SetActive(selectionManager.selected);
        }
    }

    /// <summary>
    /// Entfernt das UI Popup
    /// </summary>
    /// <param name="popUp"> Das Popup das geschlossen werden soll</param>
    public void close(GameObject popUp)
    {
        popUp.SetActive(false);
    }


    /// <summary>
    /// Prüft ob Eingabe gültig ist und ruft entsprechende Methode auf
    /// </summary>
    public void OnScaleWallButtonClicked()
    {
        if (!string.IsNullOrEmpty(inputNumberOfWalls.text) && !inputNumberOfWalls.text.Equals("0"))
        {
            SelectedObjectTransform = selectionManager.SelectedObject;
            int length = Convert.ToInt32(inputNumberOfWalls.text);
            inputNumberOfWalls.text = "";
            ScaleWall(length);
        }
    }

    /// <summary>
    /// Passt die Wand um die übergebene Länge an
    /// </summary>
    /// <param name="length"> Länge um die Wand verlängert(+) oder verkürzt(-) werden soll</param>
    private void ScaleWall(int length)
    {
        if (SelectedObjectTransform.CompareTag("SelectableAttachedInnerWall"))
        {
            Vector3 direction = new Vector3();
            if (SelectedObjectTransform.parent.parent.CompareTag("NorthWall")) direction = Vector3.right;
            else if (SelectedObjectTransform.parent.parent.CompareTag("EastWall")) direction = Vector3.back;
            else if (SelectedObjectTransform.parent.parent.CompareTag("SouthWall")) direction = Vector3.left;
            else if (SelectedObjectTransform.parent.parent.CompareTag("WestWall")) direction = Vector3.forward;
            
            // Wenn das Wandelement nicht die Ursprungsgröße hat, wird diese wiederhergestellt.
            if (SelectedObjectTransform.localScale.x < 0.99f)
            {
                SelectedObjectTransform.localScale = new Vector3(1.0f, 3.2f, 0.2f);
                SelectedObjectTransform.position += Vector3.Scale(new Vector3(0.2f, 0, 0.2f), direction);
            }
            
            var scale = SelectedObjectTransform.localScale;
            Vector3 overlapBox = new Vector3(scale.x * 1.1f, scale.y, scale.z);
            Collider[] colliders = Physics.OverlapBox(SelectedObjectTransform.position, overlapBox / 2f, SelectedObjectTransform.rotation);
            Transform neighborWall = null;
            foreach (Collider collider1 in colliders)
            {
                if (collider1.transform != SelectedObjectTransform &&
                    ((collider1.transform.parent.parent.CompareTag("OuterWall") && collider1.transform.parent == SelectedObjectTransform.parent.parent) ||
                     (collider1.transform.rotation == SelectedObjectTransform.rotation && collider1.CompareTag("SelectableWall"))))
                {
                    neighborWall = collider1.transform;
                }
            }


            if (neighborWall != null)
            {
                for (int i = 0; i < Math.Abs(length); i++)
                {
                    if (length < 0)
                    {
                        bool foundOuterWall = false;
                        colliders = Physics.OverlapBox(SelectedObjectTransform.position, SelectedObjectTransform.localScale / 2, SelectedObjectTransform.rotation);
                        foreach (Collider collider1 in colliders)
                        {
                            if (collider1.transform.parent.parent.CompareTag("OuterWall") && collider1.transform.parent == SelectedObjectTransform.parent.parent)
                            {
                                foundOuterWall = true;
                            }

                            if (collider1.transform != SelectedObjectTransform && collider1.transform.rotation == SelectedObjectTransform.rotation &&
                                (collider1.CompareTag("SelectableWall") || collider1.CompareTag("SelectableWindow") || collider1.CompareTag("SelectableDoor")))
                            {
                                Destroy(collider1.gameObject);
                            }
                        }

                        if (!foundOuterWall)
                        {
                            Vector3 localScale = SelectedObjectTransform.localScale;
                            SelectedObjectTransform.position -= Vector3.Scale(new Vector3(localScale.x, 0, localScale.x), direction);
                        }
                        else
                        {
                            Destroy(SelectedObjectTransform.gameObject);
                            break;
                        }
                    }
                    else
                    {
                        Vector3 position = SelectedObjectTransform.position;
                        Vector3 localScale = SelectedObjectTransform.localScale;
                        Transform hitInnerWall = null;
                        bool foundOuterWall = false;
                        bool hitsInnerWall = false;
                        bool isEndPiece = false;

                        colliders = Physics.OverlapBox(position + Vector3.Scale(new Vector3(localScale.x, 0, localScale.x), direction), localScale / 2.1f, SelectedObjectTransform.rotation);
                        foreach (Collider collider1 in colliders)
                        {
                            if (i == length - 1 && collider1.transform.parent.name.Equals("InnerWall"))
                            {
                                hitsInnerWall = true;
                                hitInnerWall = collider1.transform;
                                if (collider1.CompareTag("SelectableAttachedInnerWall"))
                                {
                                    isEndPiece = true;
                                }
                            }

                            if (collider1.transform.parent.parent.CompareTag("OuterWall") && SelectedObjectTransform.parent.parent != collider1.transform.parent)
                            {
                                foundOuterWall = true;
                            }
                        }

                        if (!foundOuterWall)
                        {
                            Vector3 dirHitInnerWall = new Vector3();
                            Instantiate(WallPrefab, position, SelectedObjectTransform.rotation, SelectedObjectTransform.parent);
                            SelectedObjectTransform.position += Vector3.Scale(new Vector3(localScale.x, 0, localScale.x), direction);
                            if (hitsInnerWall)
                            {
                                if (hitInnerWall.parent.parent.CompareTag("NorthWall")) dirHitInnerWall = Vector3.right;
                                else if (hitInnerWall.parent.parent.CompareTag("EastWall")) dirHitInnerWall = Vector3.back;
                                else if (hitInnerWall.parent.parent.CompareTag("SouthWall")) dirHitInnerWall = Vector3.left;
                                else if (hitInnerWall.parent.parent.CompareTag("WestWall")) dirHitInnerWall = Vector3.forward;
                                SelectedObjectTransform.localScale = new Vector3(localScale.x / 2.0f + localScale.z / 2.0f, localScale.y, localScale.z);
                                SelectedObjectTransform.position -= Vector3.Scale(new Vector3(0.2f, 0, 0.2f), direction);
                                if (isEndPiece)
                                {
                                    hitInnerWall.localScale = new Vector3(localScale.x / 2.0f + localScale.z / 2.0f, localScale.y, localScale.z);
                                    hitInnerWall.position -= Vector3.Scale(new Vector3(0.2f, 0, 0.2f), dirHitInnerWall);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            if (length <= -(SelectedObjectTransform.parent.childCount - 1))
            {
                Debug.Log("Warehouse smaller than number of walls that should be removed!");
                return;
            }

            GameObject oppositeWall = null;
            Transform oppositeWallRim = null;
            GameObject temp;
            GameObject parentWall = SelectedObjectTransform.parent.gameObject;
            Transform connectingWall = null;
            Vector3 selectedWallLocalScale = SelectedObjectTransform.localScale;
            Transform neighborWall = null;

            Collider[] colliders = Physics.OverlapSphere(SelectedObjectTransform.position, SelectedObjectTransform.localScale.x + 0.1f);
            foreach (var collider in colliders)
            {
                if (collider.transform.rotation != SelectedObjectTransform.rotation)
                {
                    connectingWall = collider.transform.parent;
                }
                else if (collider.transform != SelectedObjectTransform)
                {
                    neighborWall = collider.gameObject.transform;
                }
            }

            string parentWallTag = parentWall.tag;
            switch (parentWallTag)
            {
                case "NorthWall":
                    oppositeWall = GameObject.FindGameObjectWithTag("SouthWall");
                    break;
                case "EastWall":
                    oppositeWall = GameObject.FindGameObjectWithTag("WestWall");
                    break;
                case "SouthWall":
                    oppositeWall = GameObject.FindGameObjectWithTag("NorthWall");
                    break;
                case "WestWall":
                    oppositeWall = GameObject.FindGameObjectWithTag("EastWall");
                    break;
            }

            if (oppositeWall != null && connectingWall != null && neighborWall != null)
            {
                Vector3 direction = -((SelectedObjectTransform.position - neighborWall.position).normalized);
                if (SelectedObjectTransform.CompareTag("LeftWallRim"))
                {
                    foreach (var wallRim in GameObject.FindGameObjectsWithTag("RightWallRim"))
                    {
                        if (wallRim.transform.parent.gameObject == oppositeWall)
                        {
                            oppositeWallRim = wallRim.transform;
                            break;
                        }
                    }

                    if (oppositeWallRim != null)
                    {
                        for (int i = 0; i < Math.Abs(length); i++)
                        {
                            if (length < 0)
                            {
                                SelectedObjectTransform.position = SelectedObjectTransform.position + direction * selectedWallLocalScale.x;
                                oppositeWallRim.position = oppositeWallRim.position + direction * oppositeWallRim.localScale.x;
                                connectingWall.position = connectingWall.position + direction * selectedWallLocalScale.x;

                                colliders = Physics.OverlapSphere(SelectedObjectTransform.position, SelectedObjectTransform.localScale.x / 4);
                                foreach (var collider in colliders)
                                {
                                    if (collider.gameObject != SelectedObjectTransform.gameObject)
                                    {
                                        Destroy(collider.gameObject);
                                    }
                                }

                                colliders = Physics.OverlapSphere(oppositeWallRim.position, oppositeWallRim.localScale.x / 4);
                                foreach (var collider in colliders)
                                {
                                    if (collider.gameObject != oppositeWallRim.gameObject)
                                    {
                                        Destroy(collider.gameObject);
                                    }
                                }
                            }
                            else
                            {
                                //Extend Selected Wall
                                temp = Instantiate(WallPrefab, SelectedObjectTransform.position, SelectedObjectTransform.rotation, SelectedObjectTransform.parent);
                                temp.tag = "SelectableWall";
                                SelectedObjectTransform.position = SelectedObjectTransform.position - direction * selectedWallLocalScale.x;

                                //Extend Opposite Wall
                                temp = Instantiate(WallPrefab, oppositeWallRim.position, oppositeWallRim.rotation, oppositeWallRim.parent);
                                temp.tag = "SelectableWall";
                                oppositeWallRim.position = oppositeWallRim.position - direction * oppositeWallRim.localScale.x;

                                //Move Connecting Wall
                                connectingWall.position = connectingWall.position - direction * selectedWallLocalScale.x;
                            }
                        }
                    }
                }
                else if (SelectedObjectTransform.CompareTag("RightWallRim"))
                {
                    foreach (var wallRim in GameObject.FindGameObjectsWithTag("LeftWallRim"))
                    {
                        if (wallRim.transform.parent.gameObject == oppositeWall)
                        {
                            oppositeWallRim = wallRim.transform;
                            break;
                        }
                    }

                    if (oppositeWallRim != null)
                    {
                        for (int i = 0; i < Math.Abs(length); i++)
                        {
                            if (length < 0)
                            {
                                SelectedObjectTransform.position = SelectedObjectTransform.position + direction * selectedWallLocalScale.x;
                                oppositeWallRim.position = oppositeWallRim.position + direction * oppositeWallRim.localScale.x;
                                connectingWall.position = connectingWall.position + direction * selectedWallLocalScale.x;

                                colliders = Physics.OverlapSphere(SelectedObjectTransform.position, SelectedObjectTransform.localScale.x / 4);
                                foreach (var collider in colliders)
                                {
                                    if (collider.gameObject != SelectedObjectTransform.gameObject)
                                    {
                                        Destroy(collider.gameObject);
                                    }
                                }

                                colliders = Physics.OverlapSphere(oppositeWallRim.position, oppositeWallRim.localScale.x / 4);
                                foreach (var collider in colliders)
                                {
                                    if (collider.gameObject != oppositeWallRim.gameObject)
                                    {
                                        Destroy(collider.gameObject);
                                    }
                                }
                            }
                            else
                            {
                                //Extend Selected Wall
                                temp = Instantiate(WallPrefab, SelectedObjectTransform.position, SelectedObjectTransform.rotation, SelectedObjectTransform.parent);
                                temp.tag = "SelectableWall";
                                SelectedObjectTransform.position = SelectedObjectTransform.position - direction * selectedWallLocalScale.x;

                                //Extend Opposite Wall
                                temp = Instantiate(WallPrefab, oppositeWallRim.position, oppositeWallRim.rotation, oppositeWallRim.parent);
                                temp.tag = "SelectableWall";
                                oppositeWallRim.position = oppositeWallRim.position - direction * oppositeWallRim.localScale.x;

                                //Move Connecting Wall
                                connectingWall.position = connectingWall.position - direction * selectedWallLocalScale.x;
                            }
                        }
                    }
                }
            }
        }


        close(popUpScaleWall);
    }
}