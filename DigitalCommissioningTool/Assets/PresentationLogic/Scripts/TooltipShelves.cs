using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;




namespace ApplicationFacade
{
    public class TooltipShelves : MonoBehaviour
    {
        [SerializeField] SelectionManager selectionManager;
        [SerializeField] Image current;
        public event EventHandler ShelveSelected;
        public TextMeshProUGUI header;
        public TextMeshProUGUI content;
        public LayoutElement layoutElement;
        public int characterWrapLimit;
        private Tooltip tooltip;
        private string headerMessage = "New Text";
        private string contentMessage = "New Text";


        void Start()
        {

            selectionManager.ShelveSelected += SelectionManager_ShelveSelected;
            tooltip = current.GetComponent<Tooltip>();
        }

        private void SelectionManager_ShelveSelected(GameObject selectedObject)
        {
           
                Debug.Log("Selected!");
                headerMessage = selectedObject.name;
                activate(selectedObject);
            

        }




        void Update()
        {
            if (Application.isEditor)
            {
                int headerLenght = header.text.Length;
                int contentLength = content.text.Length;

                if ((headerLenght > characterWrapLimit || contentLength > characterWrapLimit))

                    layoutElement.enabled = true;
                else
                    layoutElement.enabled = false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                deactivate();
            }
            
        }

        public void activate(GameObject selectedObject)
        {
            getMessages(selectedObject);

            tooltip.SetTooltip(headerMessage, contentMessage);
        }



        public void deactivate()
        {
            tooltip.RemoveTooltip();
        }

        private void getMessages(GameObject selectedObject)
        {
            //ItemData list = GameManager.GameWarehouse.GetStorageRackItem(selectedObject);
            //Debug.Log(list.Count);
            System.Random rnd = new System.Random();
            int anzahl = rnd.Next(1, 13);
            string count = "Anzahl: " + anzahl +"\n";
            contentMessage = count;
            contentMessage += "Gewicht: " + anzahl * (rnd.Next(1, 2) - 0.4f) + " kg";
        }


    }

}

