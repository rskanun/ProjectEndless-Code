using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    // 상호작용이 가능한 오브젝트 목록
    private List<Npc> npcs = new List<Npc>();

    public void OnInteract()
    {
        if (npcs.Count <= 0)
        {
            // 범위 안에 대화할 대상이 없는 경우 무시
            return;
        }

        // 현재 상호작용 가능한 범위에 있는 대상 중 가장 먼저 범위에 들어온 대상과 대화
        TalkContext.Instance.ActiveDialogue(npcs[0]);
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 맞닿은 오브젝트가 상호작용 가능할 경우
        if (collision.CompareTag("NPC"))
        {
            // 해당 오브젝트의 정보를 가져오기
            Npc npc = collision.gameObject.GetComponent<Npc>();
            npcs.Add(npc);

            Debug.Log($"keydown Space");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            Npc npc = collision.gameObject.GetComponent<Npc>();

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