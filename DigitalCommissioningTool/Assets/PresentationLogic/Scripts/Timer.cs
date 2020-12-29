using System.Collections;
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
    
    public delegate void TimerEventHandler(float currentTime);

    public static event TimerEventHandler TimerStopped;
    
    public static event TimerEventHandler TimerReset;
    

    // Start is called before the first frame update
    void Start()
    {
        resetBtn.gameObject.SetActive(false);
        timerBtn.transform.parent.gameObject.SetActive(false);

    }

    void Update()
    {
        //Modusbedingte Verfügbarkeit
        if (mode.Mode.Equals("EditorMode"))
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
    //Wird von Play Button referenziert
    public void onClick()
    {
        clicked = !clicked;
        timerBtn.text = clicked ? "Stop" : "Resume";
        if (timerBtn.text.Equals("Resume") && currentTime > 0)
        {
            TimerStopped?.Invoke(currentTime);
        }
    }

    //Wird von Rest Button referenziert
    public void Reset()
    {
        currentTime = 0;
        textBox.text = currentTime.ToString("F2");
        timerBtn.text = "Start";
        clicked = false;
        TimerReset?.Invoke(currentTime);
    }

    //Macht Buttons sichtbar
    public void setVisible()
    {
        textBox.text = currentTime.ToString("F2");
        resetBtn.gameObject.SetActive(true);
        timerBtn.transform.parent.gameObject.SetActive(true);
        isHovered = !isHovered;                                     //bool Value für den Taste P
    }

    //Deaktiviert und macht Button unsichtbar
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
