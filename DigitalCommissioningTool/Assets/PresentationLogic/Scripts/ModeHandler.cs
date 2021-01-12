using System.Collections;
using System.Collections.Generic;
using ApplicationFacade.Application;
using UnityEngine;
using UnityEngine.UI;
using Camera = UnityEngine.Camera;

public class ModeHandler : MonoBehaviour
{
    public GameObject EditorModeCamera;
    public GameObject MosimCamera;
    public SelectionManager SelectionManager;
    [SerializeField]private Sprite ModiSwitchBuild;
    [SerializeField] private Sprite ModiSwitchMosim;
    private bool ModeSwitchButton = false;
    private bool ShiftPressed = false;
    private int Frame = 0;

    /// <summary>
    /// Stellt den aktuellen Modus ("EditorMode" oder "MosimMode") dar. 
    /// </summary>
    public string Mode { get; private set; }

    
        public void SwitchMode()
    {
        EditorModeCamera.GetComponent<Camera>().enabled = !EditorModeCamera.GetComponent<Camera>().enabled;
        MosimCamera.GetComponent<Camera>().enabled = !MosimCamera.GetComponent<Camera>().enabled;

        if (EditorModeCamera.GetComponent<Camera>().enabled)
        {
            Mode = "EditorMode";
            this.GetComponent<Image>().sprite  = ModiSwitchBuild;
            Debug.Log("EditorMode");
        }
        else
        {
            Mode = "MosimMode";
            this.GetComponent<Image>().sprite = ModiSwitchMosim;
            Debug.Log("MosimMode");
        }
        SelectionManager.ResetSelection();
    }

    // Start is called before the first frame update
    void Start()
    {
        Mode = "MosimMode";
        MosimCamera.GetComponent<Camera>().enabled = true;
        EditorModeCamera.GetComponent<Camera>().enabled = false;
    }

    // Wechselt den M
    void Update()
    {
        Frame += 1;

        if ( Frame == 30 )
        {
            Frame = 0;

            if ( ModeSwitchButton )
            {
                if ( KeyManager.ChangeMode.ShiftNeeded )
                {
                    if ( ShiftPressed )
                    {
                        SwitchMode( );
                    }
                }

                else
                {
                    SwitchMode( );
                }

                ModeSwitchButton = false;
                ShiftPressed = false;
            }
        }

        else
        {
            if ( Input.GetKeyDown( KeyManager.ChangeMode.Code ) )
            {
                ModeSwitchButton = true;
            }

            if ( Input.GetKeyDown( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift ) )
            {
                ShiftPressed = true;
            }
        }
    }
}
