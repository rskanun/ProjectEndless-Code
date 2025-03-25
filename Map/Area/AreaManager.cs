using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField]
    private MapData map;

    // 해당 맵의 구역 정보
    private HashSet<FieldArea> areas = new HashSet<FieldArea>();

    [ReadOnly, SerializeField]
    private FieldArea _currentArea;
    public FieldArea CurrentArea
    {
        private set { _currentArea = value; }
        get { return _currentArea; }
    }

    [ReadOnly, SerializeField]
    private FieldArea _lastEntedArea;
    public FieldArea LastEntedArea
    {
        private set { _lastEntedArea = value; }
        get { return _lastEntedArea; }
    }

    public void RegisterArea(FieldArea area)
    {
        // 관리 구역 리스트에 추가
        areas.Add(area);
    }

    public void RemoveArea(FieldArea area)
    {
        // 관리 구역 리스트에 삭제
        areas.Remove(area);
    }

    public List<AreaData> GetAreaDatas()
    {
        return areas.Select(area => area.GetAreaData()).ToList();
    }

    public void SetAreaDatas(List<AreaData> datas)
    {
        if (datas == null) return;

        foreach (AreaData data in datas)
        {
            // 해당 ID를 가진 Area 찾기
            FieldArea area = areas.FirstOrDefault(a => a.ID == data.id);

            // 찾은 Area가 존재하면 IsClearArea를 업데이트
            if (area != null)
            {
                area.IsClearArea = data.isClearArea;
            }
        }
    }

    /************************************************************
     * [구역 이동]
     * 
     * 특정 구역에 들어오거나 나갈 때 실행될 함수 관리
     ************************************************************/

    public void OnEntedArea(FieldArea area)
    {
        Debug.Log("Enter Area: " + area);
        // 첫 방문 구역일 경우
        if (LastEntedArea == null)
        {
            // 해당 구역 활성화
            CurrentArea = area;
            EnableArea(area);
        }

        // 마지막 방문 구역으로 등록
        LastEntedArea = area;
    }

    public void OnExitedArea(FieldArea area)
    {
        Debug.Log("Exit Area: " + area);
        if (CurrentArea != area)
        {
            // 나간 영역이 현재 구역이 아닐 경우 무시
            return;
        }

        // 마지막으로 방문한 구역을 카메라 영역으로 변경
        EnableArea(LastEntedArea);
    }

    public void EnableArea(FieldArea area)
    {
        FieldArea prevArea = CurrentArea;
        CurrentArea = area;

        // 해당 구역을 카메라 영역으로 지정
        MapManager.SetCurrentArea(area.AreaCollider);

        // 구역 몬스터 활성화
        if (area.IsClearArea == false)
        {
            // 클리어된 구역이 아닐 경우에만 활성화
            area.SetActiveMonsters(true);
        }

        // 이전 구역 비활성화
        if (prevArea != null && prevArea != area)
        {
            DisableArea(prevArea);
        }
    }

    private void DisableArea(FieldArea area)
    {
        // 구역 몬스터 비활성화
        area.SetActiveMonsters(false);
    }
}