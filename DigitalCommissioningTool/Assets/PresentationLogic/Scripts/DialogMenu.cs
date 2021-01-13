using ApplicationFacade.Warehouse;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogMenu : MonoBehaviour
{
    public TMP_InputField AmountInput;
    [HideInInspector]
    public ItemData SelectedItem;
    public int Amount;

    public delegate void AmountConfirmed(DialogMenu dialog);
    public event AmountConfirmed AmountConfirmedEvent;
    /// <summary>
    /// Überprüft, ob genügend Items verfügbar sind und gibt ein Event aus, falls dies der Fall ist
    /// </summary>
    public void OnClick()
    {
        try
        {
            int result = Int32.Parse(AmountInput.text);
            if (SelectedItem.Count >= result)
            {
                Amount = result;
                AmountConfirmedEvent(this);
                gameObject.SetActive(false);
            }
            else
            {
                AmountInput.text = SelectedItem.Count.ToString();
                Debug.LogWarning("Nicht genügend Inventar vorhanden");
            }
        }
        catch (FormatException)
        {
            Debug.LogWarning("Eingabe ist nicht zulässig");
        }
    }
}
