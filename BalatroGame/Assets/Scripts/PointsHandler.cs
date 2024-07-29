using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PointsHandler : MonoBehaviour
{
    // Singleton instance
    private static PointsHandler _instance;
    public static PointsHandler Instance => _instance;

    // Serialized fields
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private TextMeshProUGUI _totalPointsText;
    [SerializeField] private TextMeshProUGUI _multText;

    // Private fields
    private int _points;
    private int _totalPoints;
    private float _mult;

    // Properties
    public int Points => _points;
    public int TotalPoints => _totalPoints;

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
        _points = 0;
        _totalPoints = 0;
        _mult = 1;
        _pointsText.text = $"{_points}";
        _totalPointsText.text = $"Total: {_totalPoints}";
        _multText.text = $"{_mult}";
    }
    public void SetBasePointsAndMults(int points, float mult)
    {
        _points = points;
        _mult = mult;
        _pointsText.text = $"{_points}";
        _multText.text = $"{_mult}";
    }

    public void ResetTotalPoints()
    {
        _totalPoints = 0;
        _totalPointsText.text = $"Total: {_totalPoints}";
    }

    public void AddPoints(int points)
    {
        _points += points;
        _pointsText.text = $"{_points}";
    }

    public void AddPoints(int points, bool isJoker)
    {
        if (isJoker)
        {
            StartCoroutine(LerpPointsToPoints(points));
            _points += points;
            _pointsText.text = $"{_points}";
        }
    }

    public void AddMult(float mult)
    {
        StartCoroutine(LerpMultToTotalMult(mult));
        //_mult += mult;
        //_multText.text = $"{_mult}";
    }

    public void AddTotalPoints(int points)
    {
        StartCoroutine(LerpPointsToTotalPoints(points));
    }

    public void CalculateTotalPoints()
    {
        int total = _points * (int)_mult;
        AddTotalPoints(total);
        _mult = 1;
        _multText.text = $"{_mult}";
        PokerSystem.Instance.SetTextToEmpty();
    }

    private IEnumerator LerpMultToTotalMult(float mult)
    {
        float time = 0;
        float duration = 0.5f;

        float startMult = _mult;
        float endMult = _mult + mult;

        while (time < duration)
        {
            time += Time.deltaTime;
            _mult = (int)Mathf.Lerp(startMult, endMult, time / duration);
            _multText.text = $"{_mult}";
            yield return null;
        }
    }

    private IEnumerator LerpPointsToPoints(int points)
    {
        float time = 0;
        float duration = 1;

        int startPoints = _points;
        int endPoints = _points + points;

        while (time < duration)
        {
            time += Time.deltaTime;
            _points = (int)Mathf.Lerp(startPoints, endPoints, time / duration);
            _pointsText.text = $"{_points}";
            yield return null;
        }
    }

    private IEnumerator LerpPointsToTotalPoints(int points)
    {
        float time = 0;
        float duration = 1;

        // Total points
        int startPoints = _totalPoints;
        int endPoints = _totalPoints + points;

        // Points to add
        int startAddPoints = _points;
        int endAddPoints = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            _totalPoints = (int)Mathf.Lerp(startPoints, endPoints, time / duration);
            _points = (int)Mathf.Lerp(startAddPoints, endAddPoints, time / duration);
            _totalPointsText.text = $"Total: {_totalPoints}";
            _pointsText.text = $"{_points}";
            yield return null;
        }
    }
}
