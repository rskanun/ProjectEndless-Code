using Assets.Script.Control.Text.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Control.Text
{

    public class TextManager : MonoBehaviour
    {
        // 참조 스크립트
        public EventManager command;
        public TextUI ui;

        // 텍스트 현재 진행 상태
        private TextLine nowLine;
        private bool readLock;
        private bool isTalking = false;
        public bool IsTalking { get { return isTalking; } }
        private int lineNum;
        private int lineCnt;

        // 텍스트 저장 공간
        private List<Line> lines;

        // 출력 속도
        private float typingSpeed;

        /************************************************************
        * [대화 출력]
        * 
        * 인게임 화면의 대화 제어
        ************************************************************/
        public void initTalk(NPC npc)
        {
            // 대화 가능한 npc일 경우
            if (npc.getID() != 0)
            {
                // 대화 처음 시작 시 해당되는 대화목록 가져오기
                lines = TextReader.getLines(npc.getID());

                isTalking = true;
            }
        }

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
