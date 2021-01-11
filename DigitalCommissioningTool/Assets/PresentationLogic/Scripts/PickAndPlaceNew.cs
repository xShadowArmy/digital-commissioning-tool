using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApplicationFacade.Application;
using ApplicationFacade.Warehouse;


public class PickAndPlaceNew : MonoBehaviour
{
    public GameObject selected;                 //= ausgewähltes Regal
    bool isDragging;                            //= true wenn ein Regal ausgewählt
    float lastPosX;                             //= letzte MausPosition (X)
    float lastPosZ;                             //= letzte MausPosition (Z)
    public LayerMask m_LayerMask, mask;
    bool moveX, moveZ;                          //=true wenn jeweilige Achse ausgewählt     
    public Material material1, material2;       //material bei Kollision ändern (material1 = ok(blau), material2 = Kollision(rot))
    Renderer rend;
    Transform invisibleWall;                    //zum färben (Kindelement von Regal: "Überzug" ohne Kollider)            
    int rotation;                               //gesamt Rotation       
    int rotationRight;                          //Rotation im Uhrzeigersinn
    int rotationLeft;                           //Rotation gen den Uhrzeigersinn

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
        rotationRight = 0;
        rotationLeft = 0;
        
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

    //Auswahl Regal :
    private void OnStorageSelected(Transform storage)
    {
        if (ModeHandler.Mode.Equals("EditorMode"))
        {            
            if ( storage == null )
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
        //rotation im Uhrzeigersinn berechnen:
        if (Input.GetKeyDown(KeyManager.Rotate.Code) || Input.GetKeyDown(KeyManager.RotateRight.Code))
        {
            //Debug.Log("x: " + selected.transform.eulerAngles.x + " y: " + selected.transform.eulerAngles.y + " z : " + selected.transform.eulerAngles.z);

            if (rotationRight < 4)                      //ermitteln wie oft im Uhrzeigersinn rotiert wurde da nur 4 möglich sind => bei 4 = 0
            {
                rotationRight++;
            }

            if (rotationRight == 4)
            {
                rotationRight = 0;
            }

            //Gesamte Rotation:
            rotation = rotationRight - rotationLeft;   //Rotation gegenden Uhrzeigersinn abziehen um einen Wert für die gemeinsame rotation zu erhalten
            if ( rotation < 0 )
            {
                rotation = rotation + 4;               //Um nicht zwichen positiven und negativen Werten unterscheiden zu müssen, wird der gesamtwert mit 4 (Rotationsmöglichkeiten) addiert
            }
            //die einzelnen Rotationsmöglichkeiten:
            if ( rotation == 0 )
            { selected.transform.rotation = Quaternion.Euler( 0, 0, 0 ); }
            if ( rotation == 1 )
            { selected.transform.rotation = Quaternion.Euler( 0, 90, 0 ); }
            if ( rotation == 2 )
            { selected.transform.rotation = Quaternion.Euler( 0, 180, 0 ); }
            if ( rotation == 3 )
            { selected.transform.rotation = Quaternion.Euler( 0, 270, 0 ); }


        }
        //Rotation gegen den Uhrzeigersinn berechnen (beinahe identisch wie im Uhrzeiger sinn nur die Rotationswerte werden vertauscht)
        else if (Input.GetKeyDown(KeyManager.RotateLeft.Code))
        {
            if (rotationLeft < 4)
            {
                rotationLeft++;
            }

            if (rotationLeft == 4)
            {
                rotationLeft = 0;
            }

            //Gesamte Rotation:
            rotation = rotationRight - rotationLeft;   //Rotation gegenden Uhrzeigersinn abziehen um einen Wert für die gemeinsame rotation zu erhalten
            if ( rotation < 0 )
            {
                rotation = rotation + 4;               //Um nicht zwichen positiven und negativen Werten unterscheiden zu müssen, wird der gesamtwert mit 4 (Rotationsmöglichkeiten) addiert
            }
            //die einzelnen Rotationsmöglichkeiten:
            if ( rotation == 0 )
            { selected.transform.rotation = Quaternion.Euler( 0, 0, 0 ); }
            if ( rotation == 1 )
            { selected.transform.rotation = Quaternion.Euler( 0, 90, 0 ); }
            if ( rotation == 2 )
            { selected.transform.rotation = Quaternion.Euler( 0, 180, 0 ); }
            if ( rotation == 3 )
            { selected.transform.rotation = Quaternion.Euler( 0, 270, 0 ); }
        }

        StorageData storage = GameManager.GameWarehouse.GetStorageRack( selected );

        if ( storage == null )
        {
            storage = GameManager.GameContainer.GetContainer( selected );
        }

        storage.SetTransform( selected.transform );
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

                //(Wenn Objekt auf kamera zu fliegen => add layer floor -> boden hinzufügen und am würfel entfernen)
            }
        }

    }
    
    /// <summary>
    /// Regal platzieren  
    /// </summary>
    private void PlaceObject()
    {
        if ( Input.GetMouseButton( 0 ) || Input.GetKeyDown("return"))
        {
            if (HitSomething() == false)                         //Platzieren (mit enter taste) nur zulassen wenn das Objekt nicht auf (bzw. sich in) einem anderen Objekt steht
            {
                //Position speichern

                StorageData temp = GameManager.GameWarehouse.GetStorageRack( selected );

                if ( temp == null )
                {
                    temp = GameManager.GameContainer.GetContainer( selected );
                }

                temp.SetTransform( selected.transform );

                //Werte wieder auf Anfangswerte setzten:
                moveX = false;
                moveZ = false;
                isDragging = false;
                rotation = 0;
                rotationRight = 0;
                rotationLeft = 0;
                invisibleWall = null;
                selected = null;
            }
            /*else
            {   
                Debug.Log("Cant Place on Object");
            }*/
        }
    }

    /// <summary>
    /// in X Achse bewegen
    /// </summary>
    /// <param name="selected">Regal das bewegt werden soll</param>
    private void MoveInXAxis(GameObject selected)
    {
        //um die Mausposition zu finden
        Ray ray2 = GameObject.FindGameObjectWithTag("EditorModeCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit2;
        //                  (ray, hit, range, mask)
        if ( Physics.Raycast( ray2, out hit2, Mathf.Infinity, mask ) )
        {
            //erhalte koordinaten x und z durch die Maus Position (y = 0 da nicht nach oben bzw. unten bewegt werden soll)
            float posX = hit2.point.x;
       
            //aktualisierte maus position (wenn Mauszeiger bewegt wird)
            if ( lastPosX != posX )
            {
                lastPosX = posX;
       
                //Cursor = Regal (Maus position dem Regal übergeben)
                selected.transform.position = new Vector3( posX, 0f, selected.transform.position.z );
       
                //(Wenn Objekt auf kamera zu fliegen => add layer floor -> boden hinzufügen und am würfel entfernen)
            }
        }

       //da die Achsenbewegung abhängig von der Kamera ist (Editormodus Kamera im Modehandler):
      // ModeHandler modeHandler = GameObject.Find("SwitchModeButton").GetComponent<ModeHandler>();
      // Vector3 cameraRight = modeHandler.EditorModeCamera.transform.right;                         //camera right vector         
      // float x = 0;
      // float speed = 2f;
      // if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
      // {
      //     x = Input.GetAxis("Horizontal") * speed;                                                //neue x position je nachdem welche taste gedrückt 
      // }
      // else
      // {
      //     x = Input.GetAxis("Mouse X") * speed;                                                   //x wert nach maus position (falls statt taste maus horizontal bewegt wird)
      // }
      // Vector3 pos = selected.transform.position;                                                  //pos = position des ausgweählten Objekts 
      // 
      // Vector3 cR = cameraRight;                                                                   //cR = X-Achse (aus Kamera)
      // cR.y = 0;
      // cR = cameraRight.normalized;
      // pos = new Vector3(pos.x, pos.y, (x * Time.deltaTime));                                          //aktualisiere pos mit neuem wert für pos.z
      // pos = cR * pos.z;                                                                           //berechne pos mit camera.transform
      // 
      // selected.transform.Translate(pos, Space.World);                                             //bewege Regal nach pos, Space.World damit unabhängig von rotation 

    }

    /// <summary>
    /// in Y Achse bewegen
    /// </summary>
    /// <param name="selected">Regal das bewegt wird</param>
    private void MoveInZAxis(GameObject selected)
    {
        //um die Mausposition zu finden
        Ray ray2 = GameObject.FindGameObjectWithTag("EditorModeCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit2;
        //                  (ray, hit, range, mask)
        if ( Physics.Raycast( ray2, out hit2, Mathf.Infinity, mask ) )
        {
            //erhalte koordinaten x und z durch die Maus Position (y = 0 da nicht nach oben bzw. unten bewegt werden soll)
            float posZ = hit2.point.z;

            //aktualisierte maus position (wenn Mauszeiger bewegt wird)
            if ( lastPosZ != posZ )
            {
                lastPosZ = posZ;

                //Cursor = Regal (Maus position dem Regal übergeben)
                selected.transform.position = new Vector3( selected.transform.position.x, 0f, posZ );

                //(Wenn Objekt auf kamera zu fliegen => add layer floor -> boden hinzufügen und am würfel entfernen)
            }
        }

        //Ähnlich wie MoveInXAxis nur mit der anderen Achse: 
        //ModeHandler modeHandler = GameObject.Find("SwitchModeButton").GetComponent<ModeHandler>();
        //Vector3 cameraForward = modeHandler.EditorModeCamera.transform.forward;                          //camera.transform.forward für vertikale position
        //
        //float z = 0;
        //float speed = 2f;
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        //{
        //    z = Input.GetAxis("Vertical") * speed;
        //}
        //else
        //{
        //    z = Input.GetAxis("Mouse Y") * speed;
        //}
        //Vector3 pos = selected.transform.position;
        //
        //Vector3 cF = cameraForward;
        //cF.y = 0;
        //cF = cF.normalized;
        //pos = new Vector3((z * Time.deltaTime), 0, 0);
        //pos = cF * pos.x;
        //selected.transform.Translate(pos, Space.World);

    }

    /// <summary>
    /// löscht Regal
    /// </summary>
    private void DeleteObject()
    {
        //Debug.Log("Delete " + selected.name);
        if ( !GameManager.GameWarehouse.RemoveStorageRack(GameManager.GameWarehouse.GetStorageRack(selected)) )
        {
            GameManager.GameContainer.RemoveContainer( GameManager.GameContainer.GetContainer( selected ) );
        }

        isDragging = false;     //= false da kein Objekt mehr ausgewählt 
    }

    // Update is called once per frame
    void Update()
    {
        if ( selected != null )
        {
            if ( Input.GetKeyDown( KeyManager.MoveSelected.Code ) )
            {
                isDragging = true;
            }

            if ( Input.GetKeyDown( KeyManager.RemoveSelected.Code ) )
            {
                DeleteObject( );
            }
        }

        if (isDragging && ModeHandler.Mode.Equals("EditorMode") && selected != null )
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
            if (moveZ){                                                             //wenn y ausgewählt soll es nur möglich sein das Regal in y-Achse zu bewegen 
                MoveInZAxis(selected);
            }

            PlaceObject();
        }
    }
}






