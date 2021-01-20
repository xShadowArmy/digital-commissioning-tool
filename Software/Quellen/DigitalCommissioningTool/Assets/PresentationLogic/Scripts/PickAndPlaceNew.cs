using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApplicationFacade.Application;
using ApplicationFacade.Warehouse;


public class PickAndPlaceNew : MonoBehaviour
{
    public GameObject selected;
    bool isDragging;
    float lastPosX;
    float lastPosZ;
    public LayerMask m_LayerMask, mask;
    bool moveX, moveZ;
    public Material material1, material2;
    Renderer rend;
    Transform invisibleWall;
    int rotation;
    private GameObject SwitchModeButton;
    private ModeHandler ModeHandler;

    // Start is called before the first frame update
    void Start()
    {
        //Beim starten alles auf Anfangswerte setzen: 
        isDragging = false;
        lastPosX = 0f;
        lastPosZ = 0f;
        invisibleWall = null;
        selected = null;
        rotation = 0;

        SwitchModeButton = GameObject.Find("SwitchModeButton");
        ModeHandler = SwitchModeButton.GetComponent<ModeHandler>();
        SelectionManager.StorageSelected += OnStorageSelected;
        SelectionManager.MovableStorageSelected += OnStorageSelected;
    }

    private void OnMouseUp()
    {
        selected = null;
        isDragging = false;
    }

    /// <summary>
    /// Wird aufgerufen wenn Regal ausgewählt.
    /// </summary>
    /// <param name="storage">Das ausgewählte Regal</param>
    private void OnStorageSelected(Transform storage)
    {
        if (ModeHandler.Mode.Equals("EditorMode"))
        {
            if (storage == null)
            {
                isDragging = false;
                selected = null;
            }

            else
            {
                selected = storage.gameObject;
            }
        }

    }

    /// <summary>
    /// Gibt zurück ob Objekt(e) getroffen werden 
    /// </summary>
    /// <returns></returns>
    private bool HitSomething()
    {
        BoxCollider c = selected.GetComponent<BoxCollider>();
        Collider[] hitColliders = Physics.OverlapBox(invisibleWall.position, invisibleWall.localScale / 2, Quaternion.identity, m_LayerMask);
        bool collisonDetected = false;
        foreach (Collider collider in hitColliders)
        {
            if ((collider.gameObject != selected && collider.transform.parent != selected.transform && collider.transform.parent.parent != selected.transform && !collider.CompareTag("SelectableFloor")))         //keine Kollision mit sich selber oder dem Boden 
            {
                collisonDetected = true;
            }
        }
        return collisonDetected;
    }

    /// <summary>
    /// rotiert Objekt
    /// </summary>
    /// <param name="selected"> Objekt das rotiert wird </param>
    private void Rotate(GameObject selected)
    {
        if (Input.GetKeyDown(KeyManager.Rotate.Code))
        {

            if (rotation < 4)                      //ermitteln wie oft im Uhrzeigersinn rotiert wurde da nur 4 möglich sind => bei 4 = 0
            {
                rotation++;
            }

            if (rotation == 4)
            {
                rotation = 0;
            }

            //die einzelnen Rotationsmöglichkeiten:
            if (rotation == 0)
            { selected.transform.rotation = Quaternion.Euler(0, 0, 0); }
            if (rotation == 1)
            { selected.transform.rotation = Quaternion.Euler(0, 90, 0); }
            if (rotation == 2)
            { selected.transform.rotation = Quaternion.Euler(0, 180, 0); }
            if (rotation == 3)
            { selected.transform.rotation = Quaternion.Euler(0, 270, 0); }
        }

        StorageData storage = GameManager.GameWarehouse.GetStorageRack(selected);

        if (storage == null)
        {
            storage = GameManager.GameContainer.GetContainer(selected);
        }

        storage.SetTransform(selected.transform);
    }

