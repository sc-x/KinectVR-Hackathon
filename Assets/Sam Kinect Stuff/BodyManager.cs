using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class BodyManager : MonoBehaviour {

    private KinectSensor sensor;
    private BodyFrameReader reader;
    private Body[] data = null;

    

    public Body[] getData()
    {
        return data;
    }


	// Use this for initialization
	void Start () {

        print("START");
        /*Get Kinect Sensor and start reading data*/

        //Get Default Kinect Sensor
        sensor = null;
        sensor = KinectSensor.GetDefault();

        if( sensor != null )
        {
            //We have a sensor connected

            print("SENSOR CONNECTED");
            //Open the connection/Start reading the data
            reader = sensor.BodyFrameSource.OpenReader();
            if( !sensor.IsOpen )
            {
                sensor.Open();
            }
            

        } else
        {
            print("NO KINECT CONNECTED");
        }

        print(sensor);
	}
	
	// Update is called once per frame
	void Update () {
        /*Get Read Skeletal Data and Apply it to Cubes*/

        //if we have our data reader
        if ( reader != null )
        {
            //Get the latest frame data
            var frame = reader.AcquireLatestFrame();
            if( frame != null )
            {
                //We have the last frame's data

                //Setup data if we're null
                if( data == null )
                {
                    /*
                    Create an array of body data with a length of how many bodies are on the screen.
                    We'll always acces data[0] because we only have one person with the rift.
                    Maybe in the future we could implement other people being able to enter your virtual world but they wouldn't be wearing the rift.
                    */
                    data = new Body[sensor.BodyFrameSource.BodyCount];
                }

                //Get and refresh data from the reader
                frame.GetAndRefreshBodyData(data);
                frame.Dispose();
                frame = null;

                

            }
        }
        
	
	}

    //When we quit the application
    void OnApplicationQuit()
    {
        //If we have a data reader
        if( reader != null )
        {
            //Dispose of it
            reader.Dispose();
            reader = null;
        }

        //If we have a sensor
        if( sensor != null )
        {

            //If a connection is open, close it
            if( sensor.IsOpen )
            {
                sensor.Close();
            }

            //Get rid of the reference
            sensor = null;
        }
        
    }
}
