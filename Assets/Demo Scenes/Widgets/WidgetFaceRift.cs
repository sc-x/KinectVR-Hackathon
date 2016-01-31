using UnityEngine;
using System.Collections;

public class WidgetFaceRift : MonoBehaviour {
    Transform OculusCam;
	// Use this for initialization
	void Start () {
        OculusCam = GameObject.FindGameObjectWithTag("Oculus").transform;

	}
	
	// Update is called once per frame
	void Update () {
        Quaternion originalRot = transform.rotation;
        transform.LookAt(OculusCam);
        transform.RotateAround(transform.right, 90);
	}
}
