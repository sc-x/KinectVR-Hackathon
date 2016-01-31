using UnityEngine;
using System.Collections;

public class cubeDetect : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		touch visionDetect = GetComponent<touch>();
		if (visionDetect.getTouch()) { 
			this.transform.Rotate (1, 1, 1);
		}
	}
}
