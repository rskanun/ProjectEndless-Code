using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private static int curAreaID;
    private static int lastEntedAreaID;

    [SerializeField]
    private MapData mapInfo;

    // 해당 맵의 구역 정보
    private Dictionary<int, FieldArea> areaDict = new();

    private void Awake()
    {
        // 현재 맵으로 설정
        GameData.Instance.MapScene = gameObject.scene.name;
        GameData.Instance.MapName = mapInfo.Name;

        // 해당 맵이 거점 맵인 경우
        if (this is BaseMap)
        {
            // 전투 패배시 돌아올 장소로 지정
            GameData.Instance.RespawnMapScene = gameObject.scene.name;
        }
    }

    public void RegisterArea(FieldArea area)
    {
        // 관리 구역 리스트에 추가
        areaDict.Add(area.ID, area);

        // 게임 데이터에도 해당 정보 추가
        GameData.Instance.AreaDatas.Add(new AreaData(area.ID, area.IsClearArea));
    }

    public void RemoveArea(FieldArea area)
    {
        // 관리 구역 리스트에 삭제
        areaDict.Remove(area.ID);

        // 게임 데이터에서도 해당 정보 삭제
        GameData.Instance.AreaDatas.RemoveWhere(data => data.id == area.ID);
    }

    /************************************************************
     * [구역 관리]
     * 
     * 게임 파일 로드 및 전투 결과에 따른 구역 정보 설정
     ************************************************************/

    /// <summary>
    /// 게임 파일 로드 시, 게임 데이터를 토대로 현재 각 구역들의 클리어 상황만을 업데이트
    /// </summary>
    public void UpdateData()
    {
        // 각 구역의 클리어 여부만 갱신
        foreach (AreaData data in GameData.Instance.AreaDatas)
        {
            // 해당 ID를 가진 Area 찾기
            FieldArea area = areaDict.GetValueOrDefault(data.id);

            // 찾은 Area가 존재하면 클리어 여부 업데이트
            if (area != null)
            {
                area.IsClearArea = data.isClearArea;
                area.SetActiveMonsters(!data.isClearArea); // 클리어했다면 비활성화
            }
        }
    }

    /// <summary>
    /// 전투에서 승리했을 경우 해당 구역을 클리어 했다고 설정
    /// </summary>
    public void OnEndBattle()
    {
        // 필드로 복귀한 게 아니라면 무시
        if (GameData.Instance.State != GameState.Field) return;

        // 전투에서 승리한 경우에만 현재 구역을 토벌했다고 인정
        if (BattleCache.Current.Result == BattleResult.Victory)
        {
            areaDict[curAreaID].IsClearArea = true;
            areaDict[curAreaID].SetActiveMonsters(false);
        }
    }

    /************************************************************
     * [구역 이동]
     * 
     * 특정 구역에 들어오거나 나갈 때 실행될 함수 관리
     ************************************************************/

    public void OnEntedArea(FieldArea area)
    {
        // 이전 구역이 현재 없거나, 첫 방문 구역인 경우 혹은 모종의 이유로 재방문한 경우
        if (!areaDict.ContainsKey(lastEntedAreaID) || curAreaID == area.ID)
        {
            // 해당 구역 활성화
            EnableArea(area);
        }

        // 마지막 방문 구역으로 등록
        lastEntedAreaID = area.ID;
    }

    public void OnExitedArea(FieldArea area)
    {
        if (curAreaID != area.ID)
        {
            // 나간 영역이 현재 구역이 아닐 경우 무시
            return;
        }

        // 마지막으로 방문한 구역을 카메라 영역으로 변경
        EnableArea(areaDict[lastEntedAreaID]);
    }

    public void EnableArea(FieldArea area)
    {
        FieldArea prevArea = areaDict.GetValueOrDefault(curAreaID);
        curAreaID = area.ID;

        // 해당 구역을 카메라 영역으로 지정
        SetCurrentArea(area);

        // 해당 구역의 몬스터 정보를 캐시 데이터에 저장
        BattleCache.Current.FieldData = area.FieldData;

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

    private void SetCurrentArea(FieldArea area)
    {
        // 카메라 영역 지정
        PlayerTrackerCamera.cameraArea = area.AreaCollider;

        // 구역 변경 알림
        GameEventManager.Instance.NotifyAreaChanged();
    }

    private void DisableArea(FieldArea area)
    {
        // 구역 몬스터 비활성화
        area.SetActiveMonsters(false);
    }
}