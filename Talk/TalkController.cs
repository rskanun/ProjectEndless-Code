using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private TextManager textManager;
    [SerializeField] private SelectManager selectManager;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private PlayerController playerController;

    // 현재 라인 진행 상황
    private bool readLock;
    private bool isTalking;
    private int lineNum;

    // Select 관련 변수
    private Stack<Select> selectStack;

    /************************************************************
    * [대화키]
    * 
    * 대사를 읽어 그에 따른 인게임 이벤트 제어
    ************************************************************/

    public void OnControlKeyPressed()
    {
        if (Input.GetButtonDown("Talking"))
        {
            OnReadHandler();
        }
    }

    public void OnReadHandler()
    {
        if (isTalking && !selectManager.IsSelectOpen)
        {
            if (textManager.IsPrinting) textManager.TextSkip();
            else readLock = false;
        }
        else if(!isTalking)
        {
            EndTalk();
        }
    }

    private void EndTalk()
    {
        // reset value
        readLock = false;
        lineNum = 0;

        // dialog ui off
        textManager.TextDestroy();

        ControlContext.Instance.SetState(playerController);
    }

    /************************************************************
    * [대사 관리]
    * 
    * 대사를 읽어 그에 따른 인게임 이벤트 제어
    ************************************************************/

    public void StartTalk(Npc npc)
    {
        if (npc.isInteractive())
        {
            // 대화 처음 시작 시 해당되는 대화목록 가져오기
            List<Line> lines = npc.getLines();
            selectStack = new Stack<Select>();

            isTalking = true;
            StartCoroutine(ReadLines(lines));
        }
        else
        {
            // 상호작용이 불가능한 npc일 경우
            EndTalk();
        }
    }

    private IEnumerator ReadLines(List<Line> lines)
    {
        while (lineNum < lines.Count)
        {
            if (readLock == false)
            {
                Line line = lines[lineNum++];
                ProcessLine(line);
            }

            yield return null;
        }

        isTalking = false;
    }

    /************************************************************
    * [라인 출력 관리]
    * 
    * 라인을 읽고서 거기에 따른 인게임 이벤트 제어
    ************************************************************/

    private void ProcessLine(Line line)
    {
        switch (line.Code)
        {
            case LineType.Text:
                ProcessTextLine((TextLine)line);
                break;

            case LineType.Select:
                ProcessSelect((Select)line);
                break;

            case LineType.Case:
                ProcessCase(); // End로 스킵
                break;

            case LineType.Event:
                ProcessEventLine((EventLine)line);
                break;

            default:
                break;
        }
    }

    private void ProcessTextLine(TextLine line)
    {
        readLock = true;

        textManager.PrintText(line);
    }

    private void ProcessSelect(Select line)
    {
        readLock = true;

        selectStack.Push(line);
        selectManager.OpenSelect(line, optionSelect);
    }

    public void optionSelect(string option)
    {
        Select select = selectStack.Peek();
        int skipLineNum = select.OptionsLineNum[option];

        JumpLine(skipLineNum);
    }

    private void ProcessCase()
    {
        Select select = selectStack.Pop();
        int skipLineNum = select.EndLineNum;

        JumpLine(skipLineNum);

        readLock = false;
    }

    private void JumpLine(int num)
    {
        lineNum = num + 1; // case나 end 제외
        readLock = false;
    }

    private void ProcessEventLine(EventLine line)
    {
        string command = line.Command;
        eventManager.getCommandEvent(command);
    }
}