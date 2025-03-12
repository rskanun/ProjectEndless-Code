using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private TalkController controller;
    [SerializeField] private TextManager textManager;
    [SerializeField] private SelectManager selectManager;

    [Space]
    // 현재 라인 진행 상황
    [SerializeField] private bool isPrinting;
    [SerializeField] private bool isTalking;
    [SerializeField] private int lineNum;

    // Select 관련 변수
    private Stack<Select> selectStack;

    /************************************************************
    * [대사 관리]
    * 
    * 대사를 읽어 그에 따른 인게임 이벤트 제어
    ************************************************************/

    public void StartTalk(Npc npc)
    {
        // 대화 처음 시작 시 해당되는 대화목록 가져오기
        List<Line> lines = npc.GetLines();
        selectStack = new Stack<Select>();

        // 대화 전용 컨트롤러로 변경
        ControlContext.Instance.SetController(controller);

        isTalking = true;
        StartCoroutine(ReadLines(lines));
    }

    private IEnumerator ReadLines(List<Line> lines)
    {
        while (lineNum < lines.Count)
        {
            // 대사 출력
            PrintLine(lines[lineNum]);

            // 대사를 출력하는 동안 대기
            yield return new WaitUntil(() => !isPrinting);

            // 다음 대사 출력
            lineNum++;
        }

        // 대사를 모두 읽었다면 대화 멈추기
        EndTalk();
    }

    public void OnTalkHandler()
    {
        if (isTalking)
        {
            // 대화 중이면 해당 대화를 스킵
            OnSkipTalk();
        }
    }

    private void OnSkipTalk()
    {
        // 선택창이 띄워진 경우 패스
        if (selectManager.IsSelectOpen) return;

        if (textManager.IsPrinting)
        {
            // 텍스트가 출력 중인 경우 한 번에 출력
            textManager.TextSkip();
        }
        else
        {
            // 텍스트 출력이 끝난 경우 다음 대화 출력
            NextTalk();
        }
    }

    private void NextTalk()
    {
        isPrinting = false;
    }

    private void EndTalk()
    {
        // 대사 읽기에 쓰이는 변수 초기화
        isPrinting = false;
        isTalking = false;
        lineNum = 0;

        // 대화창 UI 끄기
        textManager.CloseDialogue();

        // 초기 컨트롤러로 변경
        ControlContext.Instance.ResetController();
    }

    /************************************************************
    * [라인 출력 관리]
    * 
    * 라인을 읽고서 거기에 따른 인게임 이벤트 제어
    ************************************************************/

    private void PrintLine(Line line)
    {
        switch (line.Code)
        {
            case LineType.Text:
                PrintTextLine((TextLine)line);
                break;

            case LineType.Select:
                ActiveSelection((Select)line);
                break;

            case LineType.Case:
                SkipToEnd(); // End로 스킵
                break;

            case LineType.Event:
                ActiveEvent((EventLine)line);
                break;

            default:
                break;
        }
    }

    private void PrintTextLine(TextLine line)
    {
        isPrinting = true;

        textManager.PrintText(line);
    }

    private void ActiveSelection(Select line)
    {
        isPrinting = true;

        selectStack.Push(line);
        selectManager.OpenSelect(line, OptionSelect);
    }

    public void OptionSelect(string option)
    {
        Select select = selectStack.Peek();
        int skipLineNum = select.OptionsLineNum[option];

        JumpTo(skipLineNum);
    }

    private void SkipToEnd()
    {
        Select select = selectStack.Pop();
        int skipLineNum = select.EndLineNum;

        JumpTo(skipLineNum);
    }

    private void JumpTo(int num)
    {
        lineNum = num;
        isPrinting = false;
    }

    private void ActiveEvent(EventLine line)
    {
        // 이벤트 일시 사용 중지
        string command = line.Command;
        // eventManager.GetCommandEvent(command);
    }
}