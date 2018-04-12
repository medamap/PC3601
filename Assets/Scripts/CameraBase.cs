using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBase : MonoBehaviour {

    public enum CameraType {
        Sphere,
        Cylynder
    }

    public float hmin = 0f;
    public float hmax = 1f;
    public float hstep = 0.1f;
    public float vmin = 0f;
    public float vmax = 1f;
    public float vstep = 1f;

}
