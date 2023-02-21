using Assets.Script.Control.Text.Object;
using Assets.Script.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    public class LineManager : MonoBehaviour
    {
        // csv 파일 위치
        private static string file = @"Assets\Resources\dialogue.csv";

        // 텍스트 데이터 저장
        private static Dictionary<int, List<Line>> lineData;

        // 참조 스크립트
        public TextManager textManager;
        public SelectManager selectManager;
        public EventManager eventManager;

        // 현재 라인 진행 상황
        private TextLine nowLine;
        [SerializeField] private bool readLock;
        [SerializeField] private bool isTalking = false;
        public bool IsTalking { get { return isTalking; } }
        [SerializeField] private int lineNum;

        // 텍스트 저장 공간
        private List<Line> lines;

        private void Awake()
        {
            fileRead();
        }

        /************************************************************
        * [초기 설정]
        * 
        * CSV 파일로부터 대사 가져오기 및 정리 데이터 객체 형태로 보관
        ************************************************************/

        public void fileRead()
        {
            lineData = new Dictionary<int, List<Line>>();
            StreamReader reader = new StreamReader(File.OpenRead(file));

            // 텍스트 코드를 기억할 dummy int
            int num = 0;

            while (!reader.EndOfStream)
            {
                string str = reader.ReadLine();

                if (str.ToCharArray()[0] != '#')
                {
                    addLineData(ref num, str);
                }
            }
        }

        private void addLineData(ref int num, string str)
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

        /************************************************************
        * [대사 관리]
        * 
        * 대사를 읽어 그에 따른 인게임 이벤트 제어
        ************************************************************/
        public void initTalk(NPC npc)
        {
            // 대화 가능한 npc일 경우
            if (npc.getID() != 0)
            {
                // 대화 처음 시작 시 해당되는 대화목록 가져오기
                lines = lineData[npc.getID()];

                isTalking = true;

                StartCoroutine(readLines());
            }
        }

        IEnumerator readLines()
        {
            while(isTalking)
            {
                if (readLock == false) readLine();
                yield return null;
            }
        }

        private void readLine()
        {
            if (lineNum < lines.Count)
            {
                Line line = lines[lineNum++];

                switch (line.Code)
                {
                    case Code.Text:
                        nowLine = (TextLine)line;
                        nextText();
                        break;

                    case Code.Select:
                        readLock = true;
                        selectManager.openSelect((Select)line);
                        break;

                    case Code.Case:
                        selectCase(); // End까지 스킵
                        break;

                    case Code.Event:
                        EventLine eventLine = (EventLine)line;
                        eventManager.getCommandEvent(eventLine.Command);
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

        public void nextText()
        {
            readLock = !textManager.talking(nowLine);
        }

        public void selectCase(string option = "")
        {
            for (Line line = lines[lineNum];
                line.Code != Code.End && lineNum < lines.Count; line = lines[++lineNum])
            {
                if (line.Code == Code.Case)
                {
                    Case selectCase = (Case)line;
                    if (selectCase.Choice.Equals(option))
                    {
                        lineNum++;
                        selectManager.closeSelect();
                        break;
                    }
                }
            }

            readLock = false;
        }
    }
}