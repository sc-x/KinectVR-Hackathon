using UnityEngine;
using System.Collections;

public class MiscFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) { UnityEngine.VR.InputTracking.Recenter();  }
	}
}
