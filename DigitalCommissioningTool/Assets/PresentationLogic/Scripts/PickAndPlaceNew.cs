using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAndPlaceNew : MonoBehaviour
{
    public LayerMask dragable;
    GameObject selected;
    bool isDragging;
    bool hitObject; //keinObject bewegen wenn schon eins ausgewählt
    float lastPosX;
    float lastPosZ;
    Vector3 mousePos;
    public LayerMask mask;


    // Start is called before the first frame update
    void Start()
    {
        //pos = transform.position;
        isDragging = false;
        hitObject = false;
        lastPosX = 0f;
        lastPosZ = 0f;
        selected = null;
    }

    // Update is called once per frame
    void Update()
    {
        Camera editorModeCamera = GameObject.FindGameObjectWithTag("EditorModeCamera").GetComponent<Camera>();
        GameObject switchModeButton = GameObject.Find("SwitchModeButton");
        ModeHandler modeHandler = switchModeButton.GetComponent<ModeHandler>();
        if (Input.GetMouseButton(0) && modeHandler.Mode.Equals("EditorMode"))
        {
            Ray ray = editorModeCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, dragable))
            {
                if (hit.collider != null && hitObject == false)
                {
                    selected = hit.collider.gameObject;
                    isDragging = true;
                    hitObject = true;
                }
            }
        }
        if (isDragging)
        {
            Ray ray2 = GameObject.FindGameObjectWithTag("EditorModeCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit2;
            //                  (ray, hit, range, mask)
            if (Physics.Raycast(ray2, out hit2, Mathf.Infinity, mask))
            {
                //get position (x,y,z) from click
                float posX = hit2.point.x;
                float posZ = hit2.point.z;

                //   Debug.Log("x: " + posX + "z: " + posZ);

                //=> erhält maus position
                //Updated Posx!=x, only when mouse is moving 
                if (lastPosX != posX || lastPosZ != posZ)
                {
                    lastPosX = posX;
                    lastPosZ = posZ;
                    //    Debug.Log("x: " + posX + "z: " + posZ);

                    //Cursor
                    selected.transform.position = new Vector3(posX, 0f, posZ);
                    //Objekte fliegen auf kamera zu => add layer floor -> boden hinzufügen und am würfel entfernen
                }


                //When space
                if (Input.GetKeyDown("space"))
                {
                    selected.transform.rotation = selected.transform.rotation * Quaternion.AngleAxis(45, Vector3.up);
                    // hit.collider.transform.rotation = *Quaternion.AngleAxis(90, Vector3.up);
                }

            }
            if (Input.GetKeyDown("return"))
            {
                hitObject = false;
                isDragging = false;
            }
        }

    }

}




