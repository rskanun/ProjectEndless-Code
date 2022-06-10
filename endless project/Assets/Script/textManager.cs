using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class textManager : MonoBehaviour
{
    private Dictionary<int, string[]> textData;

    // Object Class
    eventManager eventM;

    // Game Object
    Text textLine;
    GameObject textDialogue;

    // Variable
    private int textLineNum;
    private int lineCnt;

    private string[] lines;

    private float typingSpeed;
    private float setTypingSpeed;

    private bool isTalking = false;

    private void Awake()
    {
        textData = new Dictionary<int, string[]>();
        initText();
    }
    public void init(Text textLine, GameObject textDialogue)
    {
        this.textLine = textLine;
        this.textDialogue = textDialogue;
    }

    public bool talk(objectManager objData)
    {
        if (textLineNum == 0)
        {
            initTalk(objData);
            return true;
        }

        if (textLineNum <= (lines.Length - 1))
        {
            talking();
            return true;
        }

        else
        {
            initTalk(objData);
            return false;
        }
            
    }

    private void initTalk(objectManager objData)
    {
        if (!isTalking)
        {
            textLine.gameObject.SetActive(true);
            textDialogue.gameObject.SetActive(true);

            lines = getText(objData.id);

            isTalking = true;
        }

        else
        {
            textLineNum = 0;
            lineCnt = 0;
            textLine.text = "";

            textLine.gameObject.SetActive(false);
            textDialogue.gameObject.SetActive(false);

            isTalking = false;
        }
    }

    private void talking()
    {
        string str = lines[textLineNum];

        if (str.FirstOrDefault().Equals("/"))
        {
            eventM.getEvent(str);
            return;
        }

        if (lineCnt == 0)
        {
            typingSpeed = setTypingSpeed;
            textLine.text = "";
            StartCoroutine(chatDelay(lines[textLineNum]));

        }

        else if (lineCnt < str.Length)
        {
            typingSpeed = 0;
        }
    }
    IEnumerator chatDelay(string str)
    {
        while (lineCnt < str.Length)
        {
            textLine.text = str.Substring(0, lineCnt + 1);
            lineCnt++;

            yield return new WaitForSeconds(typingSpeed);

        }

        lineCnt = 0;
        textLineNum++;
    }

    private void initText()
    {
        textData.Add(1, new string[]
        {
            "네가 이곳의 주인공이구나?",
            "그거 알아? \r\n사실 이곳은 가상세계야."
        });

        textData.Add(2, new string[]
        {
            "표지판에 박힌 가시에 찔렸다."
        });
    }

    public string[] getText(int id)
    {
        return textData[id];
    }
}
