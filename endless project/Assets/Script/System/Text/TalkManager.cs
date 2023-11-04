using Assets.Script.Control.Text.Object;
using Assets.Script.System;
using Assets.Script.System.Option;
using Assets.Script.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Script.Text
{
    public class TalkManager : MonoBehaviour
    {
        [Header("참조 스크립트")]
        [SerializeField] private TextManager textManager;
        [SerializeField] private SelectManager selectManager;
        [SerializeField] private EventManager eventManager;

        // 현재 라인 진행 상황
        private TextLine nowLine;
        private bool readLock;
        private bool coroutineLock;
        private int lineNum;

        // Select 관련 변수
        private Stack<Select> selectStack;

        // 텍스트 저장 공간
        private List<Line> lines;

        // 참조 스크립터블 오브젝트
        private PlayerState playerState;

        private void Start()
        {
            playerState = PlayerState.Instance;
        }

        public void nextText()
        {
            readLock = !textManager.talking(nowLine);
        }

        public void optionSelect(string option)
        {
            Select select = selectStack.Peek();
            int skipLineNum = select.OptionsLineNum[option];

            jumpLine(skipLineNum);
        }

        private void jumpLine(int num)
        {
            if (num < lines.Count)
            {
                lineNum = num + 1; // case나 end 제외
                readLock = false;
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
                lines = npc.getLines();
                selectStack = new Stack<Select>();

                playerState.IsTalking = true;

                StartCoroutine(readLines());
            }
        }

        IEnumerator readLines()
        {
            while(playerState.IsTalking)
            {
                if (readLock == false && coroutineLock == false)
                {
                    coroutineLock = true;
                    readLine();
                }
                yield return null;
            }
        }

        private void readLine()
        {
            if (lineNum < lines.Count)
            {
                Line line = lines[lineNum];
                processLine(line);

                // 다음 라인으로 이동
                lineNum++;
            }

            else
            {
                lineNum = 0;
                playerState.IsTalking = false;
            }

            coroutineLock = false;
        }

        /************************************************************
        * [라인 출력 관리]
        * 
        * 라인을 읽고서 거기에 따른 인게임 이벤트 제어
        ************************************************************/

        private void processLine(Line line)
        {
            switch (line.Code)
            {
                case LineType.Text:
                    processTextLine((TextLine)line);
                    break;

                case LineType.Select:
                    processSelect((Select)line);
                    break;

                case LineType.Case:
                    processCase(); // End까지 스킵
                    break;

                case LineType.Event:
                    processEventLine((EventLine)line);
                    break;

                default:
                    break;
            }
        }

        private void processTextLine(TextLine line)
        {
            readLock = true;
            nowLine = line;

            readLock = !textManager.talking(line);
        }

        private void processSelect(Select line)
        {
            readLock = true;

            selectStack.Push(line);
            selectManager.openSelect(line);
        }

        private void processCase()
        {
            Select select = selectStack.Pop();
            int skipLineNum = select.EndLineNum;

            jumpLine(skipLineNum);

            readLock = false;
        }

        private void processEventLine(EventLine line)
        {
            string command = line.Command;
            eventManager.getCommandEvent(command);
        }
    }
}