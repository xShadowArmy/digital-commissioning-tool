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

    private Transform SelectedObject { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("GameController");
        popUp = controller.GetComponent<WallEditor>();
    }

    // Update is called once per frame
    void Update()
    {
        Camera editorModeCamera = GameObject.FindGameObjectWithTag("EditorModeCamera").GetComponent<Camera>();
        GameObject switchModeButton = GameObject.Find("SwitchModeButton");
        ModeHandler modeHandler = switchModeButton.GetComponent<ModeHandler>();

        if (modeHandler.Mode.Equals("EditorMode"))
        {
            Ray ray = editorModeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit))
            {
                Transform tempObject = hit.transform;
                if (tempObject.CompareTag("SelectableWall"))
                {
                    Debug.Log("1a");
                    if (SelectedObject != null)
                    {
                        Debug.Log("2a");
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
                else if (SelectedObject != null)
                {
                    Debug.Log("1b");
                    Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                    invisibleWall.GetComponent<Renderer>().material = defaultMaterial;
                    selected = false;
                    popUp.SetPopUp(tempObject.name);

                }
            }
        }
    }
}