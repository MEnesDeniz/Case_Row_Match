using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToGoDown : MonoBehaviour
{
    private Camera _cam;
    private Transform _transform;
    private ButtonToGoUp buttonToGoUp;
    private float LevelNum;

    public void Setup(float Levelnum)
    {
        this.LevelNum = Levelnum;
        buttonToGoUp = FindObjectOfType<ButtonToGoUp>();
    }
    private void OnMouseDown()
    {
        _cam = Camera.main;
        _transform = _cam.transform;
        var offset = _transform.position;
        if(offset.y >(LevelNum*-1.7f))
        {
            Camera.main.transform.position = new Vector3(offset.x, offset.y - 1f, offset.z);

            var offset2 = this.transform.position;
            transform.position = new Vector3(offset2.x, offset2.y - 1f, offset2.z);

            var offset3 = buttonToGoUp.transform.position;
            buttonToGoUp.transform.position = new Vector3(offset3.x, offset3.y - 1f, offset3.z);
        }
   
    }
}
