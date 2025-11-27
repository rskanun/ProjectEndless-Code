using Sirenix.OdinInspector;
using UnityEngine;

public class BaseMap : Map
{
    [Title("회복 장소 및 이벤트")]
    [SerializeField]
    private Vector2 respawnPos;

    /// <summary>
    /// 전투에서 패배해 거점으로 돌아온 경우
    /// </summary>
    public new void OnEndBattle()
    {
        // 필드로 복귀한 게 아니라면 무시
        if (GameData.Instance.State != GameState.Field) return;

        // 전투에서 패배한 경우 이벤트 진행
        if (BattleCache.Current.Result == BattleResult.Defeat)
        {
            // 이것저것 따져서 루프하고 뭐하고 해야하지만
            // 일단은 회복 장소로 이동과 회복부터
            foreach (var character in PartyData.Instance.Characters)
            {
                character.Stats.HP = character.Stats.MaxHP;
                character.Stats.SP = character.Stats.MaxSP;
            }

            // HUD 업데이트
            GameEventManager.Instance.NotifyPartyStatsUpdate();

            // 플레이어 캐릭터 회복 장소로 이동
            PlayerManager.CurrentPlayer.Teleport(respawnPos);
        }
    }
}