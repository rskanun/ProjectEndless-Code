using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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

    // 카메라 중심점(대상)
    private Dictionary<int, Transform> bodyTransforms;

    // 각 진형 별 InstanceID값
    private HashSet<int> playerIDs;
    private HashSet<int> enemyIDs;

    // 선택창 모션 위치
    private float x = 3.0f;
    private float y = 1.0f;

    public BattleCameraDirector()
    {
        bodyTransforms = new Dictionary<int, Transform>();

        playerIDs = new HashSet<int>();
        enemyIDs = new HashSet<int>();
    }

    public void RegisterManager(BattleCameraManager manager)
    {
        this.manager = manager;
    }

    public void RemoveManager()
    {
        manager = null;
    }

    public void RegisterPlayerChrPivot(int instanceID, Transform pivot)
    {
        bodyTransforms.Add(instanceID, pivot);
        playerIDs.Add(instanceID);
    }

    public void RegisterEnemyChrPivot(int instanceID, Transform pivot)
    {
        bodyTransforms.Add(instanceID, pivot);
        enemyIDs.Add(instanceID);
    }

    // 전투 연출
    //
    // 개전 시:
    // 플레이어 그룹 비추기(기습 or 일반 or 역기습 상황 보여주기)
    // -> 점점 전체 화면 비추기(어떤 몬스터가 어느 위치에 있는 지 보여주기)
    //
    // 턴이 돌아올 시:
    // 별 다른 모션 없이 해당 캐릭터와 타겟을 그룹샷
    //
    // 행동 선택 시:
    // 별 다른 모션 없이 해당 캐릭터의 싱글샷
    // -> 행동 선택 모션을 취하며 빠르게 살짝 옆으로 카메라 이동
    //
    //
    // 행동 선택 시 바로 행동 모션에 들어감
    // 원거리 행동 -> 제자리에서 사용 모션
    // 근거리 행동 -> 타겟에게 달려가는 모션
    //
    // 차례가 돌아오면 완전한 사용 모션
    //
    // 행동 모션 이후 곧바로 다음 선택된 행동 개시


    /***************************************************************
    * [ 카메라 설정 ]
    * 
    * 누굴 향해 카메라를 잡을 지 설정
    ***************************************************************/

    public void FocusFullScreen()
    {
        manager.LiveMainCamera();
    }

    public void FocusPlayerGroup()
    {
        // 플레이어 그룹의 중심점만 가져오기
        List<Transform> group = bodyTransforms
            .Where(v => playerIDs.Contains(v.Key))
            .Select(v => v.Value)
            .ToList();

        if (group.Count > 0)
        {
            // 그룹으로 잡을 엔티티가 있는 경우에만 카메라 라이브
            manager.LiveGroupCamera(group);
        }
    }

    public void FocusEnemyGroup()
    {
        // 적 그룹의 중심점만 가져오기
        List<Transform> group = bodyTransforms
            .Where(v => enemyIDs.Contains(v.Key))
            .Select(v => v.Value)
            .ToList();

        if (group.Count > 0)
        {
            // 그룹으로 잡을 엔티티가 있는 경우에만 카메라 라이브
            manager.LiveGroupCamera(group);
        }
    }

    public void FocusGroup(List<GameObject> groupObjs)
    {
        // 중심점이 등록된 오브젝트만 그룹으로 선정
        List<Transform> group = groupObjs
            .Select(obj => bodyTransforms.GetValueOrDefault(obj.GetInstanceID()))
            .Where(transform => transform != null)
            .ToList();

        if (group.Count > 0)
        {
            // 그룹으로 잡을 엔티티가 있는 경우에만 카메라 라이브
            manager.LiveGroupCamera(group);
        }
    }

    public void FocusSingle(GameObject entityObj)
    {
        int instanceID = entityObj.GetInstanceID();

        if (bodyTransforms.ContainsKey(instanceID))
        {
            var cmOffset = manager.singleCam.GetComponent<CinemachineCameraOffset>();

            // 카메라 위치 조정
            cmOffset.m_Offset.x = 0;
            cmOffset.m_Offset.y = 0;

            // 카메라 라이브
            manager.LiveSingleCamera(bodyTransforms[instanceID]);
        }
    }

    public void FocusSelection(GameObject entityObj)
    {
        int instanceID = entityObj.GetInstanceID();

        if (bodyTransforms.ContainsKey(instanceID))
        {
            var cmOffset = manager.singleCam.GetComponent<CinemachineCameraOffset>();

            // 카메라 위치 조정
            cmOffset.m_Offset.x = x;
            cmOffset.m_Offset.y = y;

            // 카메라 라이브
            manager.LiveSingleCamera(bodyTransforms[instanceID]);
        }
    }

    /***************************************************************
    * [ 카메라 연출 ]
    * 
    * 카메라 시점을 옮기며 연출
    ***************************************************************/

    public IEnumerator DirectBattleStart()
    {
        // 전투 시작 상황을 위한 플레이어 그룹 카메라 잡아주기
        // (기습 or 일반 or 역기습 애니메이션 연출)
        FocusPlayerGroup();
        yield return new WaitForSeconds(3.5f); // 현재는 시간이지만 나중엔 애니메이션이 끝나는데로

        // 전체적인 상황 보여주기
        FocusFullScreen();
        yield return new WaitForSeconds(2.5f);
    }

    public IEnumerator DirectActionSelection()
    {
        yield return null;
    }

    public IEnumerator DirectActionMotion(Entity actor, Entity target)
    {
        // 단일 타겟을 리스트로 변환
        List<Entity> list = new List<Entity>() { target };
        yield return DirectActionMotion(actor, list);
    }

    public IEnumerator DirectActionMotion(Entity actor, List<Entity> targets)
    {
        // 행동을 하는 캐릭터부터 카메라 잡기
        FocusSingle(actor.gameObject);

        // 모션 동안 대기
        yield return new WaitWhile(() => actor.IsActing);

        // 타겟이 되는 대상 그룹샷
        FocusGroup(targets.Select(t => t.gameObject).ToList());
    }

    public IEnumerator DirectSelectMotion()
    {
        var cmOffset = manager.singleCam.GetComponent<CinemachineCameraOffset>();

        Sequence seq = DOTween.Sequence()
        .AppendInterval(0.5f)
        .Append(DOTween.To(
            () => cmOffset.m_Offset.x,
            x => cmOffset.m_Offset.x = x,
            x,
            0.4f
        ).SetEase(Ease.InCirc))
        .Join(DOTween.To(
            () => cmOffset.m_Offset.y,
            y => cmOffset.m_Offset.y = y,
            y,
            0.4f
        ).SetEase(Ease.InCirc));

        // DOTween 애니메이션이 끝날 때까지 대기
        yield return seq.WaitForCompletion();
    }
}