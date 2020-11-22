using ApplicationFacade;
using NUnit.Framework;
using UnityEngine;

namespace UnitTests.SystemFacade
{
    public class GameManagerTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void creates_project()
        {
            string projectName = "TestProject3490580298539";
            GameManager.CreateProject(projectName);
            GameManager.GameWarehouse.CreateWall(new Vector3(1f, 2f, 3f), new Vector3(4f, 5f, 6f), new Vector3(7f, 8f, 9f));
            GameManager.GameWarehouse.CreateDoor(new Vector3(1f, 2f, 3f), new Vector3(4f, 5f, 6f), new Vector3(77f, 88f, 99f), DoorType.Gate);
            GameManager.GameWarehouse.CreateStorageReck(new Vector3(11f, 222f, 333f), new Vector3(4f, 5f, 6f), new Vector3(7f, 8f, 9f));
            StorageData data0 = GameManager.GameWarehouse.CreateStorageReck(new Vector3(1f, 2f, 3f), new Vector3(433f, 555f, 666f), new Vector3(7f, 8f, 9f));
            GameManager.GameWarehouse.CreateStorageReckItem(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), data0);

            StorageData data = GameManager.GameContainer.CreateContainer(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
            ItemData item = GameManager.GameContainer.CreateContainerItem(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), data);

            item.SetPosition(new Vector3(1234567f, 1234567f, 1234567f));

            GameManager.SaveProject(projectName);
            GameManager.CloseProject();
            Assert.IsTrue(GameManager.OpenProject(projectName));
            
            
        }
    }
}