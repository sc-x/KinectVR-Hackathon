using UnityEngine;
using System.Collections;

public class CalendarSelect : MonoBehaviour {
    UIObject uiObject;
    float color = 0;
    public float fadeRate = 10;
    Material mat;
	// Use this for initialization
	void Start () {
        uiObject = GetComponent<UIObject>();
        mat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        if ((uiObject.handInObject))
        {
            color = 255;
        }

        color -= fadeRate * Time.deltaTime;
        if( color < 0 ) { color = 0; }
        print(color);

        mat.SetColor("_TintColor", new Color(255,255,255,color/255));
	}
}
