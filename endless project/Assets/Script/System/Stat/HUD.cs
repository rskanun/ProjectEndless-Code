using Assets.Script.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Stat
{
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        private HealthPointBarUI ui;

        private float hp; // hp의 값이 변경되기 이전의 값

        void Start()
        {
            // init component
            ui = GetComponent<HealthPointBarUI>();

            initHpBar();
        }

        void initHpBar()
        {
            hp = player.hp;
            ui.setHpBar((float)player.hp / player.maxHp);
        }

        void Update()
        {
            if (hp != player.hp)
            {
                // 깎일 양의 Bar를 10틱으로 나눠 애니메이션의 형태로 보여주기
                ui.barUpdate(hp, player);

                hp = player.hp;
            }
        }
    }
}