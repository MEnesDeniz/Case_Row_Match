using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToGoUp : MonoBehaviour
{
    private Camera _cam;
    private Transform _transform;
    private ButtonToGoDown buttonToGoDown;

    public void Setup()
    {
        buttonToGoDown = FindObjectOfType<ButtonToGoDown>();
    }

    private void OnMouseDown()
    {
        _cam = Camera.main;
        _transform = _cam.transform;

        var offset = _transform.position;
        if(offset.y <= 1)
        {
            Camera.main.transform.position = new Vector3(offset.x, offset.y + 1f, offset.z);

            var offset2 = this.transform.position;
            transform.position = new Vector3(offset2.x, offset2.y + 1f, offset2.z);

            var offset3 = buttonToGoDown.transform.position;
            buttonToGoDown.transform.position = new Vector3(offset3.x, offset3.y + 1f, offset3.z);
        }

    }
}
