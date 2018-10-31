using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogController : MonoBehaviour {

    public TextMeshProUGUI log;
    public InputField In;
    public ScrollRect scroll;
    public BlinkingCursor BlinkingCursor;
    public JukeboxController JukeboxController;

    private bool blip = true;

    private int slow = 50;
    private int slower = 120;
    private bool readingDone = false;
    private bool printing = false;

    private void Start()
    {

    }

    private void scrollToEnd()
    {
        StartCoroutine(delayScroll());   
    }

    private IEnumerator delayScroll()
    {
        yield return new WaitForSeconds(0.01f);
        scroll.normalizedPosition = new Vector2(0, 0);
    }

    private void playBlip()
    {
        if(blip)
            JukeboxController.playSound(JukeboxController.TEXT_BLIP);
        blip = !blip;
    }

    //Set caret to end of input
    private void SetCaret()
    {
        In.Select();
        In.ActivateInputField();
        In.selectionAnchorPosition = In.text.Length;
        In.selectionFocusPosition = In.text.Length;
        In.caretPosition = In.text.Length;
    }

    public void onValueChange()
    {
        if(!In.text.EndsWith(""+BlinkingCursor.caret))
            BlinkingCursor.hideCaret();
    }

    //DETECT SPECIFIC INPUT
    public void onFinishEdit()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            if (!readingDone)
            {
                readingDone = true;
            }
            else
            {
                In.text = "";
            }
            scrollToEnd();
        }
        SetCaret();
    }

    //READ
    public void Read(Action<string> callback)
    {
        In.text = "";
        SetCaret();
        StartCoroutine(
            BlockingRead(
                (res)=>{
                    res = res.Replace(""+BlinkingCursor.caret, ""
                        );
                    callback(res);
                    In.text = "";
                    scrollToEnd();
                }
            )
        );
    }

    private IEnumerator BlockingRead(Action<string> callback)
    {
        readingDone = false;
        yield return new WaitUntil(() => readingDone);
        readingDone = false;
        callback(In.text);
    }

    // PRINT
    public void Print(string toPrint)
    {
        StartCoroutine(QueueAppendJob(toPrint, UIColors.DEFAULT_TEXT, 0));
    }

    public void Println(string toPrint)
    {
        Print(toPrint + "\n");
    }

    //PRINT COLOR
    public void PrintColored(string toPrint, int color)
    {
        StartCoroutine(QueueAppendJob(toPrint, color, 0));
    }

    public void PrintlnColored(string toPrint, int color)
    {
        PrintColored(toPrint+"\n", color);
    }

    //ITEM PRINT
    public void ItemPrint(Item i)
    {
        StartCoroutine(QueueAppendJob(i.getName(), i.getColor(), 0));
    }

    public void ItemPrintln(Item i)
    {
        StartCoroutine(QueueAppendJob(i.getName()+"\n", i.getColor(), 0));
    }

    //SLOW PRINT
    public void SlowPrint(string toPrint)
    {
        StartCoroutine(QueueAppendJob(toPrint, UIColors.DEFAULT_TEXT, slow));
    }

    public void SlowPrintln(string toPrint)
    {
        SlowPrint(toPrint + "\n");
    }

    //SLOW PRINT COLOR
    public void SlowPrintColored(string toPrint, int color)
    {
        StartCoroutine(QueueAppendJob(toPrint, color, slow));
    }

    public void SlowPrintlnColored(string toPrint, int color)
    {
        SlowPrintColored(toPrint + "\n", color);
    }

    //SLOWER PRINT
    public void SlowerPrint(string toPrint)
    {
        StartCoroutine(QueueAppendJob(toPrint, UIColors.DEFAULT_TEXT, slower));
    }

    public void SlowerPrintln(string toPrint)
    {
        SlowerPrint(toPrint + "\n");
    }

    //SLOWER PRINT COLOR
    public void SlowerPrintColored(string toPrint, int color)
    {
        StartCoroutine(QueueAppendJob(toPrint, color, slower));
    }

    public void SlowerPrintlnColored(string toPrint, int color)
    {
        SlowerPrintColored(toPrint + "\n", color);
    }

    //DELAY PRINT
    public void DelayPrint(string toPrint, int delay)
    {
        StartCoroutine(QueueAppendJob(toPrint, UIColors.DEFAULT_TEXT, delay));
    }

    public void DelayPrintln(string toPrint, int delay)
    {
        DelayPrint(toPrint+"\n", delay);
    }
       
    //DELAY PRINT COLOR
    public void DelayPrintColored(string toPrint, int color, int delay)
    {
        StartCoroutine(QueueAppendJob(toPrint, color, delay));
    }

    public void DelayPrintlnColored(string toPrint, int color, int delay)
    {
        DelayPrintColored(toPrint+"\n", color, delay);
    }

    //BASELINE APPEND LOGIC
    private IEnumerator QueueAppendJob(string toPrint, int color, int delay)
    {
        scrollToEnd();
        if (printing)
        {
            yield return new WaitUntil(()=>!printing);
        }
        printing = true;
        StartCoroutine(Append(toPrint, color, delay));

    }

    private IEnumerator Append(string toPrint, int color, int delay)
    {
        if (toPrint.Length == 0) {
            printing = false;
            scrollToEnd();
            yield break;
        }

        float msDelay = ((float)delay) / 1000f;

        if (color == UIColors.DEFAULT_TEXT)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(msDelay);
                log.text += toPrint[0];
                if(toPrint[0] != ' ')
                    playBlip();
                scrollToEnd();
                StartCoroutine(Append(toPrint.Substring(1), color, delay));
            }
            else
            {
                log.text += toPrint;
                scrollToEnd();
                printing = false;
                yield break;
            }
        }
        else
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(msDelay);
                log.text += string.Format("<#{0}>{1}</color>", color.ToString("X").PadLeft(6, '0'), toPrint[0]);
                if (toPrint[0] != ' ')
                    playBlip();
                scrollToEnd();
                StartCoroutine(Append(toPrint.Substring(1), color, delay));
            }
            else
            {
                log.text += string.Format("<#{0}>{1}</color>", color.ToString("X").PadLeft(6, '0'), toPrint);
                printing = false;
                scrollToEnd();
                yield break;
            }
        }
    }

}
