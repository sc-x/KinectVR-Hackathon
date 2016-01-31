using UnityEngine;
using System.Collections;

public class Button1 : MonoBehaviour {
    UIObject uiObject;
    public GameObject ball;
	// Use this for initialization
	void Start () {
        uiObject = GetComponent<UIObject>();
	}
	
	// Update is called once per frame
	void Update () {
        print(uiObject.leftHandInObject);
        if ( (uiObject.leftHandInObject && uiObject.leftHandJustClosed) || (uiObject.righthandInObject && uiObject.rightHandJustClosed))
        {
            Instantiate(ball, transform.position, Quaternion.identity);
        }
	}
}
