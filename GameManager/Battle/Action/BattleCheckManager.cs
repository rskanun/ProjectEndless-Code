using System.Collections.Generic;
using UnityEngine;

public class BattleCheckManager : MonoBehaviour
{
    public GameObject linePrefab;
    public Transform container;

    [Header("참조 스크립트")]
    [SerializeField] private Timeline timeline;

    private BattleSequence seq;

    private void Awake()
    {
        seq = BattleData.Instance.Sequence;
    }

    public void OnStartChecking()
    {
        // 첫 타임라인부터 살피기
        CheckingAction(0);
    }

    public void OnEndChecking()
    {

    }

    private void CheckingAction(int index)
    {
        // 타임라인 옮기기
        timeline.MoveIndex(index);

        // 해당 행동 화면상에 띄우기
        BattleAction action = seq.GetTurnAction(index);

        Entity actor = action.actor;
        Vector2 actorPos = actor.transform.position;

        List<Vector2> targetsPos = GetTargetsPos(action);

        CreateArrows(actorPos, targetsPos);
    }

    private List<Vector2> GetTargetsPos(BattleAction action)
    {
        scriptList.Select(script => script.transform.position).ToList();
    }

    private void CreateArrows(Vector2 actor, List<Vector2> targets)
    {
        foreach (Vector2 target in targets)
        {
            GameObject arrow = Instantiate(linePrefab, container);
            DottedArrowLine line = arrow.GetComponent<DottedArrowLine>();

            line.DrawLine(actor, target);
        }
    }
}