using System.Collections;
using System.Collections.Generic;
using ApplicationFacade.Application;
using ApplicationFacade.Warehouse;
using UnityEngine;

public class EditorModeCamera : MonoBehaviour
{
    [SerializeField] private float Sensitivity = 1.0f;
    private float VerticalAxis { get; set; }
    private float HorizontalAxis { get; set; }
    private static bool _rightClicked;
    private static float _speed;
    private bool StorageKeyPressed = false;
    private bool WallKeyPressed = false;
    private bool ShiftPressed = false;
    private int Frame = 0;
    Transform spawn;

    // Start is called before the first frame update
    void Start()
    {
        spawn = GameManager.GameWarehouse.ObjectSpawn.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Frame += 1;

        if ( Frame == 30 )
        {
            Frame = 0;

            if ( StorageKeyPressed )
            {
                if ( KeyManager.InsertStorageReck.ShiftNeeded )
                {
                    if ( ShiftPressed )
                    {
                        GameManager.GameWarehouse.CreateStorageRack( spawn.position, spawn.rotation, spawn.localScale );
                    }
                }

                else
                {
                    GameManager.GameWarehouse.CreateStorageRack( spawn.position, spawn.rotation, spawn.localScale );
                }

                StorageKeyPressed = false;
                ShiftPressed = false;
            }            
            
            if ( WallKeyPressed )
            {
                if ( KeyManager.InsertStorageReck.ShiftNeeded )
                {
                    if ( ShiftPressed )
                    {
                        GameManager.GameWarehouse.CreateWall( new Vector3( spawn.position.z, 1.6f, spawn.position.z ), spawn.rotation, new Vector3( 1f, 3.2f, 0.2f ), WallFace.Undefined, WallClass.Inner, "SelectableInnerWall" );
                    }
                }

                else
                {
                    GameManager.GameWarehouse.CreateWall( new Vector3( spawn.position.z, 1.6f, spawn.position.z ), spawn.rotation, new Vector3( 1f, 3.2f, 0.2f ), WallFace.Undefined, WallClass.Inner, "SelectableInnerWall" );
                }

                WallKeyPressed = false;
                ShiftPressed = false;
            }
        }

        else
        {
            if ( Input.GetKeyDown( KeyManager.InsertStorageReck.Code ) )
            {
                StorageKeyPressed = true;
            }

            if ( Input.GetKeyDown( KeyManager.InsertWall.Code ) )
            {
                WallKeyPressed = true;
            }

            if ( Input.GetKeyDown( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift ) )
            {
                ShiftPressed = true;
            }
        }

        _rightClicked = Input.GetMouseButton(1);

        if (_rightClicked)
        {
            this.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * Sensitivity,
                Input.GetAxis("Mouse X") * Sensitivity));

            //Fix Z-Axis
            float z = transform.eulerAngles.z;
            this.transform.Rotate(0, 0, -z);
            
            VerticalAxis = Input.GetAxis("Vertical");
            HorizontalAxis = Input.GetAxis("Horizontal");

            if (_speed < 15 && (VerticalAxis != 0 || HorizontalAxis != 0))
            {
                _speed += 0.1f;
            }
            else if (VerticalAxis == 0 && HorizontalAxis == 0)
            {
                _speed = 0;
            }

            this.transform.Translate(new Vector3(HorizontalAxis, 0, VerticalAxis) * (Time.deltaTime * _speed));

            if ( transform.position.y <= 2 )
            {
                transform.position = new Vector3( transform.position.x, 2f, transform.position.z );
            }
        }

        if ( Input.GetKey( KeyManager.MoveCameraUp.Code ) )
        {
            transform.Translate( new Vector3( 0, 5f, 0 ) * Time.deltaTime );
        }

        else if ( Input.GetKey( KeyManager.MoveCameraDown.Code ) )
        {
            if ( transform.position.y >= 2.5f )
            {
                transform.Translate( new Vector3( 0, -5f, 0 ) * Time.deltaTime );
            }
        }
    }
}