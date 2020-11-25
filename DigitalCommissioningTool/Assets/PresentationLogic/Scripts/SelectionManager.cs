using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public delegate void ObjectSelected(Transform selectedObject);

    public event ObjectSelected WallSelected;

    public event ObjectSelected StorageSelected;

    private Transform SelectedObject { get; set; }


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
            if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit))
            {
                Transform tempObject = hit.transform;
                if (tempObject.CompareTag("SelectableWall"))
                {
                    if (SelectedObject != null)
                    {
                        SelectedObject.Find("InvisibleWall").transform.GetComponent<Renderer>().material =
                            defaultMaterial;
                        selected = false;
                        popUp.SetPopUp(tempObject.name);
                    }

                    SelectedObject = tempObject;
                    Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                    invisibleWall.GetComponent<Renderer>().material = selectedMaterial;
                    selected = true;
                    popUp.SetPopUp(tempObject.name);
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
                    selected = true;
                    StorageSelected?.Invoke(SelectedObject);
                }
                else if (SelectedObject != null)
                {
                    Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                    invisibleWall.GetComponent<Renderer>().material = defaultMaterial;
                    selected = false;
                    popUp.SetPopUp(tempObject.name);
                }
            }
        }
    }
}