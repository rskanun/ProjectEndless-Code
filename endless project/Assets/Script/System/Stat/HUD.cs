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

        private Coroutine hpCoroutine;
        private Coroutine apCoroutine;

        private int nowHP; // hp의 값이 변경되기 이전의 값
        public int HP
        {
            set
            {
                // 체크 코루틴과 겹침 방지
                StopCoroutine(hpCoroutine);

                player.hp = value;
                hpUI.barUpdate(nowHP, player.hp, player.maxHP);

                nowHP = value;

                hpCoroutine = StartCoroutine(checkingHP());
            }
            get { return nowHP; }
        }
        private int nowAP; // ap의 값이 변경되기 이전의 값
        public int AP
        {
            set
            {
                // 체크 코루틴과 겹침 방지
                StopCoroutine(apCoroutine);

                player.ap = value;
                apUI.barUpdate(player.ap, player.maxAP);

                nowAP = value;

                apCoroutine = StartCoroutine(checkingAP());
            }
            get { return nowAP; }
        }

        private void Start()
        {
            // init component
            hpUI = GetComponent<HealthPointBarUI>();
            apUI= GetComponent<AwakenPointBarUI>();

            initBar();

            hpCoroutine = StartCoroutine(checkingHP());
            apCoroutine = StartCoroutine(checkingAP());
        }

        private void initBar()
        {
            // hp
            nowHP = player.hp;
            hpUI.setHPBar(player.hp, player.maxHP);

            // ap
            nowAP = player.ap;
            apUI.setAPBar(player.ap, player.maxAP);
        }

        /***************************************************************
        * [ 변수 체크 ]
        * 
        * 플레이어의 hp와 ap의 애니메이션이 제대로 실행되지 않았는지 체크
        ***************************************************************/

        IEnumerator checkingHP()
        {
            WaitForSeconds wait = new WaitForSeconds(0.1f);

            while(true)
            {
                if (nowHP != player.hp)
                {
                    HP = player.hp;
                }

                yield return wait;
            }
        }

        IEnumerator checkingAP()
        {
            WaitForSeconds wait = new WaitForSeconds(0.1f);

            while(true)
            {
                if(nowAP != player.ap)
                {
                    AP = player.ap;
                }

                yield return wait;
            }
        }
    }
}