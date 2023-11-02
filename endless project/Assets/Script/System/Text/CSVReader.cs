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

public class CSVReader : ScriptableObject
{
    private const string DIALOG_FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_DIRECTORY = "Assets/Resources/Scripts";
    private const string FILE_PATH = "Assets/Resources/Scripts/CSVReader.asset";

    private static CSVReader _instance;
    public static CSVReader Instance
    {
        get
        {
            if(_instance != null) return _instance;

            _instance = Resources.Load<CSVReader>("Scripts/CSVReader");

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

                    AssetDatabase.CreateFolder("Assets/Resources", "Scripts");
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

            // id를 기억할 dummy int
            int id = 0;

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
                        script.setScenario(new Scenario(), id);
                    }

                    Scenario scenario = script.getScenario(id);
                    scenario.AddLine(addLineData(strs));
                }
            }
        }

        return script;
    }

    private Line addLineData(string[] strs)
    {
        // 코드별로 분리
        LineType code = (LineType)Enum.Parse(typeof(LineType), strs[1]);
        Line line = LineFactory.Instance.createLine(code, strs);

        return line;
    }
}
