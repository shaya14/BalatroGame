using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions.Must;

public class LeftPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _remainingHandsText;
    [SerializeField] private TextMeshProUGUI _remainingDiscardsText;
    [SerializeField] private TextMeshProUGUI _pointsToWinText;
    [SerializeField] private TextMeshProUGUI _moneyText;

    void Start()
    {
        Update();
    }

    void Update()
    {
        _remainingHandsText.text = $"Hands: {GameHandler.Instance.RemainingHands}";
        _remainingDiscardsText.text = $"Discards: {GameHandler.Instance.RemainingDiscards}";
        _pointsToWinText.text = $"Win: {GameHandler.Instance.PointsToWin}";
        _moneyText.text = $"Money: ${GameHandler.Instance.PlayerMoney}";
    }
}
