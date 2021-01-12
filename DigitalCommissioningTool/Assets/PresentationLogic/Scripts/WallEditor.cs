using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SystemFacade;
using ApplicationFacade;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ApplicationFacade.Application;
using ApplicationFacade.Warehouse;

public class WallEditor : MonoBehaviour
{
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
    [SerializeField] private Camera EditorModeCamera;
    [SerializeField] private GameObject ObjectSpawn;

    public GameObject popUp;
    public LayerMask mask;

    private Vector3 oldMousePos = new Vector3(0, 0, 0);
    private Transform SelectedObjectTransform;
    private WallData wall;
    private bool moveObject = false;
    private bool innerWallSelected = false;
    private ModeHandler ModeHandler;
    private GameObject SwitchModeButton;
    private GameObject AddInnerWallButton;
    private List<Collider> objectsInRange = new List<Collider>();

    
    /// <summary>
    /// Setzt Texte für UI Buttons, weist Eventfunktionen zu und initialisiert Objekte
    /// </summary>
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
        if (innerWallSelected && SelectedObjectTransform != null)
        {
            wall = GameManager.GameWarehouse.GetWall(SelectedObjectTransform.gameObject);
            Ray ray = EditorModeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if ( Input.GetKeyDown( KeyManager.MoveSelected.Code ) )
            {
                moveObject = true;
            }

            //Drag
            if ( moveObject && Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
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
                    if (collider1.gameObject != SelectedObjectTransform.gameObject && collider1.transform.rotation != SelectedObjectTransform.rotation && collider1.CompareTag("SelectableWall") &&
                        GameManager.GameWarehouse.GetWall(collider1.gameObject).Class == WallClass.Outer)
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
            if ( moveObject && Input.GetMouseButtonDown(0) || Input.GetKeyDown( KeyCode.Return ) )
            {
                //Snap to other Wall
                if (objectsInRange.Count > 0)
                {
                    WallFace wallFace = WallFace.Undefined;
                    Transform snapObject = objectsInRange[0].transform;
                    Vector3 direction = new Vector3();

                    if (Math.Abs(SelectedObjectTransform.rotation.eulerAngles.y - 180.0f) < 1f || Math.Abs(SelectedObjectTransform.rotation.eulerAngles.y) < 1f)
                    {
                        if (snapObject.parent.CompareTag("NorthWall"))
                        {
                            direction = Vector3.right;
                            wallFace = WallFace.North;
                        }
                        else if (snapObject.parent.CompareTag("SouthWall"))
                        {
                            direction = Vector3.left;
                            wallFace = WallFace.South;
                        }

                        wall.SetPosition(snapObject.position + Vector3.Scale(new Vector3(SelectedObjectTransform.localScale.z / 2.0f + SelectedObjectTransform.localScale.x / 2.0f, 0, 0), direction));
                        wall.SetRotation(wall.Object.transform.rotation);
                    }
                    else
                    {
                        if (snapObject.parent.CompareTag("EastWall"))
                        {
                            direction = Vector3.back;
                            wallFace = WallFace.East;
                        }
                        else if (snapObject.parent.CompareTag("WestWall"))
                        {
                            direction = Vector3.forward;
                            wallFace = WallFace.West;
                        }

                        wall.SetPosition(snapObject.position + Vector3.Scale(new Vector3(0, 0, SelectedObjectTransform.localScale.z / 2.0f + SelectedObjectTransform.localScale.x / 2.0f), direction));
                        wall.SetRotation(wall.Object.transform.rotation);
                    }

                    wall.SetTag("SelectableAttachedInnerWall");
                    wall.SetFace(wallFace);

                    foreach (Collider collider1 in objectsInRange)
                    {
                        Transform invisibleWall = collider1.transform.Find("InvisibleWall").transform;
                        invisibleWall.GetComponent<Renderer>().material = transparentMaterial;
                    }
                }

                innerWallSelected = false;
                moveObject = false;
                selectionManager.ResetSelection();
            }

            //Rotate
            if (Input.GetKeyUp(KeyManager.Rotate.Code))
            {
                wall.Object.transform.Rotate(Vector3.up * 90);
            }

            //Remove
            if (Input.GetKeyUp(KeyManager.RemoveSelected.Code))
            {
                SelectedObjectTransform = null;
                innerWallSelected = false;
                GameManager.GameWarehouse.RemoveWall(wall);
            }
        }
    }

