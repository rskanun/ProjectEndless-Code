using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;




#if UNITY_EDITOR
using UnityEditor;
#endif

public class TextScriptResource : ScriptableObject
{
    private const string FILE_DIRECTORY = "Assets/Resources/Scenario";
    private const string TEXT_SCRIPT_DIRECTORY = "Assets/Resources/Scenario/TextScript";
    private const string FILE_PATH = "Assets/Resources/Scenario/ScriptResource.asset";

    private static TextScriptResource _instance;
    public static TextScriptResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<TextScriptResource>("Scenario/ScriptResource");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    string[] folders = FILE_DIRECTORY.Split('/');
                    string currentPath = folders[0];

                    for (int i = 1; i < folders.Length; i++)
                    {
                        if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                        {
                            AssetDatabase.CreateFolder(currentPath, folders[i]);
                        }

                        currentPath += "/" + folders[i];
                    }
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<TextScriptResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<TextScriptResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif

            return _instance;
        }
    }

    public TextScript CurrentScript { get; private set; }

    public void LoadScript(int chapter, int root, int subChapter)
    {
        // 해당 상황에 맞는 스크립트를 가져올 폴더 경로 구하기
        string path = GetFolderPath(chapter, root, subChapter);

        // 경로를 통한 CSV 파일 라인 구하기
        CsvFile file = CsvReader.ReadFiles(path);

        // CSV 파일의 정보를 토대로 텍스트 스크립트 구현
        CurrentScript = BuildTextScript(file);
    }

    private string GetFolderPath(int chapter, int root, int subChapter)
    {
        // 챕터번호 1자리 + 분기번호 1자리 + 서브챕터번호 2자리
        string folderName = $"{chapter}{root}{subChapter:d2}";
        string path = TEXT_SCRIPT_DIRECTORY + "/" + folderName;

        return path;
    }

    private TextScript BuildTextScript(CsvFile csvFile)
    {
        TextScript script = new TextScript();

        Dictionary<int, int> lineNum = new Dictionary<int, int>(); // ID 값에 해당하는 라인의 마지막 index값
        int id = 0; // id를 기억할 dummy int

        foreach (string[] cells in csvFile)
        {
            // 처음 부여한 ID일 경우
            int tmpID = 0;
            if (int.TryParse(cells[0], out tmpID) && !script.ContainsKey(tmpID))
            {
                // 처음 부여한 키일 때에만 id값 변경
                id = tmpID;

                // 새 스크립트 라인 생성
                script.SetLines(new List<Line>(), id);
                lineNum.Add(id, 0);
            }

            // 라인 객체 생성
            Line line = CreateLine(cells);

            // 스크립트에 추가
            script.GetLines(id).Add(line);

            // 다음 라인 넘버로 넘어감
            lineNum[id]++;
        }

        // Select 옵션 연결
        LinkSelectBranches(script, lineNum.Keys.ToList());

        // 완성된 스크립트 전달
        return script;
    }

    private Line CreateLine(string[] strs)
    {
        // 코드별로 분리
        LineType code = (LineType)Enum.Parse(typeof(LineType), strs[1]);
        Line line = LineFactory.CreateLine(code, strs);

        return line;
    }

    private void LinkSelectBranches(TextScript script, List<int> ids)
    {
        foreach (int id in ids)
        {
            int index = 0;
            Stack<Select> stack = new Stack<Select>();

            foreach (Line line in script.GetLines(id))
            {
                switch (line.Code)
                {
                    case LineType.Select:
                        stack.Push((Select)line);
                        break;

                    case LineType.Case:
                        stack.Peek().addOptionBookmark(((Case)line).Choice, index);
                        break;

                    case LineType.End:
                        stack.Pop().EndLineNum = index;
                        break;
                }

                index++;
            }
        }
    }

    public bool HasLines(int id)
    {
        // id 값이 0이상이고 현재 스크립트가 해당 id 값의 스크립트를 가지고 있는 지
        return (id > 0) && CurrentScript.ContainsKey(id);
    }
}