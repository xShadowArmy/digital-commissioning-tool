using ApplicationFacade.Warehouse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WarehouseItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public WarehouseItemMenu WarehouseItemMenu;
    public Material SelectionMaterial;
    public Material TransparentMaterial;
    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeMaterial(SelectionMaterial);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeMaterial(TransparentMaterial);
    }
    private void ChangeMaterial(Material material)
    {
        ItemData rootItem = WarehouseItemMenu.GetStockItem(transform.GetSiblingIndex()-1);
        foreach (ItemData item in rootItem.ChildItems)
        {
            if (item.Object != null)
            {
                Renderer highlightBox = item.Object.transform.Find("InvisibleWall").GetComponent<Renderer>();
                highlightBox.material = material;
            }
        }

    }
}
