using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TouchTest : MonoBehaviour
{

    public bool debugMode = false;
    public Text screenText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (debugMode)
            screenText.text = string.Format("Touches Detected: {0}",
                Input.touchCount);
    }
}
