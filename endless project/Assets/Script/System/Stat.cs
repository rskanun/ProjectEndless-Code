using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    [SerializeField]
    private Player player;

    public Image apBar;
    public Image spBar;
    
    void Awake()
    {
        // 초기 AP값 설정
        player.ap = player.mp;
        
    }

    public void setAP(int ap)
    {
        StartCoroutine(barAnimation(apBar, player.ap, ap, player.maxAP));
    }

    IEnumerator barAnimation(Image bar, int before, int after, int max)
    {
        // 깎일 양의 Bar를 10틱으로 나눠 애니메이션의 형태로 보여주기
        float movePer = (before - after) / 10.0f;
        while(before != after)
        {
            bar.fillAmount -= movePer / 100;
            yield return new WaitForSeconds(0.01f);
        }
    }


}
