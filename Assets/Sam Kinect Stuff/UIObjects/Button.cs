using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
    UIObject uiObject;

	// Use this for initialization
	void Start () {
        uiObject = GetComponent<UIObject>();
	}
	
	// Update is called once per frame
	void Update () {
        print(uiObject.leftHandInObject);
        if ( uiObject.leftHandInObject && uiObject.leftHandJustClosed )
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }
	}
}
