using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CsvReader : ScriptableObject
{
    private const string DIALOG_FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_DIRECTORY = "Assets/Resources/Scenario";
    private const string FILE_PATH = "Assets/Resources/Scenario/CsvReader.asset";

    private static CsvReader _instance;
    public static CsvReader Instance
    {
        get
        {
            if(_instance != null) return _instance;

            _instance = Resources.Load<CsvReader>("Scenario/CsvReader");

#if UNITY_EDITOR
            if(_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if(!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    if(!AssetDatabase.IsValidFolder(DIALOG_FILE_DIRECTORY))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }

                    AssetDatabase.CreateFolder("Assets/Resources", "Scenario");
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<CsvReader>(FILE_PATH);

                if(_instance == null)
                {
                    _instance = CreateInstance<CsvReader>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif

            return _instance;
        }
    }

    [MenuItem("GameObject/Singleton Scriptable Object/CsvReader", false, 30)]
    public static void CreateInInspector()
    {
        CsvReader dummy = Instance;
    }

    // Select 객체 내 optionsLineNum을 위한 Dictionary
    private Stack<Select> selectStack = new Stack<Select>();

    public Script GetScript(string path)
    {
        Script result = new Script();
        string[] csvFiles = Directory.GetFiles(path, "*.csv");

        foreach (string filePath in csvFiles)
        {
            if (File.Exists(filePath))
            {
                fileRead(result, filePath);
            }
        }

        return result;
    }

    private void fileRead(Script script, string path)
    {
        StreamReader sr = new StreamReader(File.OpenRead(path));

        int lineNum = 0;
        int id = 0; // id를 기억할 dummy int

        string str;
        while ((str = sr.ReadLine()) != null)
        {
            if (str[0] != '#')
            {
                string[] strs = str.Split(',');

                // 처음 부여한 키면 list 추가
                if (string.IsNullOrEmpty(strs[0]) == false)
                {
                    id = int.Parse(strs[0]);
                    script.SetLines(new List<Line>(), id);

                    // 라인 넘버 초기화
                    lineNum = 0;
                }

                script.GetLines(id).Add(createLine(strs, lineNum));

                // 다음 라인 넘버로 넘어감
                lineNum++;
            }
        }
    }

    private Line createLine(string[] strs, int lineNum)
    {
        // 코드별로 분리
        LineType code = (LineType)Enum.Parse(typeof(LineType), strs[1]);
        Line line = LineFactory.Instance.CreateLine(code, strs);

        // Select 처리
        if (code == LineType.Select)
        {
            selectStack.Push((Select)line);
        }
        // Case 처리
        else if (code == LineType.Case)
        {
            Select select = selectStack.Peek();
            select.addOptionBookmark(((Case)line).Choice, lineNum);
        }
        // End 처리
        else if (code == LineType.End)
        {
            Select select = selectStack.Pop();
            select.EndLineNum = lineNum;
        }

        return line;
    }
}
