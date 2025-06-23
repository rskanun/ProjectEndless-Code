using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private void OnEnable()
    {
        DamagePopup.Instance.RegisterIndicator(this);
    }

    private void OnDisable()
    {
        DamagePopup.Instance.RemoveIndicator();
    }

    public void IndicateDamage(Vector2 pos, int damage)
    {
        GameObject damageObj = Instantiate(DamagePopup.Instance.DamagePrefab, transform);
        damageObj.transform.position = pos;

        TextMeshProUGUI textMesh = damageObj.GetComponent<TextMeshProUGUI>();
        textMesh.text = damage.ToString();
    }
}