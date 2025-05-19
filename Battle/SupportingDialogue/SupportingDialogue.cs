using DG.Tweening;
using TMPro;
using UnityEngine;

public class SupportingDialogue : MonoBehaviour
{
    [SerializeField] private CanvasGroup dialogue;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("대사창 설정")]
    [SerializeField] private float duration;
    [SerializeField] private SupportingAnnouncer announcer;

    private void DialoguePrint(string text)
    {
        string replaceText = StringReplacer.ReplaceKeywords(text);
        DOTween.Sequence()
            .OnStart(() =>
            {
                dialogue.alpha = 1.0f; // 대화창 활성화
                dialogueText.text = replaceText; // 대화창 대사 삽입
            })
            .Insert(duration, dialogue.DOFade(0.0f, 0.5f));
    }

    /***************************************************************
    * [ 개전 대사 ]
    * 
    * 전투 시작 시, 현재 상황에 따라서 출력되는 대사
    ***************************************************************/

    public void PrintBattleStart()
    {
        DialoguePrint(announcer.BattleStart);
    }

    /***************************************************************
    * [ 반격 대사 ]
    * 
    * 패링에 성공하여 적이 흐트러진 상태가 되었을 때 출력되는 대사
    ***************************************************************/

    public void PrintParryingSuccess()
    {
        DialoguePrint(announcer.ParryingSuccess);
    }

    /***************************************************************
    * [ 처리 대사 ]
    * 
    * 적을 처리했을 때 출력되는 대사
    ***************************************************************/

    public void PrintKillEnemy()
    {
        DialoguePrint(announcer.KillEnemy);
    }

    /***************************************************************
    * [ 기절 대사 ]
    * 
    * 플레이어의 파티 중 누군가 쓰러진 경우 출력되는 대사
    ***************************************************************/

    public void PrintKnockdownPlayer()
    {
        DialoguePrint(announcer.KnockdownPlayer);
    }

    /***************************************************************
    * [ 승리 대사 ]
    * 
    * 전투에서 승리 시 출력되는 대사
    ***************************************************************/

    public void PrintVictory()
    {
        DialoguePrint(announcer.BattleVictory);
    }
}