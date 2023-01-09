using Assets.Script.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Stat
{
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        private HealthPointBarUI hpUI;
        private AwakenPointBarUI apUI;

        private int hp; // hp의 값이 변경되기 이전의 값
        private int ap; // ap의 값이 변경되기 이전의 값

        private void Start()
        {
            // init component
            hpUI = GetComponent<HealthPointBarUI>();
            apUI= GetComponent<AwakenPointBarUI>();

            initHpBar();
        }

        private void initHpBar()
        {
            hp = player.hp;
            hpUI.setHPBar((float)player.hp / player.maxHp);
        }

        private void Update()
        {
            if (hp != player.hp)
            {
                // 깎일 양의 Bar를 10틱으로 나눠 애니메이션의 형태로 보여주기
                hpUI.barUpdate(hp, player);

                hp = player.hp;
            }

            if(ap != player.ap)
            {
                apUI.barUpdate(player);

                ap = player.ap;
            }
        }
    }
}