﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class Timer : MonoBehaviour
{
    public float currentTime;
    public TMP_Text textBox;
    public bool clicked = false;
    private bool isHovered = false;
    private float holdKeyTime = 4.0f;
    private float keyTime;
    [SerializeField] private TMP_Text timerBtn;
    [SerializeField] private Button resetBtn;
    [SerializeField] private ModeHandler mode;
    [SerializeField] private GameObject panelTimer;
    [SerializeField] private GameObject panelTimeMeasure;

    public delegate void TimerEventHandler(float currentTime, string buttonText);

    public static event TimerEventHandler TimerStopped;
    
    public static event TimerEventHandler TimerReset;
    

    // Start is called before the first frame update
    void Start()
    {
        resetBtn.gameObject.SetActive(false);
        timerBtn.transform.parent.gameObject.SetActive(false);
        panelTimeMeasure.SetActive(false);
    }

    void Update()
    {
        //Modusbedingte Verfügbarkeit
        if (ModeHandler.Mode.Equals("EditorMode"))
        {

            panelTimer.SetActive(false);
        }
        else
        {
            panelTimer.SetActive(true);
            //Timer
            if (clicked)
            {
                currentTime += Time.deltaTime;
                textBox.text = currentTime.ToString("F2");
            }
            if (Input.GetKeyDown(KeyCode.P))
            {

            }


            //Tasten Steuerung
            if (isHovered)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {

                    onClick();
                }
            }
        }



    }
    /// <summary>
    /// Wird von Play Button referenziert und steuert ´die Bennenung des Buttons
    /// löst TimerStopped Event aus
    /// </summary>
    public void onClick()
    {
        clicked = !clicked;
        timerBtn.text = clicked ? "Stop" : "Resume";
        if (timerBtn.text.Equals("Resume") && currentTime > 0)
        {
            TimerStopped?.Invoke(currentTime, timerBtn.text);
        }
    }
    /// <summary>
    /// Wird von Reset Button referenziert und setzt den Timer wieder auf Null
    /// löst das TimerReset event aus
    /// </summary>
    public void Reset()
    {
        TimerReset?.Invoke(currentTime, timerBtn.text);
        currentTime = 0;
        textBox.text = currentTime.ToString("F2");
        timerBtn.text = "Start";
        clicked = false;
    }
    /// <summary>
    /// Macht Buttons sichtbar
    /// </summary>
    public void setVisible()
    {
        textBox.text = currentTime.ToString("F2");
        resetBtn.gameObject.SetActive(true);
        timerBtn.transform.parent.gameObject.SetActive(true);
        isHovered = !isHovered;                                     //bool Value für den Taste P
        
    }
    /// <summary>
    /// Deaktiviert und macht Button unsichtbar
    /// </summary>
    public void setInisible()
    {
        if (textBox.text.Equals("0.00"))
        {
            textBox.text = currentTime.ToString("00:00");

        }
        resetBtn.gameObject.SetActive(false);
        timerBtn.transform.parent.gameObject.SetActive(false);
        isHovered = !isHovered;                                     //bool Value für den Taste P
       

    }
}
