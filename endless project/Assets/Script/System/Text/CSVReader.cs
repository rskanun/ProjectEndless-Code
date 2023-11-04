using UnityEngine;
using System.Collections.Generic;
using Assets.Script.Control.Text.Object;
using System.IO;
using System;
using Assets.Script.Control.Text;
using Assets.Script.System.Text;
using Assets.Script.System.Text.LineObject;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Singleton Object/CSVReader", fileName = "CSVReader")]
public class CSVReader : ScriptableObject
{
    private const string DIALOG_FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_DIRECTORY = "Assets/Resources/Scenario";
    private const string FILE_PATH = "Assets/Resources/Scenario/CSVReader.asset";

    private static CSVReader _instance;
    public static CSVReader Instance
    {
        get
        {
            if(_instance != null) return _instance;

            _instance = Resources.Load<CSVReader>("Scenario/CSVReader");

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

    // Select 객체 내 optionsLineNum을 위한 Dictionary
    private Stack<Select> selectStack = new Stack<Select>();

    private Dictionary<int, Script> _mainScript;
    public Script getMainScript()
    {
        int chapterID = 0;

        if (_mainScript != null) return _mainScript[chapterID];

        // main script is null
        _mainScript = new Dictionary<int, Script>();

        if (ChapterResource.Instance.Data != null)
        {
            foreach (ChapterData data in ChapterResource.Instance.Data)
            {
                if (data.csvFile != null)
                {
                    int id = data.chapterID;

                    // 중복 주의
                    if (_mainScript.ContainsKey(id))
                    {
                        Debug.LogWarning("ID:" + id + " 챕터가 중복으로 입력되었습니다!");
                        Debug.LogWarning("현재 파일로 덮어 씌웁니다.");
                    }

                    _mainScript[id] = fileRead(data.csvFile);
                }
            }

            return _mainScript[chapterID];
        }

        return _mainScript[chapterID];
    }

    private Script fileRead(TextAsset csvFile)
    {
        Script script = new Script();

        if (csvFile != null)
        {
            StringReader sr = new StringReader(csvFile.text);

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
                        script.setScenario(new List<Line>(), id);

                        // 라인 넘버 초기화
                        lineNum = 0;
                    }

                    script.getScenario(id).Add(createLine(strs, lineNum));

                    // 다음 라인 넘버로 넘어감
                    lineNum++;
                }
            }
        }

        return script;
    }

    private Line createLine(string[] strs, int lineNum)
    {
        // 코드별로 분리
        LineType code = (LineType)Enum.Parse(typeof(LineType), strs[1]);
        Line line = LineFactory.Instance.createLine(code, strs);

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
