using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using ApplicationFacade.Application;
using ApplicationFacade.Warehouse;

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

    private void SelectionManager_ShelveSelected(GameObject selectedObject, bool active)
    {


        if (active)
        {
            ItemData item = GameManager.GameWarehouse.GetStorageRackItem(selectedObject);

            headerMessage = item.Name;
            getMessages(selectedObject, item );
            activate();

        }
        else
        {
            deactivate();
        }


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

    public void activate()
    {

        tooltip.SetTooltip(headerMessage, contentMessage);
    }



    public void deactivate()
    {
        tooltip.RemoveTooltip();
    }

    private void getMessages(GameObject selectedObject, ItemData item)
    {
        contentMessage = "Anzahl: " + item.Count + "\n";
        contentMessage += "Gewicht: " + item.Weight + " kg";

    }


}