    /// <summary>
    /// Bewegung mit Maus
    /// </summary>
    /// <param name="selected">Objekt das Bewegt werden soll</param>
    private void MoveAnywhere(GameObject selected)
    {
        //um die Mausposition zu finden
        Ray ray2 = GameObject.FindGameObjectWithTag("EditorModeCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit2;
        //                  (ray, hit, range, mask)
        if (Physics.Raycast(ray2, out hit2, Mathf.Infinity, mask))
        {
            //erhalte koordinaten x und z durch die Maus Position (y = 0 da nicht nach oben bzw. unten bewegt werden soll)
            float posX = hit2.point.x;
            float posZ = hit2.point.z;

            //aktualisierte maus position (wenn Mauszeiger bewegt wird)
            if (lastPosX != posX || lastPosZ != posZ)
            {
                lastPosX = posX;
                lastPosZ = posZ;

                //Cursor = Regal (Maus position dem Regal übergeben)
                selected.transform.position = new Vector3(posX, 0f, posZ);
            }
        }

    }

    /// <summary>
    /// Regal platzieren  
    /// </summary>
    private void PlaceObject()
    {
        if (Input.GetMouseButton(0) || Input.GetKeyDown("return"))
        {
            if (HitSomething() == false)                         //Platzieren (mit enter taste oder linker maustaste) nur zulassen wenn das Objekt nicht auf (bzw. sich in) einem anderen Objekt steht
            {
                //Position speichern

                StorageData temp = GameManager.GameWarehouse.GetStorageRack(selected);

                if (temp == null)
                {
                    temp = GameManager.GameContainer.GetContainer(selected);
                }

                temp.SetTransform(selected.transform);

                //Werte wieder auf Anfangswerte setzten:
                moveX = false;
                moveZ = false;
                isDragging = false;
                rotation = 0;
                invisibleWall = null;
                selected = null;
            }
        }
    }

    /// <summary>
    /// in X Achse bewegen
    /// </summary>
    /// <param name="selected">Regal das bewegt werden soll</param>
    private void MoveInXAxis(GameObject selected)
    {
        Ray ray2 = GameObject.FindGameObjectWithTag("EditorModeCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit2;
        if (Physics.Raycast(ray2, out hit2, Mathf.Infinity, mask))
        {
            float posX = hit2.point.x;

            if (lastPosX != posX)
            {
                lastPosX = posX;
                selected.transform.position = new Vector3(posX, 0f, selected.transform.position.z);
            }
        }
    }

    /// <summary>
    /// in Z Achse bewegen
    /// </summary>
    /// <param name="selected">Regal das bewegt wird</param>
    private void MoveInZAxis(GameObject selected)
    {
        Ray ray2 = GameObject.FindGameObjectWithTag("EditorModeCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit2;

        if (Physics.Raycast(ray2, out hit2, Mathf.Infinity, mask))
        {
            float posZ = hit2.point.z;
            if (lastPosZ != posZ)
            {
                lastPosZ = posZ;
                selected.transform.position = new Vector3(selected.transform.position.x, 0f, posZ);
            }
        }
    }

    /// <summary>
    /// löscht Regal
    /// </summary>
    private void DeleteObject()
    {
        if (!GameManager.GameWarehouse.RemoveStorageRack(GameManager.GameWarehouse.GetStorageRack(selected)))
        {
            GameManager.GameContainer.RemoveContainer(GameManager.GameContainer.GetContainer(selected));
        }

        isDragging = false;     //= false da kein Objekt mehr ausgewählt 
    }

    // Update is called once per frame
    void Update()
    {
        if (selected != null)
        {
            if (Input.GetKeyDown(KeyManager.MoveSelected.Code))
            {
                isDragging = true;
            }

            if (Input.GetKeyDown(KeyManager.RemoveSelected.Code) && ModeHandler.Mode.Equals( "EditorMode" ))
            {
                DeleteObject();
            }
        }

        if (isDragging && ModeHandler.Mode.Equals("EditorMode") && selected != null)
        {
            invisibleWall = selected.transform.Find("InvisibleWall");               //aus ausgwähltem Regal das Kindelement "invisibleWall" finden
            rend = invisibleWall.GetComponent<Renderer>();                          //renderer um später das Material zu ändern
            if (HitSomething())
            {
                rend.material = material2;                                          //material ändern wenn es sich auf einem Objekt befindet
            }
            else
            {
                rend.material = material1;                                          //material ändern wenn nicht mehr auf einem anderen Objekt ist    
            }
            Rotate(selected);
            if (!moveX && !moveZ)                                                   //Wenn keine gewünschte Achse Angegeben erfolgt die Bewegung durch die Mausposition
            {
                MoveAnywhere(selected);
            }
            if (Input.GetKeyDown(KeyManager.MoveXAxis.Code))
            {
                moveX = !moveX;
                moveZ = false;
            }
            if (moveX)                                                              //wenn x ausgewählt soll es nur möglich sein das Regal in X-Achse zu bewegen 
            {
                MoveInXAxis(selected);
            }
            if (Input.GetKeyDown(KeyManager.MoveZAxis.Code))
            {
                moveX = false;
                moveZ = !moveZ;
            }
            if (moveZ)
            {                                                             //wenn z ausgewählt soll es nur möglich sein das Regal in z-Achse zu bewegen 
                MoveInZAxis(selected);
            }

            PlaceObject();
        }
    }
}






