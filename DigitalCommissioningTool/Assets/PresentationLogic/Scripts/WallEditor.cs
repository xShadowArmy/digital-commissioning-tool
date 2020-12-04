using System;
using System.Collections;
using System.Collections.Generic;
using SystemFacade;
using UnityEngine;
using UnityEngine.UI;

public class WallEditor : MonoBehaviour
{
    public GameObject popUp;


    private int numberOfWalls;
    [SerializeField] private Text addWindowText;
    [SerializeField] private Text addDoorText;
    [SerializeField] private Text addWallText;
    [SerializeField] private SelectionManager selectionManager;

    [SerializeField] private InputField inputNumberOfWalls;
    [SerializeField] private GameObject popUpScaleWall;

    [SerializeField] private GameObject DoorPrefab;
    [SerializeField] private GameObject WallPrefab;
    [SerializeField] private GameObject WindowPrefab;


    private Transform SelectedObjectTransform;
    public Text myText;
    string s = "test";

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
    }

    private void OnRightWallRimSelected(Transform selectedObject)
    {
        Debug.Log("RighWallRimSelected: " + selectedObject.gameObject.name);
        SetPopUpScaleWall();
    }

    private void OnLeftWallRimSelected(Transform selectedObject)
    {
        Debug.Log("LeftWallRimSelected: " + selectedObject.gameObject.name);
        SetPopUpScaleWall();
    }

    private void OnWallSelected(Transform selectedObject)
    {
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
                            if (relativePoint.x < 0.0)
                            {
                                foundRightWallElement = true;
                                rightWallElement = collider.gameObject;
                            }
                            else
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
                            if (relativePoint.x < 0.0)
                            {
                                foundRightWallElement = true;
                                rightWallElement = collider.gameObject;
                            }
                            else
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
    }

    public void SetPopUp(string s)
    {
        string wand = null;

        switch (s)
        {
            case "WallNord":
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

        if (selectionManager != null)
        {
            popUp.SetActive(selectionManager.selected);
        }
    }

    public void SetPopUpScaleWall()
    {
        SelectedObjectTransform = selectionManager.SelectedObject;
        string wand = null;
        string s = SelectedObjectTransform.parent.name;
        Debug.Log(selectionManager.SelectedObject.gameObject);

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

        if (selectionManager != null)
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
        if (length <= -(SelectedObjectTransform.parent.childCount-1))
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
                Debug.Log(connectingWall.gameObject.name);
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
            Vector3 dir = -((SelectedObjectTransform.position - neighborWall.position).normalized);
            Debug.Log(dir);
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
                            SelectedObjectTransform.position = SelectedObjectTransform.position + dir * selectedWallLocalScale.x;
                            oppositeWallRim.position = oppositeWallRim.position + dir * oppositeWallRim.localScale.x;
                            connectingWall.position = connectingWall.position + dir * selectedWallLocalScale.x;

                            colliders = Physics.OverlapSphere(SelectedObjectTransform.position, SelectedObjectTransform.localScale.x / 4);
                            foreach (var collider in colliders)
                            {
                                if (collider.gameObject != SelectedObjectTransform.gameObject)
                                {
                                    Debug.Log(colliders.Length);
                                    Debug.Log(collider.gameObject.name);
                                    Destroy(collider.gameObject);
                                }
                            }

                            colliders = Physics.OverlapSphere(oppositeWallRim.position, oppositeWallRim.localScale.x / 4);
                            foreach (var collider in colliders)
                            {
                                if (collider.gameObject != SelectedObjectTransform.gameObject)
                                {
                                    Debug.Log(colliders.Length);
                                    Debug.Log(collider.gameObject.name);
                                    Destroy(collider.gameObject);
                                }
                            }
                        }
                        else
                        {
                            //Extend Selected Wall
                            temp = Instantiate(WallPrefab, SelectedObjectTransform.position, SelectedObjectTransform.rotation, SelectedObjectTransform.parent);
                            temp.tag = "SelectableWall";
                            SelectedObjectTransform.position = SelectedObjectTransform.position - dir * selectedWallLocalScale.x;

                            //Extend Opposite Wall
                            temp = Instantiate(WallPrefab, oppositeWallRim.position, oppositeWallRim.rotation, oppositeWallRim.parent);
                            temp.tag = "SelectableWall";
                            oppositeWallRim.position = oppositeWallRim.position - dir * oppositeWallRim.localScale.x;

                            //Move Connecting Wall
                            connectingWall.position = connectingWall.position - dir * selectedWallLocalScale.x;
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
                            SelectedObjectTransform.position = SelectedObjectTransform.position + dir * selectedWallLocalScale.x;
                            oppositeWallRim.position = oppositeWallRim.position + dir * oppositeWallRim.localScale.x;
                            connectingWall.position = connectingWall.position + dir * selectedWallLocalScale.x;

                            colliders = Physics.OverlapSphere(SelectedObjectTransform.position, SelectedObjectTransform.localScale.x / 4);
                            foreach (var collider in colliders)
                            {
                                if (collider.gameObject != SelectedObjectTransform.gameObject)
                                {
                                    Debug.Log(colliders.Length);
                                    Debug.Log(collider.gameObject.name);
                                    Destroy(collider.gameObject);
                                }
                            }

                            colliders = Physics.OverlapSphere(oppositeWallRim.position, oppositeWallRim.localScale.x / 4);
                            foreach (var collider in colliders)
                            {
                                if (collider.gameObject != SelectedObjectTransform.gameObject)
                                {
                                    Debug.Log(colliders.Length);
                                    Debug.Log(collider.gameObject.name);
                                    Destroy(collider.gameObject);
                                }
                            }
                        }
                        else
                        {
                            //Extend Selected Wall
                            temp = Instantiate(WallPrefab, SelectedObjectTransform.position, SelectedObjectTransform.rotation, SelectedObjectTransform.parent);
                            temp.tag = "SelectableWall";
                            SelectedObjectTransform.position = SelectedObjectTransform.position - dir * selectedWallLocalScale.x;

                            //Extend Opposite Wall
                            temp = Instantiate(WallPrefab, oppositeWallRim.position, oppositeWallRim.rotation, oppositeWallRim.parent);
                            temp.tag = "SelectableWall";
                            oppositeWallRim.position = oppositeWallRim.position - dir * oppositeWallRim.localScale.x;

                            //Move Connecting Wall
                            connectingWall.position = connectingWall.position - dir * selectedWallLocalScale.x;
                        }
                    }
                }
            }
        }

        close(popUpScaleWall);
    }
}