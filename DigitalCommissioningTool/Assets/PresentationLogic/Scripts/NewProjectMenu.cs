using ApplicationFacade;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProjectMenu : MonoBehaviour
{
    public GameObject InputField;
    // Start is called before the first frame update
    void Start()
    {
        ResourceHandler.ReplaceResources();
    }
    public void Back()
    {
        gameObject.SetActive(false);
    }
    public void Continue()
    {
        GameManager.CreateProject(InputField.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text);
        GameManager.GameWarehouse.CreateWall(new Vector3(1f, 2f, 3f), Quaternion.Euler(4f, 5f, 6f), new Vector3(7f, 8f, 9f));
        GameManager.GameWarehouse.CreateDoor(new Vector3(1f, 2f, 3f), Quaternion.Euler( 4f, 5f, 6f), new Vector3(77f, 88f, 99f), DoorType.Gate);
        GameManager.GameWarehouse.CreateStorageRack();
        StorageData data0 = GameManager.GameWarehouse.CreateStorageRack();
        GameManager.GameWarehouse.CreateStorageRackItem(new Vector3(0f, 0f, 0f), Quaternion.Euler( 0f, 0f, 0f), new Vector3(0f, 0f, 0f), data0);

        StorageData data = GameManager.GameContainer.CreateContainer(new Vector3(0f, 0f, 0f), Quaternion.Euler( 0f, 0f, 0f), new Vector3(0f, 0f, 0f));
        ItemData item = GameManager.GameContainer.CreateContainerItem(new Vector3(0f, 0f, 0f), Quaternion.Euler( 0f, 0f, 0f), new Vector3(0f, 0f, 0f), data);

        item.SetPosition(new Vector3(1234567f, 1234567f, 1234567f));

        gameObject.SetActive(false);
        gameObject.transform.parent.Find("MainPanel").gameObject.SetActive(false);

        //gameObject.transform.parent.Find("MainPanel").GetComponent<MainMenu>().LoadDefaultScene();
    }
}
