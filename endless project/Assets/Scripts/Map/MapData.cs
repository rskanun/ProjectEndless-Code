using System;
using UnityEngine;

public class MapData : ScriptableObject
{
    private string _id;
    public string ID
    {
        get
        {
            // ID 값이 생성되지 않은 맵 데이터일 경우
            if (string.IsNullOrEmpty(_id))
            {
                // 앞의 12자리는 생성 시간 값의 16진수 변환 값
                string timeBaseHex = DateTime.UtcNow.Ticks.ToString("X").Substring(0, 12);

                // 뒤의 12자리는 Guid를 사용한 랜덤한 값
                string guidHex = Guid.NewGuid().ToString("N").Substring(0, 12);

                // 24자리의 랜덤한 값을 반환
                _id = timeBaseHex + guidHex;
            }

            return _id;
        }
    }

    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [SerializeField]
    private string _sceneName;
    public string SceneName
    {
        get { return _sceneName; }
    }

    public override bool Equals(object other)
    {
        if (other != null && other is MapData)
        {
            MapData otherData = (MapData)other;

            return ID == otherData.ID;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Convert.ToInt32(ID, 16);
    }
}