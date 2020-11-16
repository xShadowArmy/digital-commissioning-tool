using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material selectedMaterial;
    private Transform SelectedObject { get; set;}
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit))
        {
            
            Transform tempObject = hit.transform;
            if (tempObject.CompareTag("SelectableWall"))
            {
                if (SelectedObject != null)
                {
                    SelectedObject.Find("InvisibleWall").transform.GetComponent<Renderer>().material = defaultMaterial;
                }
                SelectedObject = tempObject;
                Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                invisibleWall.GetComponent<Renderer>().material = selectedMaterial;
            }
            else if(SelectedObject != null)
            {
                Transform invisibleWall = SelectedObject.Find("InvisibleWall").transform;
                invisibleWall.GetComponent<Renderer>().material = defaultMaterial;
            }
        }
    }
}
