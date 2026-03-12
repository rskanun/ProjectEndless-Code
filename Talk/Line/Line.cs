using System;
using System.Collections.Generic;
using UnityEngine;

public enum LineType
{
    Text,       // 대사출력
    Select,     // 선택지
    Image,      // 이미지
    Destroy,    // 이미지 파괴
    Transform,  // 이미지 변형
    BGM,        // 반복되는 사운드 재생
    SE,         // 일회성 사운드 재생
    Event       // 이벤트(수치 조작 등) 발생
}

[Serializable]
public class Line
{
    [SerializeField, HideInInspector]
    private string _guid;
    public string guid => _guid;

    [SerializeField]
    private LineType _code;
    public LineType code => _code;

    // 에셋 저장 시 가지게 될 연결 라인 guid
    [SerializeField]
    private List<string> _nextLineGuids = new();
    public List<string> nextLineGuids
    {
        get => _nextLineGuids;
        set => _nextLineGuids = value;
    }

    // 연결 리스트 형태로 연결된 라인 소지
    [NonSerialized]
    private List<Line> _nextLines = new();
    public List<Line> nextLines
    {
        get => _nextLines;
        set => _nextLines = value;
    }

    public Line(LineType code)
    {
        _guid = Guid.NewGuid().ToString();
        _code = code;
    }

#if UNITY_EDITOR
    public Line(string guid, LineType code)
    {
        _guid = guid;
        _code = code;
    }
#endif
}