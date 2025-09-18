using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public static MoneyUI Instance;
    public TextMeshProUGUI moneyText;

    private int currentMoney = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        moneyText.text = currentMoney + " VND";

        // Hiện hiệu ứng +tiền nếu muốn
        Debug.Log("+ " + amount + " VND (Tổng: " + currentMoney + ")");
    }
}
