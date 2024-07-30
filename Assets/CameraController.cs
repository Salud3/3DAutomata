using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Slider zoom;

    public void Zoom()
    {
        Camera.main.fieldOfView = zoom.value;
    }


}
