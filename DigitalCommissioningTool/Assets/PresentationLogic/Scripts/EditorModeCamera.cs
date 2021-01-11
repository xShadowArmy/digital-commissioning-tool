﻿using System.Collections;
using System.Collections.Generic;
using ApplicationFacade.Application;
using UnityEngine;

public class EditorModeCamera : MonoBehaviour
{
    [SerializeField] private float Sensitivity = 1.0f;
    private float VerticalAxis { get; set; }
    private float HorizontalAxis { get; set; }
    private static bool _rightClicked;
    private static float _speed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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