using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallEditor : MonoBehaviour
{
    public GameObject popUp;
    [HideInInspector] public GameObject selectedObject;
    private SelectionManager selectWall;

    public Text myText;
    string s = "test";

    // Start is called before the first frame update

    void Start()
    {
        selectedObject = GameObject.Find("SelectionManager");
        selectWall = selectedObject.GetComponent<SelectionManager>();
        popUp.SetActive(selectWall.selected);

    }


    public void SetPopUp(string s)
    {
        string wand = null;

        switch (s)
        {
            case "WallNord":
                wand = "Nordwand";
                break;
            case "WallEast":
                wand = "Ostwand";

                break;
            case "WallSouth":
                wand = "Südwand";

                break;
            case "WallWest":
                wand = "Westwand";

                break;
            default:
                myText.text = "Geben Sie die gewünschte Länge von der Südwand ein";
                break;

        }
        myText.text = "Geben Sie die gewünschte Länge von der " + wand + " ein";
        popUp.SetActive(selectWall.selected);



    }

    public void close()
    {
        popUp.SetActive(false);
    }
}
