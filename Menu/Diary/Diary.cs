using TMPro;
using UnityEngine;

public class Diary : MonoBehaviour
{
    [Header("다이어리 구성")]
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI occupationField;
    [SerializeField] private TextMeshProUGUI abilityField;
    [SerializeField] private TextMeshProUGUI hobbyField;
    [SerializeField] private TextMeshProUGUI sanField;

    [SerializeField] private AmountTextBar hpBar;
    [SerializeField] private AmountTextBar spBar;
    [SerializeField] private TextMeshProUGUI strField;
    [SerializeField] private TextMeshProUGUI defField;
    [SerializeField] private TextMeshProUGUI agiField;
    [SerializeField] private TextMeshProUGUI dexField;
    [SerializeField] private TextMeshProUGUI mpField;


}