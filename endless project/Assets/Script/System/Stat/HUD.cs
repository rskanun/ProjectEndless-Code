using Assets.Script.UI;
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
        public int HP
        {
            set
            {
                player.HP = value;
                hpUI.barUpdate(nowHP, player.HP, player.MaxHP);

                nowHP = value;
            }
            get { return nowHP; }
        }
        private int nowAP; // ap의 값이 변경되기 이전의 값
        public int AP
        {
            set
            {
                player.AP = value;
                apUI.barUpdate(player.AP, player.MaxAP);

                nowAP = value;
            }
            get { return nowAP; }
        }

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
        * [ 변수 체크 ]
        * 
        * 플레이어의 hp와 ap의 애니메이션이 제대로 실행되지 않았는지 체크
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
            if (e.PropertyName == "HP")
            {
                HP = player.HP;
            }

            if (e.PropertyName == "AP")
            {
                AP = player.AP;
            }
        }
    }
}