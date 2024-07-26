using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Drawing;

public class GameHandler : MonoBehaviour
{
    // Singleton instance
    private static GameHandler _instance;
    public static GameHandler Instance => _instance;

    // Serialized fields
    [SerializeField] private int _remianingHands;
    [SerializeField] private int _remianingDiscards;
    [SerializeField] private int _pointsToWin;
    private int _originalRemianingHands;
    private int _originalRemianingDiscards;

    // Serialized Text fields
    [SerializeField] private TextMeshProUGUI _remainingHandsText;
    [SerializeField] private TextMeshProUGUI _remainingDiscardsText;
    [SerializeField] private TextMeshProUGUI _pointsToWinText;

    // Properties
    public int RemainingHands => _remianingHands;
    public int RemainingDiscards => _remianingDiscards;
    public int PointsToWin => _pointsToWin;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _remainingHandsText.text = $"Hands: {_remianingHands}";
        _remainingDiscardsText.text = $"Discards: {_remianingDiscards}";
        _pointsToWinText.text = $"Win: {_pointsToWin}";

        _originalRemianingHands = _remianingHands;
        _originalRemianingDiscards = _remianingDiscards;
    }

    private void Update()
    {
        if (_remianingHands <= 0 && !ListsManager.Instance.IsPlayingHand && _pointsToWin > PointsHandler.Instance.TotalPoints)
        {
            Time.timeScale = 0;
            GameManager.Instance.GameOver();
        }
        else if (_pointsToWin <= PointsHandler.Instance.TotalPoints && !ListsManager.Instance.IsPlayingHand)
        {
            GameManager.Instance.SetRoundPanelActive(true);
            RoundsManager.Instance.SetRoundCompleted(RoundsManager.Instance.CurrentRoundIndex);
            RoundsManager.Instance.ResetHandsAndPoints();
            Deck.Instance.CleanLastDeck();
            ListsManager.Instance.CleanHand();
        }
    }

    public void ResetHandsAndDiscards()
    {
        _remianingHands = _originalRemianingHands;
        _remianingDiscards = _originalRemianingDiscards;
        _remainingHandsText.text = $"Hands: {_remianingHands}";
        _remainingDiscardsText.text = $"Discards: {_remianingDiscards}";
    }

    public void UpdateReminingHands()
    {
        _remianingHands--;
        _remainingHandsText.text = $"Hands: {_remianingHands}";
    }

    public void UpdateReminingDiscards()
    {
        _remianingDiscards--;
        _remainingDiscardsText.text = $"Discards: {_remianingDiscards}";
    }

    public void SetPointsToWin(int points)
    {
        _pointsToWin = points;
        _pointsToWinText.text = $"Win: {_pointsToWin}";
    }
}
