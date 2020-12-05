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

    private List<Collider> objectsInRange = new List<Collider>();

    // Start is called before the first frame update

    void Start()
    {
        addWindowText.text = StringResourceManager.LoadString("@AddWindowText");
        addDoorText.text = StringResourceManager.LoadString("@AddDoorText");
        addWallText.text = StringResourceManager.LoadString("@AddWallText");
        SelectedObjectTransform = selectionManager.SelectedObject;
        popUp.SetActive(selectionManager.selected);
        SelectionManager.WallSelected += OnWallSelected;
        SelectionManager.LeftWallRimSelected += OnLeftWallRimSelected;
        SelectionManager.RightWallRimSelected += OnRightWallRimSelected;
        SelectionManager.InnerWallSelected += OnInnerWallSelected;
        SelectionManager.AttachedInnerWallSelected += OnAttachedInnerWallSelected;
    }

    private void Update()
    {
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
                    if (collider1.gameObject != SelectedObjectTransform.gameObject && collider1.transform.rotation != SelectedObjectTransform.rotation)
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
                    Vector3 direction = (SelectedObjectTransform.position - snapObject.position).normalized;
                    if (SelectedObjectTransform.rotation.eulerAngles.y % 180.0f == 0)
                    {
                        SelectedObjectTransform.position = snapObject.position + Vector3.Scale(new Vector3(SelectedObjectTransform.localScale.z / 2.0f + SelectedObjectTransform.localScale.x / 2.0f, 0, 0), direction);
                    }
                    else
                    {
                        SelectedObjectTransform.position = snapObject.position + Vector3.Scale(new Vector3(0, 0, SelectedObjectTransform.localScale.z / 2.0f + SelectedObjectTransform.localScale.x / 2.0f), direction);
                    }

                    SelectedObjectTransform.tag = "SelectableAttachedInnerWall";
                    SelectedObjectTransform.parent = snapObject.parent.Find("InnerWalls");
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

    public void OnAddInnerWallButtonClicked()
    {
        GameObject temp = Instantiate(WallPrefab, ObjectSpawn.transform.position, ObjectSpawn.transform.rotation);
        temp.tag = "SelectableInnerWall";
    }

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
                    Destroy(rightWallElement);
                    Destroy(SelectedObjectTransform.gameObject);
                    Instantiate(WindowPrefab, SelectedObjectTransform.position + SelectedObjectTransform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2.0f)), SelectedObjectTransform.rotation, parent);
                }
                else if (foundLeftWallElement)
                {
                    Destroy(leftWallElement);
                    Destroy(SelectedObjectTransform.gameObject);
                    Instantiate(WindowPrefab, leftWallElement.transform.position + leftWallElement.transform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2.0f)), SelectedObjectTransform.rotation, parent);
                }
                else
                {
                    Debug.Log("Not enough Space for Door");
                }
            }
        }

        close(popUp);
    }

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
                    Destroy(rightWallElement);
                    Destroy(SelectedObjectTransform.gameObject);
                    Instantiate(DoorPrefab, SelectedObjectTransform.position + SelectedObjectTransform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2.0f)), SelectedObjectTransform.rotation, parent);
                }
                else if (foundLeftWallElement)
                {
                    Destroy(leftWallElement);
                    Destroy(SelectedObjectTransform.gameObject);
                    Instantiate(DoorPrefab, leftWallElement.transform.position + leftWallElement.transform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2.0f)), SelectedObjectTransform.rotation, parent);
                }
                else
                {
                    Debug.Log("Not enough Space for Door");
                }
            }
        }

        close(popUp);
    }

    public void OnAddWallClick()
    {
        SelectedObjectTransform = selectionManager.SelectedObject;
        if (!SelectedObjectTransform.gameObject.CompareTag("SelectableWall"))
        {
            Transform parent = SelectedObjectTransform.parent;
            if (SelectedObjectTransform.CompareTag("SelectableDoor") || SelectedObjectTransform.CompareTag("SelectableWindow"))
            {
                Destroy(SelectedObjectTransform.gameObject);
                Vector3 position = SelectedObjectTransform.position;
                Vector3 localScale = SelectedObjectTransform.localScale;
                Quaternion rotation = SelectedObjectTransform.rotation;

                Instantiate(WallPrefab, position - SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, parent);
                Instantiate(WallPrefab, position + SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 2.0f)), rotation, parent);
            }
            else
            {
                Destroy(SelectedObjectTransform.gameObject);
                Instantiate(WallPrefab, SelectedObjectTransform.position, SelectedObjectTransform.rotation, parent);
            }
        }

        close(popUp);
    }

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
                myText.text = "Geben Sie die gewünschte Länge von der Südwand ein";
                break;
        }

        myText.text = "Geben Sie die gewünschte Länge von der " + wand + " ein";

        if (selectionManager != null && !selectionManager.selected)
        {
            popUp.SetActive(selectionManager.selected);
        }
    }

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
                myText.text = "Geben Sie die gewünschte Länge von der Südwand ein";
                break;
        }

        myText.text = "Geben Sie die gewünschte Länge zum Erweitern der " + wand + " ein";

        if (selectionManager != null && !selectionManager.selected)
        {
            popUpScaleWall.SetActive(selectionManager.selected);
        }
    }

    public void close(GameObject popUp)
    {
        popUp.SetActive(false);
    }


    public void OnScaleWallButtonClicked()
    {
        SelectedObjectTransform = selectionManager.SelectedObject;
        int length = Convert.ToInt32(inputNumberOfWalls.text);
        inputNumberOfWalls.text = "";
        ScaleWall(length);
    }

    private void ScaleWall(int length)
    {
        if (SelectedObjectTransform.CompareTag("SelectableAttachedInnerWall"))
        {
            Collider[] colliders = Physics.OverlapBox(SelectedObjectTransform.position, SelectedObjectTransform.localScale / 2, SelectedObjectTransform.rotation);
            Transform neighborWall = null;
            foreach (Collider collider1 in colliders)
            {
                if (collider1.transform != SelectedObjectTransform &&
                    ((collider1.transform.parent.CompareTag("OuterWall") && collider1.transform.parent == SelectedObjectTransform.parent.parent) || collider1.transform.rotation == SelectedObjectTransform.rotation))
                {
                    neighborWall = collider1.transform;
                }
            }


            if (neighborWall != null)
            {
                Vector3 direction = (SelectedObjectTransform.position - neighborWall.position).normalized;
                for (int i = 0; i < Math.Abs(length); i++)
                {
                    if (length < 0)
                    {
                        bool foundOuterWall = false;
                        colliders = Physics.OverlapBox(SelectedObjectTransform.position, SelectedObjectTransform.localScale / 2, SelectedObjectTransform.rotation);
                        foreach (Collider collider1 in colliders)
                        {
                            if (collider1.transform.parent.CompareTag("OuterWall"))
                            {
                                foundOuterWall = true;
                            }

                            if (collider1.transform != SelectedObjectTransform && collider1.transform.rotation == SelectedObjectTransform.rotation)
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
                        bool foundOuterWall = false;

                        colliders = Physics.OverlapBox(position + Vector3.Scale(new Vector3(localScale.x, 0, localScale.x), direction), localScale / 2, SelectedObjectTransform.rotation);
                        foreach (Collider collider1 in colliders)
                        {
                            if (collider1.transform.parent.CompareTag("OuterWall") && SelectedObjectTransform.parent.parent != collider1.transform.parent)
                            {
                                foundOuterWall = true;
                            }
                        }

                        if (!foundOuterWall)
                        {
                            Instantiate(WallPrefab, position, SelectedObjectTransform.rotation, SelectedObjectTransform.parent);
                            position += Vector3.Scale(new Vector3(localScale.x, 0, localScale.x), direction);
                            SelectedObjectTransform.position = position;
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
                if (collider.gameObject.transform.parent.rotation != SelectedObjectTransform.parent.rotation)
                {
                    connectingWall = collider.transform.parent;
                }
                else if (collider.transform != SelectedObjectTransform)
                {
                    neighborWall = collider.gameObject.transform;
                }
            }

            foreach (var wall in GameObject.FindGameObjectsWithTag("OuterWall"))
            {
                if (wall.transform.rotation == parentWall.transform.rotation && wall != parentWall)
                {
                    oppositeWall = wall;
                    break;
                }
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
                                    if (collider.gameObject != SelectedObjectTransform.gameObject)
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
                                    if (collider.gameObject != SelectedObjectTransform.gameObject)
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