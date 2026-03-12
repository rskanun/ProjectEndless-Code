using System.Collections.Generic;
using System.Linq;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;

public class Scenario : ScriptableObject, ISerializationCallbackReceiver
{
    private Dictionary<int, ScenarioScene> scenarios = new();
    public IEnumerable<int> IDs => scenarios.Keys;

    [SerializeField]
    private List<ScenarioEntry> serializedScenarios = new();

    [SerializeField]
    private string _nameTable;
    public string nameTable
    {
        get => _nameTable;
        set => _nameTable = value;
    }

    [SerializeField]
    private string _dialogueTable;
    public string dialogueTable
    {
        get => _dialogueTable;
        set => _dialogueTable = value;
    }

    [SerializeField]
    private string _selectionTable;
    public string selectionTable
    {
        get => _selectionTable;
        set => _selectionTable = value;
    }

#if UNITY_EDITOR
    // 에디터 전용 그래프 데이터
    [SerializeField]
    private GraphData _graphData = new GraphData();
    public GraphData graphData
    {
        get => _graphData;
        set => _graphData = value;
    }

    // 설정 데이터
    [SerializeField]
    private StringTableCollection _nameTableCollection;
    public StringTableCollection nameTableCollection
    {
        get => _nameTableCollection;
        set
        {
            if (_nameTableCollection == value) return;

            // 테이블 이름 값 업데이트
            _nameTable = value?.name;

            _nameTableCollection = value;
        }
    }
    [SerializeField]
    private StringTableCollection _dialogueTableCollection;
    public StringTableCollection dialogueTableCollection
    {
        get => _dialogueTableCollection;
        set
        {
            if (_dialogueTableCollection == value) return;

            // 테이블 이름 값 업데이트
            _dialogueTable = value?.name;

            // 대사 노드만 뽑아서 업데이트(다른 테이블의 노드 지우기 방지)
            var entries = _graphData.nodes.OfType<TextNodeData>()
                .ToDictionary(data => data.dialogueKey, data => data.dialogue);

            // 로컬라이제이션 테이블 업데이트
            OnTableChanged(_dialogueTableCollection, value, entries);

            _dialogueTableCollection = value;
        }
    }
    [SerializeField]
    private StringTableCollection _selectionTableCollection;
    public StringTableCollection selectionTableCollection
    {
        get => _selectionTableCollection;
        set
        {
            if (_selectionTableCollection == value) return;

            // 테이블 이름 값 업데이트
            _selectionTable = value?.name;

            // 대사 노드만 뽑아서 업데이트(다른 테이블의 노드 지우기 방지)
            var entries = _graphData.nodes.OfType<SelectNodeData>()
                .SelectMany(data => data.optionKeys.Zip(data.options, (k, v) => new { k, v }))
                .ToDictionary(pair => pair.k, pair => pair.v);

            // 로컬라이제이션 테이블 업데이트
            OnTableChanged(_selectionTableCollection, value, entries);

            _selectionTableCollection = value;
        }
    }

    /// <summary>
    /// 해당 에셋이 삭제될 때, 로컬라이제이션 테이블에 등록된 값들 삭제
    /// </summary>
    public void SyncLocalizationTable()
    {
        // 로컬라이제이션을 이용하는 경우에만 실행
        if (!VisualScriptingSettings.Instance.UseLocalization)
        {
            return;
        }

        // 로컬라이제이션을 사용하지만 테이블이 없는 경우에도 실행 X
        if (dialogueTableCollection == null || selectionTableCollection == null)
        {
            return;
        }

        // 현재 노드로 인해 생성된 로컬라이제이션 테이블 키 삭제
        foreach (var node in _graphData.nodes)
        {
            if (node is TextNodeData textNode)
            {
                dialogueTableCollection.SharedData.RemoveKey(textNode.dialogueKey);
            }
            else if (node is SelectNodeData selectNode)
            {
                foreach (var key in selectNode.optionKeys)
                {
                    selectionTableCollection.SharedData.RemoveKey(key);
                }
            }
        }
    }

    /// <summary>
    /// 사용되는 테이블 값이 바뀐 경우 데이터 이전
    /// </summary>
    private void OnTableChanged(StringTableCollection origin, StringTableCollection newTable, Dictionary<string, string> entries)
    {
        if (origin == null) return;

        var setting = VisualScriptingSettings.Instance;
        var table = newTable.GetTable(setting.ProjectLocale.Identifier) as StringTable;

        // 기존 테이블에 저장된 값은 지우고, 새로운 테이블로 옮기기
        foreach (var (k, v) in entries)
        {
            origin.SharedData.RemoveKey(k);
            table.AddEntry(k, v);
        }
    }

    /// <summary>
    /// 시나리오의 대사 데이터 초기화
    /// </summary>
    public void LineClear()
    {
        serializedScenarios.Clear();
        scenarios.Clear();
    }
#endif

    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize()
    {
        // 직렬화시킨 구조를 Dictionary 형태로 변경
        scenarios = serializedScenarios.ToDictionary(entry => entry.id, entry => new ScenarioScene(entry.lines));

        // guid로 저장된 연결 라인 값에 실제 값 넣기
        // 빠른 탐색을 위한 Dictionary 타입으로 변경
        var dict = serializedScenarios.SelectMany(entry => entry.lines)
                    .ToDictionary(line => line.guid, line => line);

        foreach (var line in dict.Values)
        {
            // 다음 라인 값 설정
            line.nextLines = new List<Line>();
            foreach (var guid in line.nextLineGuids)
            {
                if (dict.TryGetValue(guid, out var nextLine))
                {
                    line.nextLines.Add(nextLine);
                }
            }
        }
    }

    /// <summary>
    /// 해당 번호의 시나리오에 Line 추가
    /// </summary>
    /// <param name="num">Line을 추가할 시나리오 번호</param>
    public void AddLine(int num, Line line)
    {
        // 직렬화 형태로 임시 추가
        var entry = serializedScenarios.FirstOrDefault(e => e.id == num);
        if (entry == null)
        {
            entry = new ScenarioEntry(num, new List<Line>());
            serializedScenarios.Add(entry);
        }

        entry.lines.Add(line);
    }

    /// <summary>
    /// 번호에 맞는 Line 배열의 시작 부분 가져오기
    /// </summary>
    /// <param name="num">가져올 시나리오 번호</param>
    public ScenarioScene GetScenarioScene(int num)
    {
        // 해당 번호의 시나리오가 없거나 로드되지 않았다면
        if (ContainsKey(num) == false)
        {
            // 빈 값 리턴
            return null;
        }

        // 해당 번호의 씬 리턴
        return scenarios[num];
    }

    public bool ContainsKey(int id)
    {
        return scenarios.ContainsKey(id);
    }

    // 직렬화 저장용 객체
    [System.Serializable]
    private class ScenarioEntry
    {
        public int id;
        [SerializeReference]
        public List<Line> lines = new();

        public ScenarioEntry(int id, List<Line> lines)
        {
            this.id = id;
            this.lines = lines;
        }
    }
}