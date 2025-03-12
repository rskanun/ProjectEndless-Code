using System.Collections.Generic;

public class InteractionBridge
{
    private static InteractionBridge _instance;
    public static InteractionBridge Instance
    {
        get
        {
            if (_instance == null)
                _instance = new InteractionBridge();

            return _instance;
        }
    }

    private InteractManager interactManager;

    public void RegisterManager(InteractManager interactManager)
    {
        this.interactManager = interactManager;
    }

    public void RemoveManager()
    {
        interactManager = null;
    }

    public List<Npc> GetTalkableNPCs()
    {
        // 상호작용 매니져가 모종의 이유로 사용 불가능한 경우 null값 리턴
        if (interactManager == null) return null;

        List<Npc> interactableObjs = interactManager.GetInteractableObjects();

        // 상호작용 가능한 오브젝트의 목록에서 대화 기능을 가진 NPC 분리
        return interactableObjs;
    }
}