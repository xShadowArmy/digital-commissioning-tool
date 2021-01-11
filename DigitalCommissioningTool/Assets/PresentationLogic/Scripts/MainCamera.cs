using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCamera : MonoBehaviour
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
            this.transform.Rotate(new Vector3(0,
                Input.GetAxis("Mouse X") * Sensitivity));

            //Fix Z-Axis
            float z = transform.eulerAngles.z;
            this.transform.Rotate(0, 0, -z);
        }

    }
}
