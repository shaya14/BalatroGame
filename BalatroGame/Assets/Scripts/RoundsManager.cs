using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RoundsManager : MonoBehaviour
{
    private static RoundsManager _instance;
    [SerializeField] private List<Round> _rounds;
    private int _currentRoundIndex = 0;

    public static RoundsManager Instance => _instance;
    public int CurrentRoundIndex => _currentRoundIndex;

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

        _rounds = new List<Round>(GetComponentsInChildren<Round>());
        Debug.Log("Current round index: " + _currentRoundIndex);
    }

    public void SetRoundCompleted(int index)
    {
        if (index < _rounds.Count)
        {
            _rounds[index].SetIsCompleted(true);
            _rounds[index].SetIsAvailable(false);
            if (index + 1 < _rounds.Count)
                _rounds[index + 1].SetIsAvailable(true);
            GameHandler.Instance.UpdatePlayerMoney(_rounds[index].MoneyReward);
            _currentRoundIndex++;
        }
    }

    public void ResetHandsAndPoints()
    {
        PointsHandler.Instance.ResetTotalPoints();
        GameHandler.Instance.ResetHandsAndDiscards();
    }
}
