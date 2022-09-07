using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    [SerializeField]
    private Player player;
    
    void Awake()
    {
        player.ap = player.mp;
    }

    public void setAP(int ap)
    {

    }

    IEnumerator barAnimation(Image bar, int before, int after, int max)
    {
        float movePer = (before - after) / 10.0f;
        for(; before != after; bar.fillAmount -= movePer / 100)
        {
            yield return new WaitForSeconds(0.01f);
        }
    }


}
