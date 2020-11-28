using System.Collections;
using System.Collections.Generic;
using SystemFacade;
using UnityEngine;
using UnityEngine.UI;

public class WallEditor : MonoBehaviour
{
    public GameObject popUp;
    [HideInInspector] public GameObject selectedObject;
    private SelectionManager selectWall;
    [SerializeField] private Text addWindowText;
    [SerializeField] private Text addDoorText;
    [SerializeField] private Text addWallText;
    [SerializeField] private SelectionManager selectionManager;

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
        selectedObject = GameObject.Find("SelectionManager");
        selectWall = selectedObject.GetComponent<SelectionManager>();
        SelectedObjectTransform = selectionManager.SelectedObject;
        popUp.SetActive(selectWall.selected);
    }

    public void OnAddWindowClick()
    {
        SelectedObjectTransform = selectionManager.SelectedObject;
        if (!SelectedObjectTransform.gameObject.CompareTag("SelectableWindow"))
        {
            Transform parent = SelectedObjectTransform.parent;
            if (SelectedObjectTransform.CompareTag("SelectableDoor"))
            {
                Destroy(SelectedObjectTransform.gameObject);
                Vector3 position = SelectedObjectTransform.position;
                Vector3 localScale = SelectedObjectTransform.localScale;
                Quaternion rotation = SelectedObjectTransform.rotation;
                Instantiate(WindowPrefab, position - SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 4)), rotation, parent);
                Instantiate(WindowPrefab, position + SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 4)), rotation, parent);
            }
            else
            {
                Destroy(SelectedObjectTransform.gameObject);
                Instantiate(WindowPrefab, SelectedObjectTransform.position, SelectedObjectTransform.rotation, parent);
            }
        }
    }

    public void OnAddDoorClick()
    {
        SelectedObjectTransform = selectionManager.SelectedObject;
        if (!SelectedObjectTransform.gameObject.CompareTag("SelectableDoor"))
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
                    if (colliderTransform != SelectedObjectTransform && colliderTransform.rotation == SelectedObjectTransform.rotation)
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
                Instantiate(DoorPrefab, SelectedObjectTransform.position + SelectedObjectTransform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2)), SelectedObjectTransform.rotation, parent);
            }
            else if (foundLeftWallElement)
            {
                Destroy(leftWallElement);
                Destroy(SelectedObjectTransform.gameObject);
                Instantiate(DoorPrefab, leftWallElement.transform.position + leftWallElement.transform.TransformDirection(Vector3.left * (SelectedObjectTransform.localScale.x / 2)), SelectedObjectTransform.rotation, parent);
            }
            else
            {
                Debug.Log("Not enough Space for Door");
            }
        }
    }

    public void OnAddWallClick()
    {
        SelectedObjectTransform = selectionManager.SelectedObject;
        if (!SelectedObjectTransform.gameObject.CompareTag("SelectableWall"))
        {
            Transform parent = SelectedObjectTransform.parent;
            if (SelectedObjectTransform.CompareTag("SelectableDoor"))
            {
                Destroy(SelectedObjectTransform.gameObject);
                Vector3 position = SelectedObjectTransform.position;
                Vector3 localScale = SelectedObjectTransform.localScale;
                Quaternion rotation = SelectedObjectTransform.rotation;

                Instantiate(WallPrefab, position - SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 4)), rotation, parent);
                Instantiate(WallPrefab, position + SelectedObjectTransform.TransformDirection(Vector3.left * (localScale.x / 4)), rotation, parent);
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
        popUp.SetActive(selectWall.selected);
    }

    public void close()
    {
        popUp.SetActive(false);
    }
}