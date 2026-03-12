using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class BattleEvent
{
    [Header("이벤트 트리거")]
    [SerializeField] private EventTrigger mainTrigger;
    [SerializeField] private List<AddEventTrigger> addTriggers;

    [Header("이벤트")]
    [SerializeField] private List<EventAction> actions;
}