    /// <summary>
    /// Wird ausgefüht wenn eine noch nicht angedockte Innenwand ausgewöhlt wird und setzt Selected auf true, damit diese verschoben werden kann.
    /// </summary>
    /// <param name="selectedObject">Der Transform des GameObjects das ausgewählt wurde</param>
    private void OnInnerWallSelected(Transform selectedObject)
    {
        innerWallSelected = true;
    }

    /// <summary>
    /// Wird ausgeführt wenn das Randelement einer Innenwand ausgewählt wird und öffnet das UI Panel zum Anpassen der Länge
    /// </summary>
    /// <param name="selectedObject">Der Transform des GameObjects das ausgewählt wurde</param>
    private void OnAttachedInnerWallSelected(Transform selectedObject)
    {
        popUp.SetActive(false);
        popUpScaleWall.SetActive(true);
        innerWallSelected = false;
    }

    /// <summary>
    /// Wird ausgeführt wenn rechtes Randelement einer Außenwand ausgewählt wird und öffnet das UI Panel zum Anpassen der Länge
    /// </summary>
    /// <param name="selectedObject">Der Transform des GameObjects das ausgewählt wurde</param>
    private void OnRightWallRimSelected(Transform selectedObject)
    {
        popUp.SetActive(false);
        popUpScaleWall.SetActive(true);
        innerWallSelected = false;
    }

    /// <summary>
    /// Wird ausgeführt wenn linkes Randelement einer Außenwand ausgewählt wird und öffnet das UI Panel zum Anpassen der Länge
    /// </summary>
    /// <param name="selectedObject">Der Transform des GameObjects das ausgewählt wurde</param>
    private void OnLeftWallRimSelected(Transform selectedObject)
    {
        popUp.SetActive(false);
        popUpScaleWall.SetActive(true);
        innerWallSelected = false;
    }

    /// <summary>
    /// Wird ausgeführt wenn ein Wandelement (nicht Randelemente) ausgewählt wird und öffnet das WallEditor UI Panel zum Anpassen des Wandelements
    /// </summary>
    /// <param name="selectedObject">Der Transform des GameObjects das ausgewählt wurde</param>
    private void OnWallSelected(Transform selectedObject)
    {
        popUp.SetActive(true);
        popUpScaleWall.SetActive(false);
        innerWallSelected = false;
    }

