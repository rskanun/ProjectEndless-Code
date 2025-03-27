using UnityEngine;

public class BattleCameraDirector
{
    private static BattleCameraDirector _instance;
    public static BattleCameraDirector Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BattleCameraDirector();

            return _instance;
        }
    }

    private BattleCameraManager manager;

    public void RegisterManager(BattleCameraManager manager)
    {
        this.manager = manager;
    }

    public void RemoveManager()
    {
        manager = null;
    }

    // 전투 연출
    //
    // 개전 시:
    // 플레이어 그룹 비추기(기습 or 일반 or 역기습 상황 보여주기)
    // -> 전체 화면 비추기(어떤 몬스터가 어느 위치에 있는 지 보여주기)
    //
    // 몬스터의 턴:
    // 해당 몬스터 싱글샷
    // -> 행동 연출
    //
    // 플레이어의 턴:
    // 해당 캐릭터 싱글샷
    // -> 선택창 모션 띄우면서 카메라 이동

    public void FocusingFullScreen()
    {
        manager.LiveMainCamera();
    }

    public void FocusingPlayerParty()
    {
        manager.LivePlayerPartyCamera();
    }

    public void FocusingEnemyParty()
    {
        manager.LiveEnemyPartyCamera();
    }

    public void FocusingCharacter(int instanceID)
    {
        Debug.Log("Character");
        manager.LiveCharacterCamera(instanceID);
    }

    public void FocusingSelection(int instanceID)
    {
        Debug.Log("Selection");
        manager.LiveSelectionCamera(instanceID);
    }
}