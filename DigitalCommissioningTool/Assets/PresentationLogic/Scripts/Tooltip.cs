using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Tooltip : MonoBehaviour
{

    public TextMeshProUGUI tooltipHeader;
    public TextMeshProUGUI tooltipContent;
    public Image tooltip;
    [SerializeField] public Button AddButton;
    [SerializeField] public Button RemoveButton;


    private void Start()
    {
        tooltip.gameObject.SetActive(false);
    }
    /// <summary>
    /// Setzten der TextFelder Im Tooltip 
    /// </summary>
    /// <param name="content">Inhalt des Textkörpers im Tooltip, wie Anzahl und Gewicht der Items im ReferenzObjekt, welches das Tooltip beschreibt </param>
    /// <param name="header">Art der Kiste, die von Tooltip beschrieben wird </param>
    ///  <param name="updated">wird gesetzt, wenn nur die anzahl in den Kisten verringert wird </param>
    public void SetTooltip(string content, string header, bool updated)
    {
        SetText(content, header);

        if (!updated)
        {
            tooltip.gameObject.SetActive(true);
            Vector2 position = Input.mousePosition;
            tooltip.transform.position = position;
        }

    }
    /// <summary>
    /// Deaktiviert Tooltip
    /// </summary>
    public void RemoveTooltip()
    {
        tooltip.gameObject.SetActive(false);
        
    }
    /// <summary>
    ///Aktiviert Tooltip und setzt den Inhalt 
    /// </summary>
    /// <param name="content">Informationen über die Itemsin Kisten(Referenzobjekt)</param>
    /// <param name="header">Art der Kiste</param>
    void SetText(string content, string header)
    {
        tooltipHeader.text = header;
        tooltipContent.text = content;

    }
}
