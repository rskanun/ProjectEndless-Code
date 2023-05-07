using Assets.Script.UI;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Script.System.Stat
{
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private PlayerData player;

        private HealthPointBarUI hpUI;
        private AwakenPointBarUI apUI;

        private int nowHP; // hp의 값이 변경되기 이전의 값
        private int nowAP; // ap의 값이 변경되기 이전의 값

        private void Start()
        {
            // init component
            hpUI = GetComponent<HealthPointBarUI>();
            apUI= GetComponent<AwakenPointBarUI>();

            initBar();
        }

        private void initBar()
        {
            // hp
            nowHP = player.HP;
            hpUI.setHPBar(player.HP, player.MaxHP);

            // ap
            nowAP = player.AP;
            apUI.setAPBar(player.AP, player.MaxAP);
        }

        /***************************************************************
        * [ 변수 변화 체크 ]
        * 
        * 플레이어의 hp와 ap의 변화에 따른 애니메이션 실행
        ***************************************************************/

        private void OnEnable()
        {
            player.PropertyChanged += OnPropertyChanged;
        }

        private void OnDisable()
        {
            player.PropertyChanged -= OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // hp 변화
            if (e.PropertyName == "HP")
            {
                onHpChanged();
            }

            // ap 변화
            if (e.PropertyName == "AP")
            {
                onApChanged();
            }
        }

        private void onHpChanged()
        {
            hpUI.barUpdate(nowHP, player.HP, player.MaxHP);

            nowHP = player.HP;
        }

        private void onApChanged()
        {
            apUI.barUpdate(player.AP, player.MaxAP);

            nowAP = player.AP;
        }
    }
}