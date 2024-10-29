using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private TalkManager talkManager;

    // 상호작용이 가능한 오브젝트 목록
    private List<NPC> npcs = new List<NPC>();

    public void RotateEyes(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            // 멈췄을 때는 반영 안 하기
            return;
        }

        // 해당 방향으로 시야각 돌리기
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90.0f;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void OnInteract()
    {
        if (npcs.Count <= 0)
        {
            // 상호작용 할 오브젝트가 없다면 무시
            return;
        }

        // 가장 처음 접근한 오브젝트와 상호작용
        talkManager.StartTalk(npcs[0]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 맞닿은 오브젝트가 상호작용 가능할 경우
        if (collision.CompareTag("NPC"))
        {
            // 해당 오브젝트의 정보를 가져오기
            NPC npc = collision.gameObject.GetComponent<NPC>();
            npcs.Add(npc);

            Debug.Log($"keydown Space");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            NPC npc = collision.gameObject.GetComponent<NPC>();

            // 범위에서 벗어난 오브젝트가 현재 상호작용 가능한 오브젝트일 경우
            if (npcs.Contains(npc))
            {
                // 오브젝트의 정보를 초기화
                npcs.Remove(npc);
                Debug.Log($"{collision.name} exit");
            }
        }
    }
}