using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextHolder : MonoBehaviour
{
    public string text;

    public Text[] textItems;

    // Update is called once per frame
    void Update()
    {
        foreach (Text item in textItems)
        {
            item.text = text;
        }
    }
}
