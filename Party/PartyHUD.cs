using System.Collections.Generic;
using UnityEngine;

public class PartyHUD : MonoBehaviour
{
    [Header("HUD")]
    public Transform hudContainer;
    public GameObject hudPrefab;

    private Dictionary<string, HUD> partyHUDs = new Dictionary<string, HUD>();

    private void Start()
    {
        List<CharacterData> party = PartyData.Instance.GetPartyMembers();

        foreach (CharacterData entity in party)
        {
            GameObject hudObj = Instantiate(hudPrefab, hudContainer);
            HUD hud = hudObj.GetComponent<HUD>();

            hud.InitData(entity);

            partyHUDs[entity.Name] = hud;
        }
    }

    public void UpdateStat()
    {
        // 데이터 로드로 전체적인 데이터가 변한 경우
        List<CharacterData> party = PartyData.Instance.GetPartyMembers();
        foreach (CharacterData entity in party)
        {
            EntityStats stat = entity.Stats;
            partyHUDs[entity.Name].UpdateHP(stat.HP, stat.MaxHP);
            partyHUDs[entity.Name].UpdateSP(stat.SP, stat.MaxSP);
        }
    }

    public void UpdateHP(string name, int hp, int maxHP)
    {
        partyHUDs[name].UpdateHP(hp, maxHP);
    }

    public void UpdateSP(string name, int sp, int maxSP)
    {
        partyHUDs[name].UpdateSP(sp, maxSP);
    }
}