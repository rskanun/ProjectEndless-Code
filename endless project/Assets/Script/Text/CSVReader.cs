using UnityEngine;
using System.Collections.Generic;
using Assets.Script.Control.Text.Object;
using System.IO;
using System;
using Assets.Script.Control.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CSVReader : ScriptableObject
{
    private const string CSV_FILE_PATH = "Assets/Resources/Dialog/dialogue.csv";

    private const string FILE_DIRECTORY = "Assets/Resources/Dialog";
    private const string FILE_PATH = "Assets/Resources/Dialog/CSVReader.asset";

    private static CSVReader _instance;
    public static CSVReader Instance
    {
        get
        {
            if(_instance != null) return _instance;

            _instance = Resources.Load<CSVReader>("CSVReader");

#if UNITY_EDITOR
            if(_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if(!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                    AssetDatabase.CreateFolder("Resources", "Dialog");
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<CSVReader>(FILE_PATH);

                if(_instance == null)
                {
                    _instance = CreateInstance<CSVReader>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif

            return _instance;
        }
    }

    private Dictionary<int, List<Line>> lineData;
    public Dictionary<int, List<Line>> LineData
    {
        get
        {
            if(lineData != null) return lineData;

            lineData = fileRead();

            return lineData;
        }
    }

    private Dictionary<int, List<Line>> fileRead()
    {
        Dictionary<int, List<Line>> data = new Dictionary<int, List<Line>>();

        if(File.Exists(CSV_FILE_PATH))
        {
            StreamReader reader = new StreamReader(File.OpenRead(CSV_FILE_PATH));

            // 텍스트 코드를 기억할 dummy int
            int num = 0;

            while (!reader.EndOfStream)
            {
                string str = reader.ReadLine();

                if (str.ToCharArray()[0] != '#')
                {
                    addLineData(ref data, ref num, str);
                }
            }
        }

        return data;
    }

    private void addLineData(ref Dictionary<int, List<Line>> data, ref int num, string str)
    {
        string[] strs = str.Split(",");

        // 번호칸이 비어있다면 이전 번호 그대로 사용
        if (string.IsNullOrEmpty(strs[0]) == false)
        {
            num = int.Parse(strs[0]);
            data[num] = new List<Line>();
        }

        // 코드별로 분리
        Code code = (Code)Enum.Parse(typeof(Code), strs[1]);

        switch (code)
        {
            case Code.Text:
                data[num].Add(new TextLine(strs[2], strs[3]));
                break;

            case Code.Select:
                data[num].Add(new Select(strs));
                break;

            case Code.Case:
                data[num].Add(new Case(strs[2]));
                break;

            case Code.End:
                data[num].Add(new Line(Code.End));
                break;

            case Code.Event:
                data[num].Add(new EventLine(strs[2]));
                break;

            default:
                break;
        }
    }
}
