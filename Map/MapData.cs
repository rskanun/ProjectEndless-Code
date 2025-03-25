using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/MapData", fileName = "Map_Data")]
public class MapData : ScriptableObject
{
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [ReadOnly, SerializeField]
    private string _id;
    public string ID
    {
        get
        {
            // ID 값이 생성되지 않은 맵 데이터일 경우
            if (string.IsNullOrEmpty(_id))
            {
                // 앞의 12자리는 생성 시간 값의 16진수 변환 값
                string timeBaseHex = DateTime.UtcNow.Ticks.ToString("x").Substring(0, 12);

                // 뒤의 12자리는 Guid를 사용한 랜덤한 값
                string guidHex = Guid.NewGuid().ToString("N").Substring(0, 12);

                // 24자리의 랜덤한 값을 반환
                _id = timeBaseHex + guidHex;
            }

            return _id;
        }
    }

    [SerializeField]
    private SceneAsset _scene;
    public string SceneName
    {
        get { return _scene.name; }
    }

    // 현재 맵에 있는 구역 관리 매니져
    private AreaManager manager;

    // 임시로 지닌 로드될 구역 데이터
    private List<AreaData> loadDatas;

    public override bool Equals(object other)
    {
        if (other != null && other is MapData otherData)
        {
            return ID == otherData.ID;
        }

        throw new NotSupportedException("Equals method should be called with an object of type MapData.");
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public void RegisterManager(AreaManager manager)
    {
        this.manager = manager;

        // 관리자 연결 시 불러올 데이터가 있는 경우
        if (loadDatas != null && loadDatas.Count > 0)
        {
            manager.SetAreaDatas(loadDatas);

            // 임시 데이터 비우기
            loadDatas = null;
        }
    }

    public void RemoveManager()
    {
        manager = null;
    }

    public List<AreaData> GetAreaDatas()
    {
        if (manager == null) return new List<AreaData>();

        return manager.GetAreaDatas();
    }

    public void SetAreaDatas(List<AreaData> datas)
    {
        if (manager == null) loadDatas = datas;
        else manager.SetAreaDatas(datas);
    }
}