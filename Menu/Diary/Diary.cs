using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Diary : MonoBehaviour
{
    [Header("프로필 구성")]
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI occupationField;
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI abilityField;
    [SerializeField] private TextMeshProUGUI hobbyField;
    [SerializeField] private TextMeshProUGUI sanField;

    [Header("스탯 구성")]
    [SerializeField] private AmountTextBar hpBar;
    [SerializeField] private AmountTextBar spBar;
    [SerializeField] private TextMeshProUGUI strField;
    [SerializeField] private TextMeshProUGUI defField;
    [SerializeField] private TextMeshProUGUI agiField;
    [SerializeField] private TextMeshProUGUI dexField;
    [SerializeField] private TextMeshProUGUI mpField;

    [Header("장비 구성")]
    [SerializeField] private EquipInfo weaponField;
    [SerializeField] private EquipInfo offWeaponField;
    [SerializeField] private EquipInfo AccesssoryField1;
    [SerializeField] private EquipInfo AccesssoryField2;

    public void UpdateDiary(CharacterData character)
    {
        // 프로필 설정
        nameField.text = character.Name;
        profileImage.sprite = character.Profile.ProfileImage;
        occupationField.text = character.Profile.Occupation;
        abilityField.text = character.Profile.Ability;
        hobbyField.text = character.Profile.Hobby;
        sanField.text = GetSanToText(character);

        // 스탯 설정
        hpBar.UpdateAmount(character.Stat.HP, character.Stat.MaxHP);
        spBar.UpdateAmount(character.Stat.SP, character.Stat.MaxSP);
        strField.text = character.Stat.STR.ToString();
        defField.text = character.Stat.DEF.ToString();
        agiField.text = character.Stat.AGI.ToString();
        dexField.text = character.Stat.DEX.ToString();
        mpField.text = character.Stat.MaxMP.ToString();

        // 장비 설정
        weaponField.UpdateInfo(character.MainWeapon);
        offWeaponField.UpdateInfo(character.OffWeapon);
        AccesssoryField1.UpdateInfo(character.Accessory1);
        AccesssoryField2.UpdateInfo(character.Accessory2);
    }

    private string GetSanToText(CharacterData character)
    {
        // 플레이어의 정신상태는 접근 불가
        if (character is PlayerData) return "알 수 없음";

        if (character.Stat.SAN >= 60) return "안정";
        else if (character.Stat.SAN >= 20) return "불안";
        else return "붕괴";
    }
}