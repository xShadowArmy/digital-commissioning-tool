using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Tooltip : MonoBehaviour
{

    public TextMeshProUGUI tooltipHeader;
    public TextMeshProUGUI tooltipContent;
    public Image tooltip;


    private void Start()
    {
        tooltip.gameObject.SetActive(false);
    }

    public void SetTooltip(string content, string header)
    {
        SetText(content, header);

        tooltip.gameObject.SetActive(true);
        Vector2 position = Input.mousePosition;
        tooltip.transform.position = position;

    }

    public void RemoveTooltip()
    {
        tooltip.gameObject.SetActive(false);
    }

    void SetText(string content, string header)
    {
        tooltipHeader.text = content;
        tooltipContent.text = header;

    }
}
