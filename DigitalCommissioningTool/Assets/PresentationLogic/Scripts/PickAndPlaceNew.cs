using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAndPlaceNew : MonoBehaviour
{
    int count = 0;                                          //Zähler um beim drehen die bewegungssteuerung anpassen 
    void move()
    {
        //Bewegen
        int a = 5;                                      //=Geschwindigkeit

        //Debug.Log("hold!");
        // GetComponent<Rigidbody>().useGravity = false;         
        float x = Input.GetAxis("Horizontal") * a;          //Horizontale Eingabe (w,s)
        float y = Input.GetAxis("Vertical") * a;            //Vertikale Eingabe (a,d)
        Vector3 pos = transform.position;                   //Ausgangsposition
        if (count == 1)
        {
            //Debug.Log("count: " + count);
            pos = new Vector3(-(y * Time.deltaTime), 0, x * Time.deltaTime);            //einmal gedreht x,y anpassen sodass links=links, usw. ...
        }
        else if (count == 0)
        {
            // Debug.Log("count: " + count);
            pos = new Vector3(x * Time.deltaTime, 0, y * Time.deltaTime);               //ohne drehung 
        }//Bewegen mit tasten(wasd oder pfeiltasten)
         //  Vector3 pos = new Vector3(xDirection, yDirection, 0);                     //Bewegen mit Maus
        else if (count == 2)
        {
            {
                //  Debug.Log("count: " + count);
                pos = new Vector3(-(x * Time.deltaTime), 0, -(y * Time.deltaTime));            //x,y anpassen 
            }
        }
        else if (count == 3)
        {
            {
                //Debug.Log("count: " + count);
                pos = new Vector3((y * Time.deltaTime), 0, -(x * Time.deltaTime));            //x,y anpassen
            }
        }
        transform.Translate(pos);                                                              //Bewegung
    }

    void turn()
    {
        //drehen
        count++;                                            //zähler beim drehen erhöhen                       
        if (count == 4)
        {                                     //da es sich wiederholt zähler wieder auf 0 setzen
            count = 0;
        }
        transform.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);   //um 90°drehen
    }

    // Start is called before the first frame update
    void Start()
    {
        //pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))                            //links Mausklick (gehalten)
        {
            move();
        }
        /*  else
          {
              //       Debug.Log("released!");
              GetComponent<Rigidbody>().useGravity = true;
          }*/

        if (Input.GetKeyDown("space"))                          //bei space
        {
            turn();
        }
    }
}

