using Assets.Script.Control.Text.Object;
using Assets.Script.System;
using Assets.Script.System.Option;
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

    public class TalkManager : MonoBehaviour
    {
        [Header("플레이어 데이터")]
        [SerializeField] private PlayerData playerData;

        [Header("참조 스크립트")]
        [SerializeField] private TextManager textManager;
        [SerializeField] private SelectManager selectManager;
        [SerializeField] private EventManager eventManager;

        // 현재 라인 진행 상황
        private TextLine nowLine;
        private bool readLock;
        private bool coroutineLock;
        private int lineNum;

        // 텍스트 저장 공간
        private List<Line> lines;

        // 참조 스크립터블 오브젝트
        private PlayerState playerState;

        private void Start()
        {
            playerState = PlayerState.Instance;
        }

        private void Update()
        {
            // 텍스트 상호작용 키 감지
            talkingKeyPress();
        }

        /************************************************************
        * [대사 관리]
        * 
        * 대사를 읽어 그에 따른 인게임 이벤트 제어
        ************************************************************/

        public void talkingKeyPress()
        {
            // 대화가 처음이고 가능한 상태일 경우
            if (playerState.IsPlayerControllable)
            {
                // 대화가능한 npc가 범위 내에 있다면 상호작용 키로 대화를 활성화
                if (playerData.Npc is not null && Input.GetKeyDown(OptionSetting.Instance.Interact))
                {
                    initTalk(playerData.Npc);
                }
            }
        }

        public void initTalk(NPC npc)
        {
            // 대화 가능한 npc일 경우
            if (npc.getID() != 0)
            {
                // 대화 처음 시작 시 해당되는 대화목록 가져오기
                lines = npc.getLines();

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
                playerState.IsTalking = false;
            }

            coroutineLock = false;
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