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
    [SerializeField] private int _initialRemianingHands;
    [SerializeField] private int _initialRemianingDiscards;
    [SerializeField] private int _pointsToWin;
    [SerializeField] private int _playerMoney = 0;
    
    private int _remianingHands;
    private int _remianingDiscards;
    private bool _isCoroutineRunning = false;

    // Properties
    public int RemainingHands => _remianingHands;
    public int RemainingDiscards => _remianingDiscards;
    public int PointsToWin => _pointsToWin;
    public int PlayerMoney => _playerMoney;


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
        _remianingHands = _initialRemianingHands;
        _remianingDiscards = _initialRemianingDiscards;
    }

    private void Update()
    {
        if (_remianingHands <= 0 && !ListsManager.Instance.IsPlayingHand && _pointsToWin > PointsHandler.Instance.TotalPoints)
        {
            Time.timeScale = 0;
            GameManager.Instance.GameOver();
        }
        else if (_pointsToWin <= PointsHandler.Instance.TotalPoints && !ListsManager.Instance.IsPlayingHand && !_isCoroutineRunning)
        {
            StartCoroutine(WinRoundCourotine());
            ShopHandler.Instance.SetIsShopOpen(true);
        }
    }

    private IEnumerator WinRoundCourotine()
    {
        _isCoroutineRunning = true;

        yield return new WaitForSeconds(2);

        GameManager.Instance.SetRoundPanelActive(true);
        GameManager.Instance.SetShopPanel(true);
        RoundsManager.Instance.SetRoundCompleted(RoundsManager.Instance.CurrentRoundIndex);
        RoundsManager.Instance.ResetHandsAndPoints();
        Deck.Instance.CleanLastDeck();
        ListsManager.Instance.CleanHand();

        _isCoroutineRunning = false;
    }

    public void ResetHandsAndDiscards()
    {
        _remianingHands = _initialRemianingHands;
        _remianingDiscards = _initialRemianingDiscards;
    }

    public void UpdateReminingHands()
    {
        _remianingHands--;
    }

    public void UpdateReminingDiscards()
    {
        _remianingDiscards--;
    }

    public void SetPointsToWin(int points)
    {
        _pointsToWin = points;
    }

    public void UpdatePlayerMoney(int money)
    {
        _playerMoney += money;
    }
}
