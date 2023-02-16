using Assets.Script.Control.Text.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Control.Text
{
    public enum Code
    {
        Text,   // 대사출력                  -> 대사번호,코드(Text),이름,대사
        Select, // 선택지                    -> 대사번호,코드(Select),선택1,선택2,...,선택n
        Case,   // 선택지 선택에 따른 진행   -> 대사번호,코드(Case),선택지
        End,    // 선택지 종료 선언          -> 대사번호,코드(End)
        Event   // 이벤트(수치 조작 등) 발생 -> 대사번호,코드(Event),명령어
    }

    public class TextManager : MonoBehaviour
    {
        // 참조 스크립트
        public EventManager command;
        public TextUI ui;

        // 텍스트 데이터 저장
        private Dictionary<int, List<Line>> lineData;

        // 텍스트 현재 진행 상태
        private bool readLock;
        private bool isTalking = false;
        public bool IsTalking { get { return isTalking; } }
        private TextLine nowLine;
        private int lineNum;
        private int lineCnt;

        // 텍스트 저장 공간
        private List<Line> lines;

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
            lineData = new Dictionary<int, List<Line>>();
            initText();
        }

        private void initText()
        {
            // 텍스트 코드를 기억할 dummy int
            int num = 0;

            // 파일 끝까지 한 줄씩 읽기
            foreach (string str in CSVReader.Lines)
            {
                string[] strs = str.Split(",");

                // 번호칸이 비어있다면 이전 번호 그대로 사용
                if (string.IsNullOrEmpty(strs[0]) == false)
                {
                    num = int.Parse(strs[0]);
                    lineData[num] = new List<Line>();
                }

                // 코드별로 분리
                Code code = (Code)Enum.Parse(typeof(Code), strs[1]);

                switch (code)
                {
                    case Code.Text:
                        lineData[num].Add(new TextLine(strs[2], strs[3]));
                        break;

                    case Code.Select:
                        lineData[num].Add(new Select(strs));
                        break;

                    case Code.Case:
                        lineData[num].Add(new Case(strs[2]));
                        break;

                    case Code.End:
                        lineData[num].Add(new Line(Code.End));
                        break;

                    case Code.Event:
                        lineData[num].Add(new EventLine(strs[2]));
                        break;

                    default:
                        break;
                }
            }
        }

        public void initTalk(NPC npc)
        {
            // 대화 가능한 npc일 경우
            if (npc.getID() != 0)
            {
                // 대화 처음 시작 시 해당되는 대화목록 가져오기
                lines = lineData[npc.getID()];

                isTalking = true;
            }
        }

        /************************************************************
        * [대화 출력]
        * 
        * 인게임 화면의 대화 제어
        ************************************************************/
        private void Update()
        {
            if(isTalking && readLock == false)
            {
                readLine();
            }
        }

        private void readLine()
        {
            if(lineNum < lines.Count)
            {
                Line line = lines[lineNum++];

                switch(line.Code)
                {
                    case Code.Text:
                        nowLine = (TextLine)line;
                        talking();
                        break;

                    case Code.Select:
                        //selectManager.openSelect((Select)line);
                        break;

                    case Code.Case:
                        //selectCase(); // End까지 스킵
                        break;

                    case Code.Event:
                        EventLine eventLine = (EventLine)line;
                        command.getCommandEvent(eventLine.Command);
                        break;

                    default:
                        break;
                }
            }

            else
            {
                lineNum = 0;
                isTalking = false;
            }
        }

        public void talking()
        {
            if(lineCnt >= nowLine.Text.Length)
            {
                lineCnt = 0;

                ui.setDialogView(false);

                readLock = false;
            }

            else
            {
                printText(nowLine.Text);

                readLock = true;
            }
        }

        private void printText(string line)
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

        IEnumerator talkDelay(string line)
        {
            string text = "";

            // 대화 진행 도중일 경우
            while (lineCnt < line.Length)
            {
                // 한 글자씩 대화를 출력
                ui.setText(text += line[lineCnt++]);

                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }
}
