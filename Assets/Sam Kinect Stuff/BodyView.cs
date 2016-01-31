using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;

public class BodyView : MonoBehaviour {

    //This is responsible for rendering the body data, and eventually applying the skeletal data to a mecanim rig.


    public Material boneMaterial;
    

    //Dictionary of Body GameObjects by body ID
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodyManager bodyManager;
    private GameObject bodyManagerGO;

    //This maps all the bones by the two joints they will de connected to
    private Dictionary<JointType, JointType> _boneMap = new Dictionary<JointType, JointType>
    {
    { JointType.FootLeft, JointType.AnkleLeft },
    { JointType.AnkleLeft, JointType.KneeLeft },
    { JointType.KneeLeft, JointType.HipLeft },
    { JointType.HipLeft, JointType.SpineBase },

    { JointType.FootRight, JointType.AnkleRight },
    { JointType.AnkleRight, JointType.KneeRight },
    { JointType.KneeRight, JointType.HipRight },
    { JointType.HipRight, JointType.SpineBase },

    { JointType.HandTipLeft, JointType.HandLeft }, //Need this for HandSates
    { JointType.ThumbLeft, JointType.HandLeft },
    { JointType.HandLeft, JointType.WristLeft },
    { JointType.WristLeft, JointType.ElbowLeft },
    { JointType.ElbowLeft, JointType.ShoulderLeft },
    { JointType.ShoulderLeft, JointType.SpineShoulder },

    { JointType.HandTipRight, JointType.HandRight }, //Needthis for Hand State
    { JointType.ThumbRight, JointType.HandRight },
    { JointType.HandRight, JointType.WristRight },
    { JointType.WristRight, JointType.ElbowRight },
    { JointType.ElbowRight, JointType.ShoulderRight },
    { JointType.ShoulderRight, JointType.SpineShoulder },

    { JointType.SpineBase, JointType.SpineMid },
    { JointType.SpineMid, JointType.SpineShoulder },
    { JointType.SpineShoulder, JointType.Neck },
    { JointType.Neck, JointType.Head },
    };


    // Use this for initialization
    void Start () {
        bodyManagerGO = gameObject;
        

	}

    public bool leftHandClosed, rightHandClosed;
    public Vector3 leftHandPos, rightHandPos;

    float offsetY;

    // Update is called once per frame
    void Update () {
        int state = 0;

        //If we don't have a body manager Game Object
        if( bodyManagerGO == null )
        {
            //Exit the function
            return;
        }

        //Try to get the body manager game object's BodyManager component
        bodyManager = bodyManagerGO.GetComponent<BodyManager>();

        //If the body manager game object doesn't have a BodyManager component
        if (bodyManager == null)
        {
            //Exit the function
            return;
        }

        //Create a local copy of the bodyManager data
        Body[] data = bodyManager.getData();

        //If that data doesn't exist
        if( data == null )
        {
            //Exit the function
            return;
        }


        //List of tracked body ID numbers
        List<ulong> trackedIDs = new List<ulong>();


        //Fill that list
        //For each body in Kinect FOV
        foreach ( var body in data )
        {
            //If the body data is empty
            if( body == null )
            {
                //Skip this entry
                continue;
            }

            //If the body is being tracked
            if( body.IsTracked )
            {
                //Add its ID to the trackedIDs list
                trackedIDs.Add(body.TrackingId);
            }
        }

        //Get list of all body gameobjec
        List<ulong> knownIDs = new List<ulong>(_Bodies.Keys);

        //Delete GameObjects for bodies that aren't being tracked
        foreach ( ulong trackingID in knownIDs )
        {
            //If the currently tracked body ID list doesn't contain a body with this tracking ID
            if( !trackedIDs.Contains(trackingID) )
            {
                //Delete the GameObject
                Destroy(_Bodies[trackingID]);
                //Remove the body from the list of gameObjects
                _Bodies.Remove(trackingID);
            }
        }


        //Update body objects that need to be updated and create body objects that need to be created

        foreach ( var body in data )
        {
            //If the body doesn't exist
            if( body == null )
            {
                //Skip it
                continue;
            }

            //If the body is tracked
            if( body.IsTracked )
            {
                //If the body GameObject dictionary doesn't contain the body's tracking ID
                if( !_Bodies.ContainsKey(body.TrackingId) )
                {
                    //Create a body object with thr body's tracking ID and add it to the body GameObject dictionary (using the tracking ID as a key)
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }

                //Refresh the body object
                RefreshBodyObject(body, _Bodies[body.TrackingId]);
            }
        }
	}

    GameObject CreateBodyObject( ulong trackingID )
    {
        // Create a new body gameObject
        GameObject body = new GameObject("Body: " + trackingID);

        

        //This loop goes through every joint and draws the body
        for( JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++ )
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            jointObj.GetComponent<MeshRenderer>().enabled = false;
            jointObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            
            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.material = boneMaterial;
            lr.SetVertexCount(2);
            lr.SetWidth(0.2f, 0.2f);

            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;

            //To enable collision with UI Objects
            Rigidbody rb = jointObj.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;


        }

        return body;
    }


    //public Transform upperArm, forearm;
    //Refresh Body Skeleton (represented by cubes)
    public GameObject oculus;
    void RefreshBodyObject( Body body, GameObject bodyObject )
    {

        for( JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++ )
        {
            Windows.Kinect.Joint sourceJoint = body.Joints[jt];
            Windows.Kinect.Joint? targetJoint = null;
            if( _boneMap.ContainsKey(jt) )
            {
                targetJoint = body.Joints[_boneMap[jt]];
            }

            Transform jointObj = bodyObject.transform.FindChild(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            if (targetJoint.HasValue)
            {
                LineRenderer lr = jointObj.GetComponent<LineRenderer>();
                lr.SetPosition(0, jointObj.localPosition);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
            }

            //Hand Position Stuff
            if( jointObj.name == "HandLeft") { leftHandPos = jointObj.transform.position;  }
            if (jointObj.name == "HandRight") { rightHandPos = jointObj.transform.position; }
            if (jointObj.name == "Head") { oculus.transform.position = jointObj.transform.position; } 

            //Fix Oculus Rot
            //oculus.transform.RotateAround(Vector3.up, 180);

           

        }

        //Hand Gesture Stuff
        leftHandClosed = (body.HandLeftState == HandState.Closed);
        rightHandClosed = (body.HandRightState == HandState.Closed);

        
    }


    private static Vector3 GetVector3FromJoint( Windows.Kinect.Joint joint )
    {
        return new Vector3(joint.Position.X, joint.Position.Y, joint.Position.Z*-1)*2;
    }

    private static Quaternion GetRotationForJoints( Windows.Kinect.Joint startJoint, Windows.Kinect.Joint endJoint )
    {
        Vector3 startJointPos = new Vector3(startJoint.Position.X, startJoint.Position.Y, startJoint.Position.Z);
        Vector3 endJointPos = new Vector3(endJoint.Position.X, endJoint.Position.Y, endJoint.Position.Z);

        //transform.position = startJointPos;
        Quaternion rawRot = Quaternion.LookRotation((endJointPos - startJointPos).normalized);

        //return Quaternion.Euler(rawRot.x, rawRot.y, rawRot.z
        return rawRot;

    }



    
}
