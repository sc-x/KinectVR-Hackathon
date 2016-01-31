using UnityEngine;
using System.Collections;

public class EnableWidget : MonoBehaviour {
    UIObject uiObject;
    public GameObject widget;

    Vector3 originalScale;
    float targetScale = 0;
    // Use this for initialization
    void Start()
    {
        uiObject = GetComponent<UIObject>();
        originalScale = widget.transform.localScale;
        widget.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        print(uiObject.leftHandInObject);
        if ((uiObject.leftHandInObject) || (uiObject.righthandInObject))
        {

            targetScale = 1f;
            
            
        } else
        {
            targetScale = 0;
        }

        widget.transform.localScale = Vector3.Lerp(widget.transform.localScale, originalScale*targetScale, 0.8f);

    }
}
