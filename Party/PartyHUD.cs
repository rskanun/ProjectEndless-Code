using System.Collections.Generic;
using UnityEngine;

public class PartyHUD : MonoBehaviour
{
    [Header("HUD")]
    public Transform hudContainer;
    public GameObject hudPrefab;

    // ÆÄÆ¼ HUD
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

    public void UpdateHP(string name, int hp, int maxHP)
    {
        partyHUDs[name].UpdateHP(hp, maxHP);
    }

    public void UpdateSP(string name, int sp, int maxSP)
    {
        partyHUDs[name].UpdateSP(sp, maxSP);
    }
}