    /// <summary>
    /// Fügt Innenwand hinzu wenn der Button gedrückt wurde und passt den Tag entsprechend an.
    /// </summary>
    public void OnAddInnerWallButtonClicked()
    {
        //Ray ray = EditorModeCamera.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, 20))
        //{
        //    wall = GameManager.GameWarehouse.CreateWall(hit.point + new Vector3(0f, 1.6f, 0f), ObjectSpawn.transform.rotation, new Vector3(1f, 3.2f, 0.2f), WallFace.Undefined, WallClass.Inner, "SelectableInnerWall");
        // }
        GameManager.GameWarehouse.CreateWall( new Vector3( ObjectSpawn.transform.position.z, 1.6f, ObjectSpawn.transform.position.z ), ObjectSpawn.transform.rotation, new Vector3(1f,3.2f,0.2f), WallFace.Undefined, WallClass.Inner, "SelectableInnerWall" );
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
                DoorData door = GameManager.GameWarehouse.GetDoor(SelectedObjectTransform.gameObject);

                GameManager.GameWarehouse.CreateWindow(door.Position, door.Rotation, door.Scale, door.Face, door.Class);
                GameManager.GameWarehouse.RemoveDoor(door);
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
                            else if (!collider.gameObject.CompareTag("LeftWallRim") && !collider.gameObject.CompareTag("RightWallRim") && !collider.gameObject.CompareTag("SelectableAttachedInnerWall"))
                            {
                                foundLeftWallElement = true;
                                leftWallElement = collider.gameObject;
                            }
                        }
                    }
                }

                if (foundRightWallElement)
                {
                    WallData rw = GameManager.GameWarehouse.GetWall(rightWallElement);
                    WallData lw = GameManager.GameWarehouse.GetWall(SelectedObjectTransform.gameObject);

                    GameManager.GameWarehouse.RemoveWall(rw);
                    GameManager.GameWarehouse.RemoveWall(lw);

                    GameManager.GameWarehouse.CreateWindow(SelectedObjectTransform.position + SelectedObjectTransform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2.0f)), SelectedObjectTransform.rotation,
                        SelectedObjectTransform.localScale, rw.Face, rw.Class);
                }
                else if (foundLeftWallElement)
                {
                    WallData lw = GameManager.GameWarehouse.GetWall(leftWallElement);
                    WallData rw = GameManager.GameWarehouse.GetWall(SelectedObjectTransform.gameObject);

                    GameManager.GameWarehouse.RemoveWall(rw);
                    GameManager.GameWarehouse.RemoveWall(lw);

                    GameManager.GameWarehouse.CreateWindow(leftWallElement.transform.position + leftWallElement.transform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2.0f)), SelectedObjectTransform.rotation,
                        SelectedObjectTransform.localScale, rw.Face, rw.Class);
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
                WindowData window = GameManager.GameWarehouse.GetWindow(SelectedObjectTransform.gameObject);

                GameManager.GameWarehouse.CreateDoor(window.Position, window.Rotation, window.Scale, DoorType.Door, window.Face, window.Class);
                GameManager.GameWarehouse.RemoveWindow(window);
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
                            else if (!collider.gameObject.CompareTag("LeftWallRim") && !collider.gameObject.CompareTag("RightWallRim") && !collider.gameObject.CompareTag("SelectableAttachedInnerWall"))
                            {
                                foundLeftWallElement = true;
                                leftWallElement = collider.gameObject;
                            }
                        }
                    }
                }

                if (foundRightWallElement)
                {
                    WallData rw = GameManager.GameWarehouse.GetWall(rightWallElement);
                    WallData lw = GameManager.GameWarehouse.GetWall(SelectedObjectTransform.gameObject);

                    GameManager.GameWarehouse.RemoveWall(rw);
                    GameManager.GameWarehouse.RemoveWall(lw);

                    GameManager.GameWarehouse.CreateDoor(SelectedObjectTransform.position + SelectedObjectTransform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2.0f)), SelectedObjectTransform.rotation,
                        SelectedObjectTransform.localScale, DoorType.Door, rw.Face, rw.Class);
                }
                else if (foundLeftWallElement)
                {
                    WallData lw = GameManager.GameWarehouse.GetWall(leftWallElement);
                    WallData rw = GameManager.GameWarehouse.GetWall(SelectedObjectTransform.gameObject);

                    GameManager.GameWarehouse.RemoveWall(rw);
                    GameManager.GameWarehouse.RemoveWall(lw);

                    GameManager.GameWarehouse.CreateDoor(leftWallElement.transform.position + leftWallElement.transform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2.0f)), SelectedObjectTransform.rotation,
                        SelectedObjectTransform.localScale, DoorType.Door, rw.Face, rw.Class);
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
            if (SelectedObjectTransform.CompareTag("SelectableWindow"))
            {
                WindowData window = GameManager.GameWarehouse.GetWindow(SelectedObjectTransform.gameObject);
                GameManager.GameWarehouse.RemoveWindow(window);

                Vector3 position = window.Position;
                Vector3 localScale = window.Scale;
                Quaternion rotation = window.Rotation;

                GameManager.GameWarehouse.CreateWall(position - SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, new Vector3( 1f, 3.2f, 0.2f ), window.Face, window.Class);
                GameManager.GameWarehouse.CreateWall(position + SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, new Vector3( 1f, 3.2f, 0.2f ), window.Face, window.Class);
            }

            else if (SelectedObjectTransform.CompareTag("SelectableDoor"))
            {
                DoorData door = GameManager.GameWarehouse.GetDoor(SelectedObjectTransform.gameObject);
                GameManager.GameWarehouse.RemoveDoor(door);

                Vector3 position = door.Position;
                Vector3 localScale = door.Scale;
                Quaternion rotation = door.Rotation;

                GameManager.GameWarehouse.CreateWall(position - SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, new Vector3( 1f, 3.2f, 0.2f ), door.Face, door.Class);
                GameManager.GameWarehouse.CreateWall(position + SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, new Vector3( 1f, 3.2f, 0.2f ), door.Face, door.Class);
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

        string s = "";
        if (SelectedObjectTransform.parent != null)
        {
            s = SelectedObjectTransform.parent.name;
        }

      

        
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
        string scaleLengthText = inputNumberOfWalls.text;
        bool isNumeric = int.TryParse(scaleLengthText, out int scaleLength);
        if (!string.IsNullOrEmpty(scaleLengthText) && isNumeric && scaleLength != 0)
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
        wall = GameManager.GameWarehouse.GetWall(SelectedObjectTransform.gameObject);
        if (wall.Object.CompareTag("SelectableAttachedInnerWall"))
        {
            Vector3 direction = new Vector3();
            switch (wall.Face)
            {
                case WallFace.East:
                    direction = Vector3.back;
                    break;
                case WallFace.North:
                    direction = Vector3.right;
                    break;
                case WallFace.South:
                    direction = Vector3.left;
                    break;
                case WallFace.West:
                    direction = Vector3.forward;
                    break;
            }

            // Wenn das Wandelement nicht die Ursprungsgröße hat, wird diese wiederhergestellt.
            if (SelectedObjectTransform.localScale.x < 0.99f)
            {
                wall.SetScale(new Vector3(1.0f, 3.2f, 0.2f));
                wall.SetPosition(wall.Position + Vector3.Scale(new Vector3(0.2f, 0, 0.2f), direction));
            }

            var scale = SelectedObjectTransform.localScale;
            Vector3 overlapBox = new Vector3(scale.x * 1.1f, scale.y, scale.z);
            Collider[] colliders = Physics.OverlapBox(wall.Position, overlapBox / 2f, wall.Rotation);
            Transform neighborWall = null;
            foreach (Collider collider1 in colliders)
            {
                if (collider1.CompareTag("SelectableWall") || collider1.CompareTag("SelectableWindow") || collider1.CompareTag("SelectableDoor"))
                {
                    WallData colliderWallData = GameManager.GameWarehouse.GetWall(collider1.gameObject);
                    if (collider1.transform != SelectedObjectTransform && collider1.CompareTag("SelectableWall")
                                                                       && colliderWallData.Class == WallClass.Outer
                                                                       && colliderWallData.Face == wall.Face)
                    {
                        neighborWall = collider1.transform;
                    }
                    else if (collider1.transform.rotation == SelectedObjectTransform.rotation)
                    {
                        neighborWall = collider1.transform;
                    }
                }
            }
            
            if (neighborWall != null)
            {
                for (int i = 0; i < Math.Abs(length); i++)
                {
                    if (length < 0)
                    {
                        bool foundOuterWall = false;
                        colliders = Physics.OverlapBox(SelectedObjectTransform.position, SelectedObjectTransform.localScale / 1.9f, SelectedObjectTransform.rotation);
                        foreach (Collider collider1 in colliders)
                        {
                            if (collider1.transform.parent.parent.CompareTag("OuterWall"))
                            {
                                if (collider1.CompareTag("SelectableWall"))
                                {
                                    WallData outerWalLData = GameManager.GameWarehouse.GetWall(collider1.gameObject);
                                    if (wall.Face == outerWalLData.Face)
                                    {
                                        foundOuterWall = true;
                                    }
                                }
                                else if (collider1.CompareTag("SelectableWindow"))
                                {
                                    WindowData outerWalLData = GameManager.GameWarehouse.GetWindow(collider1.gameObject);
                                    if (wall.Face == outerWalLData.Face)
                                    {
                                        foundOuterWall = true;
                                    }
                                }
                                else if (collider1.CompareTag("SelectableDoor"))
                                {
                                    DoorData outerWalLData = GameManager.GameWarehouse.GetDoor(collider1.gameObject);
                                    if (wall.Face == outerWalLData.Face)
                                    {
                                        foundOuterWall = true;
                                    }
                                }
                            }

                            if (collider1.transform != SelectedObjectTransform && collider1.transform.rotation == SelectedObjectTransform.rotation)
                            {
                                if (collider1.CompareTag("SelectableWall"))
                                {
                                    GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(collider1.gameObject));
                                }
                                else if (collider1.CompareTag("SelectableWindow"))
                                {
                                    GameManager.GameWarehouse.RemoveWindow(GameManager.GameWarehouse.GetWindow(collider1.gameObject));
                                }
                                else if (collider1.CompareTag("SelectableDoor"))
                                {
                                    GameManager.GameWarehouse.RemoveDoor(GameManager.GameWarehouse.GetDoor(collider1.gameObject));
                                }
                            }
                        }
                        
                        if (!foundOuterWall)
                        {
                            Vector3 localScale = SelectedObjectTransform.localScale;
                            wall.SetPosition(wall.Position - Vector3.Scale(new Vector3(localScale.x, 0, localScale.x), direction));
                        }
                        else
                        {
                            GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(SelectedObjectTransform.gameObject));
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
                            
                            if (collider1.CompareTag("SelectableWall") && GameManager.GameWarehouse.GetWall(collider1.gameObject).Class == WallClass.Outer
                                                                       && GameManager.GameWarehouse.GetWall(collider1.gameObject).Face != wall.Face)
                            {
                                foundOuterWall = true;
                            }
                            else if (collider1.CompareTag("SelectableWindow") && GameManager.GameWarehouse.GetWindow(collider1.gameObject).Class == WallClass.Outer
                                                                              && GameManager.GameWarehouse.GetWindow(collider1.gameObject).Face != wall.Face)
                            {
                                foundOuterWall = true;
                            }
                            else if (collider1.CompareTag("SelectableDoor") && GameManager.GameWarehouse.GetDoor(collider1.gameObject).Class == WallClass.Outer
                                                                            && GameManager.GameWarehouse.GetDoor(collider1.gameObject).Face != wall.Face)
                            {
                                foundOuterWall = true;
                            }
                        }

                        if (!foundOuterWall)
                        {
                            Vector3 dirHitInnerWall = new Vector3();
                            GameManager.GameWarehouse.CreateWall(position, wall.Rotation, new Vector3( 1f, 3.2f, 0.2f ), wall.Face, wall.Class, "SelectableWall");
                            wall.SetPosition(wall.Position + Vector3.Scale(new Vector3(localScale.x, 0, localScale.x), direction));

                            if (hitsInnerWall)
                            {
                                if (hitInnerWall.CompareTag("SelectableWall") || hitInnerWall.CompareTag("SelectableAttachedInnerWall"))
                                {
                                    WallData hitInnerWallData = GameManager.GameWarehouse.GetWall(hitInnerWall.gameObject);

                                    switch (hitInnerWallData.Face)
                                    {
                                        case WallFace.North:
                                            dirHitInnerWall = Vector3.right;
                                            break;
                                        case WallFace.East:
                                            dirHitInnerWall = Vector3.back;
                                            break;
                                        case WallFace.South:
                                            dirHitInnerWall = Vector3.left;
                                            break;
                                        case WallFace.West:
                                            dirHitInnerWall = Vector3.forward;
                                            break;
                                    }

                                    wall.SetScale(new Vector3(localScale.x / 2.0f + localScale.z / 2.0f, localScale.y, localScale.z));
                                    wall.SetPosition(wall.Position - Vector3.Scale(new Vector3(0.2f, 0, 0.2f), direction));

                                    if (isEndPiece)
                                    {
                                        hitInnerWallData.SetScale(new Vector3(localScale.x / 2.0f + localScale.z / 2.0f, localScale.y, localScale.z));
                                        hitInnerWallData.SetPosition(hitInnerWallData.Position - Vector3.Scale(new Vector3(0.2f, 0, 0.2f), dirHitInnerWall));
                                    }
                                }
                                else if (hitInnerWall.CompareTag("SelectableWindow"))
                                {
                                    WindowData hitInnerWallData = GameManager.GameWarehouse.GetWindow(hitInnerWall.gameObject);

                                    switch (hitInnerWallData.Face)
                                    {
                                        case WallFace.North:
                                            dirHitInnerWall = Vector3.right;
                                            break;
                                        case WallFace.East:
                                            dirHitInnerWall = Vector3.back;
                                            break;
                                        case WallFace.South:
                                            dirHitInnerWall = Vector3.left;
                                            break;
                                        case WallFace.West:
                                            dirHitInnerWall = Vector3.forward;
                                            break;
                                    }

                                    wall.SetScale(new Vector3(localScale.x / 2.0f + localScale.z / 2.0f, localScale.y, localScale.z));
                                    wall.SetPosition(wall.Position - Vector3.Scale(new Vector3(0.2f, 0, 0.2f), direction));

                                    if (isEndPiece)
                                    {
                                        hitInnerWallData.SetScale(new Vector3(localScale.x / 2.0f + localScale.z / 2.0f, localScale.y, localScale.z));
                                        hitInnerWallData.SetPosition(hitInnerWallData.Position - Vector3.Scale(new Vector3(0.2f, 0, 0.2f), dirHitInnerWall));
                                    }
                                }
                                else if (hitInnerWall.CompareTag("SelectableDoor"))
                                {
                                    DoorData hitInnerWallData = GameManager.GameWarehouse.GetDoor(hitInnerWall.gameObject);

                                    if (hitInnerWallData.Face == WallFace.North) dirHitInnerWall = Vector3.right;
                                    else if (hitInnerWallData.Face == WallFace.East) dirHitInnerWall = Vector3.back;
                                    else if (hitInnerWallData.Face == WallFace.South) dirHitInnerWall = Vector3.left;
                                    else if (hitInnerWallData.Face == WallFace.West) dirHitInnerWall = Vector3.forward;

                                    wall.SetScale(new Vector3(localScale.x / 2.0f + localScale.z / 2.0f, localScale.y, localScale.z));
                                    wall.SetPosition(wall.Position - Vector3.Scale(new Vector3(0.2f, 0, 0.2f), direction));

                                    if (isEndPiece)
                                    {
                                        hitInnerWallData.SetScale(new Vector3(localScale.x / 2.0f + localScale.z / 2.0f, localScale.y, localScale.z));
                                        hitInnerWallData.SetPosition(hitInnerWallData.Position - Vector3.Scale(new Vector3(0.2f, 0, 0.2f), dirHitInnerWall));
                                    }
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
            Transform connectingWall = null;
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

            switch (wall.Face)
            {
                case WallFace.North:
                    oppositeWall = GameObject.FindGameObjectWithTag("SouthWall");
                    break;
                case WallFace.East:
                    oppositeWall = GameObject.FindGameObjectWithTag("WestWall");
                    break;
                case WallFace.South:
                    oppositeWall = GameObject.FindGameObjectWithTag("NorthWall");
                    break;
                case WallFace.West:
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
                                WallData oppositeWallRimData = GameManager.GameWarehouse.GetWall(oppositeWallRim.gameObject);
                                WallData connectingWallData = GameManager.GameWarehouse.GetWall(connectingWall.gameObject);
                                wall.SetPosition(wall.Position + direction * wall.Scale.x);
                                oppositeWallRimData.SetPosition(oppositeWallRimData.Position + direction * oppositeWallRimData.Scale.x);
                                if (connectingWall.CompareTag("NorthWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.NorthWallObjects)
                                    {
                                        child.SetPosition(child.Position + direction * wall.Scale.x);
                                    }
                                }
                                else if (connectingWall.CompareTag("EastWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.EastWallObjects)
                                    {
                                        child.SetPosition(child.Position + direction * wall.Scale.x);
                                    }
                                }
                                else if (connectingWall.CompareTag("SouthWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.SouthWallObjects)
                                    {
                                        child.SetPosition(child.Position + direction * wall.Scale.x);
                                    }
                                }
                                else if (connectingWall.CompareTag("WestWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.WestWallObjects)
                                    {
                                        child.SetPosition(child.Position + direction * wall.Scale.x);
                                    }
                                }

                                colliders = Physics.OverlapSphere(SelectedObjectTransform.position, SelectedObjectTransform.localScale.x / 4);
                                foreach (var collider in colliders)
                                {
                                    if (collider.gameObject != SelectedObjectTransform.gameObject)
                                    {
                                        if (collider.CompareTag("SelectableWall"))
                                        {
                                            GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(collider.gameObject));
                                        }
                                        else if (collider.CompareTag("SelectableWindow"))
                                        {
                                            GameManager.GameWarehouse.RemoveWindow(GameManager.GameWarehouse.GetWindow(collider.gameObject));
                                        }
                                        else if (collider.CompareTag("SelectableDoor"))
                                        {
                                            GameManager.GameWarehouse.RemoveDoor(GameManager.GameWarehouse.GetDoor(collider.gameObject));
                                        }
                                    }
                                }

                                colliders = Physics.OverlapSphere(oppositeWallRim.position, oppositeWallRim.localScale.x / 4);
                                foreach (var collider in colliders)
                                {
                                    if (collider.gameObject != oppositeWallRim.gameObject)
                                    {
                                        if (collider.CompareTag("SelectableWall"))
                                        {
                                            GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(collider.gameObject));
                                        }
                                        else if (collider.CompareTag("SelectableWindow"))
                                        {
                                            GameManager.GameWarehouse.RemoveWindow(GameManager.GameWarehouse.GetWindow(collider.gameObject));
                                        }
                                        else if (collider.CompareTag("SelectableDoor"))
                                        {
                                            GameManager.GameWarehouse.RemoveDoor(GameManager.GameWarehouse.GetDoor(collider.gameObject));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Extend Selected Wall
                                GameManager.GameWarehouse.CreateWall(wall.Position, wall.Rotation, wall.Scale, wall.Face, wall.Class, "SelectableWall");
                                wall.SetPosition(wall.Position - direction * wall.Scale.x);

                                //Extend Opposite Wall
                                WallData oppositeWallRimData = GameManager.GameWarehouse.GetWall(oppositeWallRim.gameObject);
                                GameManager.GameWarehouse.CreateWall(oppositeWallRimData.Position, oppositeWallRimData.Rotation, oppositeWallRimData.Scale, oppositeWallRimData.Face, oppositeWallRimData.Class, "SelectableWall");
                                oppositeWallRimData.SetPosition(oppositeWallRimData.Position - direction * oppositeWallRimData.Scale.x);

                                //Move Connecting Wall
                                if (connectingWall.CompareTag("NorthWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.NorthWallObjects)
                                    {
                                        child.SetPosition(child.Position - direction * wall.Scale.x);
                                    }
                                }
                                else if (connectingWall.CompareTag("EastWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.EastWallObjects)
                                    {
                                        child.SetPosition(child.Position - direction * wall.Scale.x);
                                    }
                                }
                                else if (connectingWall.CompareTag("SouthWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.SouthWallObjects)
                                    {
                                        child.SetPosition(child.Position - direction * wall.Scale.x);
                                    }
                                }
                                else if (connectingWall.CompareTag("WestWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.WestWallObjects)
                                    {
                                        child.SetPosition(child.Position - direction * wall.Scale.x);
                                    }
                                }
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
                                WallData oppositeWallRimData = GameManager.GameWarehouse.GetWall(oppositeWallRim.gameObject);
                                WallData connectingWallData = GameManager.GameWarehouse.GetWall(connectingWall.gameObject);
                                wall.SetPosition(wall.Position + direction * wall.Scale.x);
                                oppositeWallRimData.SetPosition(oppositeWallRimData.Position + direction * oppositeWallRimData.Scale.x);
                                if (connectingWall.CompareTag("NorthWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.NorthWallObjects)
                                    {
                                        child.SetPosition(child.Position + direction * wall.Scale.x);
                                    }
                                }
                                else if (connectingWall.CompareTag("EastWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.EastWallObjects)
                                    {
                                        child.SetPosition(child.Position + direction * wall.Scale.x);
                                    }
                                }
                                else if (connectingWall.CompareTag("SouthWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.SouthWallObjects)
                                    {
                                        child.SetPosition(child.Position + direction * wall.Scale.x);
                                    }
                                }
                                else if (connectingWall.CompareTag("WestWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.WestWallObjects)
                                    {
                                        child.SetPosition(child.Position + direction * wall.Scale.x);
                                    }
                                }

                                colliders = Physics.OverlapSphere(SelectedObjectTransform.position, SelectedObjectTransform.localScale.x / 4);
                                foreach (var collider in colliders)
                                {
                                    if (collider.gameObject != SelectedObjectTransform.gameObject)
                                    {
                                        if (collider.CompareTag("SelectableWall"))
                                        {
                                            GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(collider.gameObject));
                                        }
                                        else if (collider.CompareTag("SelectableWindow"))
                                        {
                                            GameManager.GameWarehouse.RemoveWindow(GameManager.GameWarehouse.GetWindow(collider.gameObject));
                                        }
                                        else if (collider.CompareTag("SelectableDoor"))
                                        {
                                            GameManager.GameWarehouse.RemoveDoor(GameManager.GameWarehouse.GetDoor(collider.gameObject));
                                        }
                                    }
                                }

                                colliders = Physics.OverlapSphere(oppositeWallRim.position, oppositeWallRim.localScale.x / 4);
                                foreach (var collider in colliders)
                                {
                                    if (collider.gameObject != oppositeWallRim.gameObject)
                                    {
                                        if (collider.CompareTag("SelectableWall"))
                                        {
                                            GameManager.GameWarehouse.RemoveWall(GameManager.GameWarehouse.GetWall(collider.gameObject));
                                        }
                                        else if (collider.CompareTag("SelectableWindow"))
                                        {
                                            GameManager.GameWarehouse.RemoveWindow(GameManager.GameWarehouse.GetWindow(collider.gameObject));
                                        }
                                        else if (collider.CompareTag("SelectableDoor"))
                                        {
                                            GameManager.GameWarehouse.RemoveDoor(GameManager.GameWarehouse.GetDoor(collider.gameObject));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Extend Selected Wall
                                GameManager.GameWarehouse.CreateWall(wall.Position, wall.Rotation, wall.Scale, wall.Face, wall.Class, "SelectableWall");
                                wall.SetPosition(wall.Position - direction * wall.Scale.x);

                                //Extend Opposite Wall
                                WallData oppositeWallRimData = GameManager.GameWarehouse.GetWall(oppositeWallRim.gameObject);
                                GameManager.GameWarehouse.CreateWall(oppositeWallRimData.Position, oppositeWallRimData.Rotation, oppositeWallRimData.Scale, oppositeWallRimData.Face, oppositeWallRimData.Class, "SelectableWall");
                                oppositeWallRimData.SetPosition(oppositeWallRimData.Position - direction * oppositeWallRimData.Scale.x);

                                //Move Connecting Wall
                                if (connectingWall.CompareTag("NorthWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.NorthWallObjects)
                                    {
                                        child.SetPosition(child.Position - direction * wall.Scale.x);
                                    }
                                }
                                else if (connectingWall.CompareTag("EastWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.EastWallObjects)
                                    {
                                        child.SetPosition(child.Position - direction * wall.Scale.x);
                                    }
                                }
                                else if (connectingWall.CompareTag("SouthWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.SouthWallObjects)
                                    {
                                        child.SetPosition(child.Position - direction * wall.Scale.x);
                                    }
                                }
                                else if (connectingWall.CompareTag("WestWall"))
                                {
                                    foreach (WallObjectData child in GameManager.GameWarehouse.WestWallObjects)
                                    {
                                        child.SetPosition(child.Position - direction * wall.Scale.x);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        close(popUpScaleWall);
    }
}