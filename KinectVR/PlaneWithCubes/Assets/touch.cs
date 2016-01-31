using UnityEngine;
using System.Collections;

public class touch : MonoBehaviour {

	// Use this for initialization
	bool doesTouch = false;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool getTouch() { 
		return doesTouch;
	}
}
