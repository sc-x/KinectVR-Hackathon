using UnityEngine;
using System.Collections;

public class UIObject : MonoBehaviour {
    public bool handInObject, leftHandInObject, righthandInObject;
    public BodyView viewer;

    public bool leftHandJustClosed, rightHandJustClosed;

    private bool lastLeftHandClosed, lastRightHandClosed;

   
    

   
	// Use this for initialization
	void Start () {
        viewer = GameObject.FindObjectOfType<BodyView>().GetComponent<BodyView>();

        GetComponent<Collider>().isTrigger = true;
        //print(viewer);


	}
	
	// Update is called once per frame
	void Update () {
        if( leftHandJustClosed ) { leftHandJustClosed = false; }
        if( rightHandJustClosed ) { rightHandJustClosed = false; }

        if( viewer.rightHandClosed != lastRightHandClosed )
        {
            rightHandJustClosed = true;
        }
        if (viewer.leftHandClosed != lastLeftHandClosed)
        {
            leftHandJustClosed = true;
        }

        handInObject = (leftHandInObject || righthandInObject);

        

        lastLeftHandClosed = viewer.leftHandClosed;
        lastRightHandClosed = viewer.rightHandClosed;

    }

    void OnTriggerEnter( Collider other )
    {
        //print(other.gameObject.name);
        if (other.gameObject.name == "HandRight")
        {
            righthandInObject = true;
        }

        if (other.gameObject.name == "HandLeft")
        {
            leftHandInObject = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "HandRight")
        {
            righthandInObject = false;
        }

        if (other.gameObject.name == "HandLeft")
        {
            leftHandInObject = false;
        }
    }
}
