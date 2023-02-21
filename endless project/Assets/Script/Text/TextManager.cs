using Assets.Script.Control.Text.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Control.Text
{
    public class TextManager : MonoBehaviour
    {
        public bool isActive { get { return ui.IsActive; } }

        // 참조 스크립트
        public TextUI ui;

        // 텍스트 현재 진행 상태
        [SerializeField]private int textCnt = 0;

        // 출력 속도
        private float typingSpeed;

        /************************************************************
        * [대화 출력]
        * 
        * 인게임 화면의 대화 제어
        ************************************************************/
        public bool talking(TextLine line)
        {
            if(textCnt >= line.Text.Length)
            {
                clear();

                return true;
            }

            // else 대사 출력
            ui.setName(line.Name);
            printText(line.Text);

            return false;
        }

        private void printText(string line)
        {
            // 한 글자도 출력이 안 됐을 경우
            if (textCnt == 0)
            {
                // 텍스트 창 활성화 및 타이핑 속도 리셋
                ui.setDialogView(true);
                typingSpeed = Option.getTypingSpeed();

                // 지정된 타이핑 속도로 출력
                StartCoroutine(textDelayPrint(line));
            }

            // 대화 출력 도중일 경우
            else if (textCnt < line.Length)
            {
                // 한 번에 출력
                typingSpeed = 0;
            }
        }

        IEnumerator textDelayPrint(string line)
        {
            string text = "";

            // 대화 진행 도중일 경우
            while (textCnt < line.Length)
            {
                // 한 글자씩 대화를 출력
                ui.setText(text += line[textCnt++]);

                yield return new WaitForSeconds(typingSpeed);
            }
        }

        public void clear()
        {
            textCnt = 0;

            ui.textClear();
            ui.setDialogView(false);
        }
    }
}
