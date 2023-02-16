using Assets.Script.Control.Text.Object;
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

    public class TextReader : MonoBehaviour
    {
        // csv 파일 위치
        private string file = @"Assets\Resources\dialogue.csv";

        // 텍스트 데이터 저장
        private static Dictionary<int, List<Line>> lineData;

        /************************************************************
        * [초기 설정]
        * 
        * CSV 파일로부터 대사 가져오기 및 정리 데이터 객체 형태로 보관
        ************************************************************/

        void Awake()
        {
            fileRead();
        }

        private void fileRead()
        {
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
        * CSV 파일로부터 읽은 데이터 관리
        ************************************************************/

        public static List<Line> getLines(int index)
        {
            return lineData[index];
        }
    }
}