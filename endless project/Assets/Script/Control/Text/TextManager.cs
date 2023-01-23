using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    // 참조 스크립트
    private EventManager command;
    private TextUI ui;

    // 텍스트 데이터 저장
    private Dictionary<int, string[]> textData;

    // EventManager가 있는 오브젝트
    public GameObject gameManager;

    // 텍스트 현재 진행 상태
    private bool isTalking = false;
    public bool IsTalking { get { return isTalking; } }
    private int lineNum;
    private int lineCnt;

    // 텍스트 저장 공간
    private string[] lines;

    // 출력 속도
    private float typingSpeed;

    /************************************************************
    * [초기 설정]
    * 
    * CSV 파일로부터 대사 가져오기 및 다른 스크립트 연결
    ************************************************************/

    private void Start()
    {
        // 텍스트 데이터 삽입
        textData = new Dictionary<int, string[]>();
        init();
    }

    private void init()
    {
        // Component Init
        command = gameManager.GetComponent<EventManager>();
        ui = GetComponent<TextUI>();

        // text input
        initText();
    }

    private void initText()
    {
        List<string> lines = CSVReader.getLines();

        // 텍스트를 정리할 dummy list
        List<string> strs = new List<string>();
        // 텍스트 코드를 기억할 dummy int
        int beforeNum = 0;

        // 파일 끝까지 한 줄씩 읽기
        foreach (string str in lines)
        {
            // 해당 줄의 스크립트와 코드 분리
            string num = str.Split(',')[0];
            string line = str.Split(',')[1];

            // 새로운 넘버가 출현했을 경우
            if (!string.IsNullOrEmpty(num))
            {
                // 이전 넘버가 있다면
                if (beforeNum != 0)
                {
                    // 이전 넘버 데이터에 텍스트가 정리된 list 추가
                    textData.Add(beforeNum, strs.ToArray());
                    strs.Clear();
                }

                beforeNum = int.Parse(num);
            }

            // 줄바꿈 인식
            line = line.Replace("\\r\\n", "\r\n");
            // 앞에 넘버가 붙는 것에 관계없이 대사를 배열에 추가
            strs.Add(line);
        }

        // 아직 남은 대사가 배열안에 있을 경우 전부 대사 데이터에 저장
        if(!(strs is null))
        {
            textData.Add(beforeNum, strs.ToArray());
            strs.Clear();
        }
    }

    /************************************************************
    * [대화 출력]
    * 
    * 인게임 화면의 대화 제어
    ************************************************************/


    public void initTalk(NPC npc)
    {
        // 현재 대사 번호 리셋
        lineNum = 0;

        // 대화 가능한 npc일 경우
        if (npc.getID() != 0)
        {
            // 대화 처음 시작 시 해당되는 대화목록 가져오기
            lines = textData[npc.getID()];

            // 대화 진행상태로 변경
            isTalking = true;
        }
    }

    public void talking()
    {
        // 한 대사를 모두 출력시 그 대사 종료
        if (lineCnt >= lines[lineNum].Length)
        {
            lineCnt = 0;
            lineNum++;

            // 텍스트 창 비활성화
            ui.setDialogView(false);
        }

        // 대화 진행
        if (lineNum < lines.Length)
        {
            // 대사 가져오기
            char[] line = lines[lineNum].ToCharArray();

            // 그 대사가 커맨드일 경우 이벤트 출력
            if (line[0] == '/')
            {
                command.getCommandEvent(lines[lineNum]);
                lineNum++;

                talking();
            }

            // 대사 출력
            else
                printText(line);
        }

        // 대화 종료
        else
        {
            // 텍스트 창 비활성화
            ui.setDialogView(false);

            // 대화 종료상태로 변경
            isTalking = false;
        }
    }
    private void printText(char[] line)
    {
        // 한 글자도 출력이 안 됐을 경우
        if (lineCnt == 0)
        {
            // 텍스트 창 활성화 및 타이핑 속도 리셋
            ui.setDialogView(true);
            typingSpeed = Option.getTypingSpeed();

            // 지정된 타이핑 속도로 출력
            StartCoroutine(talkDelay(line));
        }

        // 대화 출력 도중일 경우
        else if (lineCnt < line.Length)
        {
            // 한 번에 출력
            typingSpeed = 0;
        }
    }

    IEnumerator talkDelay(char[] line)
    {
        WaitForSeconds wait = new WaitForSeconds(typingSpeed);

        // 대화 진행 도중일 경우
        while (lineCnt < line.Length)
        {
            // 한 글자씩 대화를 출력
            ui.setText(splitString(line, lineCnt++));

            yield return wait;
        }
    }

    // 길이만큼의 문자열 자르기
    private string splitString(char[] chrs, int length)
    {
        string result = "";

        for (int i = 0; i < length; i++)
        {
            result += chrs[i];
        }

        return result;
    }
}
