using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAndPlaceNew : MonoBehaviour
{
    public LayerMask dragable;
    GameObject selected;
    bool isDragging;
    bool hitObject; //keinObject bewegen wenn schon eins ausgewählt

    // Start is called before the first frame update
    void Start()
    {
        //pos = transform.position;
        isDragging = false;
        hitObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, dragable))
            {
                if (hit.collider != null && hitObject == false)
                {
                    selected = hit.collider.gameObject;
                    isDragging = true;
                    Debug.Log("hit");
                    hitObject = true;
                }
            }
            if (isDragging)
            {
                Vector3 pos = mousePos();
                selected.transform.position = pos;
            }
        }
        else { isDragging = false; hitObject = false; }

        Vector3 mousePos()
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        }
    }
}

