using System;
using UnityEngine;

[Serializable]
public class MapData
{
    [SerializeField]
    private string _name;
    public string Name => _name;

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
}