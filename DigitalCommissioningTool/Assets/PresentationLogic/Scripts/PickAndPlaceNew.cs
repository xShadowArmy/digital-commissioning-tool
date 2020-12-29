using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApplicationFacade;


public class PickAndPlaceNew : MonoBehaviour
{
//    public LayerMask dragable;
    public GameObject selected;     //= ausgewähltes Regal
    public bool isDragging;         //= true wenn ein Regal ausgewählt
    bool hitObject;                 //keinObject bewegen wenn schon eins ausgewählt
    float lastPosX;                 
    float lastPosZ;
    //Vector3 mousePos;
    public LayerMask mask;
    bool moveX, moveZ;              

    int rotationRight;
    int rotationLeft;
    int rotation;
    public int collision;
    public bool onObject;
    //public Material[] material;
    public Material material1, material2;
    Renderer rend;
    Transform invisibleWall;
//    private static int temp;



    // Start is called before the first frame update
    void Start()
    {
        //pos = transform.position;
        isDragging = false;
        hitObject = false;
        lastPosX = 0f;
        lastPosZ = 0f;
        rotationRight = 0;
        rotationLeft = 0;
        rotation = 0;
        // temp = 1;
        invisibleWall = null;
        onObject = false;
        selected = null;

        SelectionManager.StorageSelected += OnStorageSelected;
    }

    // Update is called once per frame
    void Update()
    {


        if (isDragging)
        {

            invisibleWall = selected.transform.Find("InvisibleWall");
            rend = invisibleWall.GetComponent<Renderer>();
            //rend.sharedMaterial = material[0];
            //temp = AddStorage.objectNumber;
            hitObject = true;
            HitSomething();

            Debug.Log(selected);

            Rotate(selected);


            if (!moveX && !moveZ)
            {
                MoveAnywhere(selected);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                moveX = true;
                moveZ = false;
            }
            if (moveX)
            {
                MoveInXAxis(selected, rotation);
            }
       
            if (Input.GetKeyDown(KeyCode.Y))
            {
                moveX = false;
                moveZ = true;
            }
            if (moveZ)
            {
                MoveInYAxis(selected, rotation);
            }
                
            if (Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Backspace)) {
                DeleteObject(selected);
            }


        }
    }

    //Auswahl Regal :
    private void OnStorageSelected(Transform storage)
    {
        selected = storage.gameObject;  
        isDragging = true;

    }

    //Regal rotieren
    private void Rotate(GameObject selected) {

        //rotation
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.E))
        {
            //selected.transform.rotation = Quaternion.identity;
            // selected.transform.Rotate(new Vector3(0,45,0));
            selected.transform.rotation = selected.transform.rotation * Quaternion.AngleAxis(90, Vector3.up);

            /*if (temp == 1) { selected.transform.Rotate(new Vector3(0, 90, 0)); }
            if (temp == 2) { selected.transform.Rotate(new Vector3(0, 30, 0)); }
            if (temp == 3) { selected.transform.rotation = selected.transform.rotation * Quaternion.AngleAxis(90, Vector3.up); }
            if (temp == 4) { selected.transform.rotation = selected.transform.rotation * Quaternion.AngleAxis(45, Vector3.up); }*/

            //selected.transform.Rotate(Vector3.up * 180 * Time.deltaTime);

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

    }

    //Bewegen mit Maus
    private void MoveAnywhere(GameObject selected) {
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

            PlaceObject();

        }

    }

    //Regal Platzieren 
    private void PlaceObject() {
        if (Input.GetKeyDown("return"))
        {
            if (HitSomething() == true)
            {
                Debug.Log("Cant Place on Object");
                //rend.sharedMaterial = material[1];
            }
            else
            {
              //  rend.sharedMaterial = material[2];
                hitObject = false;
                moveX = false;
                moveZ = false;
                isDragging = false;
            }
        }
    }

    //In X-Richtung Bewegen
    private void MoveInXAxis(GameObject selected, int rotation) {

    float x = 0;
    //Debug.Log("bewegt in x Richtung");

    float speed = 2f;
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||Input.GetKey(KeyCode.RightArrow) ||Input.GetKey(KeyCode.LeftArrow))
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
        pos = new Vector3(0, 0, -(x * Time.deltaTime));
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
        PlaceObject();

}

    //In Y-Richtung Bewegen
    private void MoveInYAxis(GameObject selected, int rotation) {

        float z = 0;
        // Debug.Log("bewegt in y Richtung");

        float speed = 2f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
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
        PlaceObject();
    }

    //Regal löschen
    private void DeleteObject(GameObject selected) {
        Debug.Log("Delete " + selected.name);
        GameManager.GameWarehouse.RemoveStorageRack(GameManager.GameWarehouse.GetStorageRack(selected));
        //Destroy(selected);
        isDragging = false;
    }

    //Anzahl Kollisionen bestimmen
    private int CheckCollision() {

        Collider[] hitColliders = Physics.OverlapBox(selected.transform.position, selected.transform.localScale / 2, selected.transform.rotation);
        int i = 0;
        foreach (Collider collider in hitColliders)
        {
            //Output all of the collider names
            //  Debug.Log("Hit : " + collider.name + " " + i);
            //Increase the number of Colliders in the array
            if (collider.gameObject != selected)
            {
                i++;
                // hitObject = true;
            }
            else
            {
                i--;
            }
         //   Debug.Log(" i : " + i);
        }
        return i;

    }

    // Gibt An ob Kollision vorhanden is 
    private bool HitSomething() {
        collision = this.CheckCollision();
        if (collision > 3)                      // collision = 3 wenn auf keinem Objekt 
        {
            onObject = true;
            rend.material = material2;
        }
        else
        {
            onObject = false;
            rend.material = material1;
        }
        
        return onObject;
    }
}






