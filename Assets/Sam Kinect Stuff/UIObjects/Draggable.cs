using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {
    UIObject uiObj;

    Vector3 leftOffset, rightOffset;
    // Use this for initialization
    void Start () {
        uiObj = GetComponent<UIObject>();
        if( uiObj == null )
        {
            print("No UIObject Component for " + gameObject.name);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Testing Stuff
        if (uiObj.leftHandInObject && uiObj.leftHandJustClosed)
        {
           leftOffset = uiObj.viewer.leftHandPos - transform.position;
        }
        if (uiObj.righthandInObject && uiObj.rightHandJustClosed)
        {
            rightOffset = uiObj.viewer.rightHandPos - transform.position;
        }


        if ((uiObj.leftHandInObject && uiObj.viewer.leftHandClosed) && !uiObj.righthandInObject)
        {
            //Vector3 offset = 

            transform.position = uiObj.viewer.leftHandPos;

        }

        if ((uiObj.righthandInObject && uiObj.viewer.rightHandClosed) && !uiObj.leftHandInObject)
        {
            transform.position = uiObj.viewer.rightHandPos;
        }
    }
}
