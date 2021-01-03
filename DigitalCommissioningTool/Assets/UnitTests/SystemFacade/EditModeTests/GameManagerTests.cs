using System.IO;
using SystemFacade;
using NUnit.Framework;
using UnityEngine;
using ApplicationFacade.Application;
using ApplicationFacade.Warehouse;

namespace UnitTests.SystemFacade
{
    public class GameManagerTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void creates_projects()
        {
            string projectName = "TestProject3490580298541";
            string projectPath = Paths.ProjectsPath + projectName + ".prj";
            GameManager.CreateProject(projectName);
            //GameManager.GameWarehouse.CreateWall(new Vector3(1f, 2f, 3f), Quaternion.Euler(4f, 5f, 6f), new Vector3(7f, 8f, 9f));
            //GameManager.GameWarehouse.CreateDoor(new Vector3(1f, 2f, 3f), Quaternion.Euler( 4f, 5f, 6f), new Vector3(77f, 88f, 99f), DoorType.Door, );
            GameManager.GameWarehouse.CreateStorageRack();
            StorageData data0 = GameManager.GameWarehouse.CreateStorageRack();
            //GameManager.GameWarehouse.CreateStorageRackItem(new Vector3(0f, 0f, 0f), Quaternion.Euler( 0f, 0f, 0f), new Vector3(0f, 0f, 0f), data0);

            StorageData data = GameManager.GameContainer.CreateContainer(new Vector3(0f, 0f, 0f),  Quaternion.Euler(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
            //ItemData item = GameManager.GameContainer.CreateContainerItem(new Vector3(0f, 0f, 0f), Quaternion.Euler( 0f, 0f, 0f), new Vector3(0f, 0f, 0f), data);

            //item.SetPosition(new Vector3(1234567f, 1234567f, 1234567f));

            GameManager.SaveProject(projectName);
            GameManager.CloseProject();

            Assert.IsTrue(File.Exists(projectPath) && new FileInfo(projectPath).Length > 0);
            GameManager.DeleteProject(projectName);
        }

        [Test]
        public void loads_and_restores_projects()
        {
            string projectName = "TestProject3490580298545";
            string projectPath = Paths.ProjectsPath + projectName + ".prj";
            GameManager.CreateProject(projectName);
            //GameManager.GameWarehouse.CreateWall(new Vector3(1f, 2f, 3f), Quaternion.Euler(4f, 5f, 6f), new Vector3(7f, 8f, 9f));
            //GameManager.GameWarehouse.CreateDoor(new Vector3(1f, 2f, 3f), Quaternion.Euler( 4f, 5f, 6f), new Vector3(77f, 88f, 99f), DoorType.Door);
            GameManager.GameWarehouse.CreateStorageRack();
            StorageData origData1 = GameManager.GameWarehouse.CreateStorageRack();
            //GameManager.GameWarehouse.CreateStorageRackItem(new Vector3(0f, 0f, 0f), Quaternion.Euler( 0f, 0f, 0f), new Vector3(0f, 0f, 0f), origData1);

            StorageData origData2 = GameManager.GameContainer.CreateContainer(new Vector3(0f, 0f, 0f), Quaternion.Euler( 0f, 0f, 0f), new Vector3(0f, 0f, 0f));
            //ItemData item = GameManager.GameContainer.CreateContainerItem(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f), new Vector3(0f, 0f, 0f), origData2);

            //item.SetPosition(new Vector3(1234567f, 1234567f, 1234567f));

            GameManager.SaveProject(projectName);
            GameManager.CloseProject();

            GameManager.LoadProject(projectName);

            StorageData loadedData1 = GameManager.GameWarehouse.GetStorageRack(origData1.GetID());
            StorageData loadedData2 = GameManager.GameContainer.GetContainer(origData2.GetID());

            GameManager.CloseProject();
            GameManager.DeleteProject(projectName);

            bool same = origData1.Position == loadedData1.Position && origData1.Rotation == loadedData1.Rotation && origData1.Scale == loadedData1.Scale &&
                        origData2.Position == loadedData2.Position && origData2.Rotation == loadedData2.Rotation && origData2.Scale == loadedData2.Scale;
            Assert.IsTrue(same);
            
        }
    }
}