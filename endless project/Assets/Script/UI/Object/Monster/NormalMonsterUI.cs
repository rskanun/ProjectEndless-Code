using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Script.UI.Object.Monster
{
    public class NormalMonsterUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private TextMeshProUGUI manaText;

        public void updateHpText(int maxHP, int nowHP)
        {
            hpText.text = "체력: " + maxHP + " / " + nowHP;
        }

        public void updateManaText(int maxMana, int nowMana)
        {
            manaText.text = "마나: " + maxMana + " / " + nowMana;
        }
    }
}