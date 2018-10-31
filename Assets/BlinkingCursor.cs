using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingCursor : MonoBehaviour {

    private InputField input;
    private float m_timeStamp;
    private bool showCursor = false;
    public char caret = '█';

    void Start()
    {
        m_timeStamp = Time.time;
        input = GetComponent<InputField>();
        if (input == null)
        {
            //error
            throw new System.Exception("Unable to load input field");
        }
    }

    // Update is called once per frame
    void Update () {
        if (Time.time - m_timeStamp >= 0.5)
        {
            m_timeStamp = Time.time;
            if (input.text.Contains("" + caret))
            {
                hideCaret();
            }
            else
            {
                input.text += caret;
            }
        }
    }

    public void hideCaret()
    {
        input.text = input.text.Replace(caret, '\0');
    }
}

