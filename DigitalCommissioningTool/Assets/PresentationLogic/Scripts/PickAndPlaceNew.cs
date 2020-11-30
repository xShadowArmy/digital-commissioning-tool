using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAndPlaceNew : MonoBehaviour
{
    public LayerMask dragable;
    public GameObject selected;
    public bool isDragging;
    bool hitObject; //keinObject bewegen wenn schon eins ausgewählt
    float lastPosX;
    float lastPosZ;
    //Vector3 mousePos;
    public LayerMask mask;
    bool moveX, moveZ;

    int rotationRight = 0;
    int rotationLeft = 0;
    int rotation = 0;



    // Start is called before the first frame update
    void Start()
    {
        //pos = transform.position;
        isDragging = false;
        hitObject = false;
        lastPosX = 0f;
        lastPosZ = 0f;
        selected = null;

        SelectionManager.StorageSelected += OnStorageSelected;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            Debug.Log(selected);

            //rotation
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.E))
            {
               // selected.transform.Rotate(new Vector3(0,45,0));
                selected.transform.rotation = selected.transform.rotation * Quaternion.AngleAxis(45, Vector3.up);
                //bei 90 rotiert es um 180°? 

                if (rotationRight < 4)
                {
                    rotationRight++;
                }

                if (rotationRight == 4)
                {
                    rotationRight = 0;
                }

            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                selected.transform.rotation = selected.transform.rotation * Quaternion.AngleAxis(-45, Vector3.up);


                if (rotationLeft < 4)
                {
                    rotationLeft++;
                }

                if (rotationLeft == 4)
                {
                    rotationRight = 0;
                }
            }

            rotation = rotationRight - rotationLeft;

            if (rotation < 0)
            {
                rotation = rotation + 4;
            }

            if (!moveX && !moveZ)
            {
                // Debug.Log("auswahl: " + storage.gameObject.name);
                Ray ray2 = GameObject.FindGameObjectWithTag("EditorModeCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit2;
                //                  (ray, hit, range, mask)
                if (Physics.Raycast(ray2, out hit2, Mathf.Infinity, mask))
                {
                    //get position (x,y,z) from click
                    float posX = hit2.point.x;
                    float posZ = hit2.point.z;

                    //Debug.Log("x: " + posX + "z: " + posZ);

                    //=> erhält maus position
                    //Updated Posx!=x, only when mouse is moving 
                    if (lastPosX != posX || lastPosZ != posZ)
                    {
                        lastPosX = posX;
                        lastPosZ = posZ;
                      //  Debug.Log("x: " + posX + "z: " + posZ);

                        //Cursor
                        selected.transform.position = new Vector3(posX, 0f, posZ);
                        //Objekte fliegen auf kamera zu => add layer floor -> boden hinzufügen und am würfel entfernen
                    }


                }
                if (Input.GetKeyDown("return"))
                {
                    hitObject = false;
                    isDragging = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                moveX = true;
                moveZ = false;
            }
            if (moveX)
            {
                float x = 0;
                //Debug.Log("bewegt in x Richtung");

                float speed = 0.2f;
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    x = Input.GetAxis("Horizontal") * speed;
                }
                else
                {
                    x = Input.GetAxis("Mouse X") * speed;
                }
                //  Debug.Log("X Richtung: " + x);

                Vector3 pos = selected.transform.position;


                if (rotation == 0)
                {
                    pos = new Vector3(0, 0, x * Time.deltaTime);
                }
                if (rotation == 1)
                {
                    pos = new Vector3(-(x * Time.deltaTime), 0, 0);
                }
                if (rotation == 2)
                {
                    pos = new Vector3(0, 0, -(x * Time.deltaTime));
                }
                if (rotation == 3)
                {
                    pos = new Vector3(x * Time.deltaTime, 0, 0);
                }



                selected.transform.Translate(pos);



                if (Input.GetKeyDown(KeyCode.Y))
                {
                    moveX = false;
                    moveZ = true;
                }
                if (Input.GetKeyDown("return"))
                {
                    moveX = false;
                    moveZ = false;
                    isDragging = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                moveX = false;
                moveZ = true;
            }
            if (moveZ)
            {
                float z = 0;
                // Debug.Log("bewegt in y Richtung");

                float speed = 0.2f;
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                {
                    z = Input.GetAxis("Vertical") * speed;
                }
                else
                {
                    z = Input.GetAxis("Mouse Y") * speed;
                }
                // Debug.Log("Z Richtung: " + z);

                Vector3 pos = selected.transform.position;


                if (rotation == 0)
                {
                    pos = new Vector3(-(z * Time.deltaTime), 0, 0);
                }
                if (rotation == 1)
                {
                    pos = new Vector3(0, 0, -(z * Time.deltaTime));
                }
                if (rotation == 2)
                {
                    pos = new Vector3((z * Time.deltaTime), 0, 0);
                }
                if (rotation == 3)
                {
                    pos = new Vector3(0, 0, (z * Time.deltaTime));
                }


                selected.transform.Translate(pos);

                if (Input.GetKeyDown(KeyCode.X))
                {
                    moveX = true;
                    moveZ = false;
                }
                if (Input.GetKeyDown("return"))
                {
                    moveX = false;
                    moveZ = false;
                    isDragging = false;
                }
            }
            if (Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Backspace)) {
                Debug.Log("Delete " + selected.name);
                Destroy(selected);
                isDragging = false;
            }


        }
    }

    private void OnStorageSelected(Transform storage)
    {
        selected = storage.gameObject;
        isDragging = true;

    }

}






