using UnityEngine;

namespace Assets.Script.System.Player
{
    public class InteractionManager : MonoBehaviour
    {
        [Header("플레이어 데이터")]
        [SerializeField] private PlayerData player;

        // 참조 스크립터블 오브젝트
        private OptionSetting option;
        private PlayerState playerState;

        private void Start()
        {
            option = OptionSetting.Instance;
            playerState = PlayerState.Instance;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (playerState.IsAttacking == false)
            {
                // 맞닿은 오브젝트가 NPC일 시
                if (collision.CompareTag(Tag.NPC))
                {
                    // 해당 NPC의 정보를 가져오기
                    player.Npc = collision.gameObject.GetComponent<NPC>();
                    Debug.Log("keydown " + option.Interact.ToString());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (playerState.IsAttacking == false)
            {
                // 맞닿은 오브젝트가 NPC일 시
                if (collision.CompareTag(Tag.NPC))
                {
                    // NPC의 정보를 초기화
                    player.Npc = null;
                    Debug.Log("exit");
                }
            }
        }
    }
}