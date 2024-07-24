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
    [SerializeField] private float _fadeDuration = 2.0f;

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
        _pointsText.text = $"+{_points}";
        _totalPointsText.text = $"Total: {_totalPoints}";
        _multText.text = $"x{_mult}";
    }
    public void SetBasePointsAndMults(int points, float mult)
    {
        _points = points;
        _mult = mult;
        _pointsText.text = $"+{_points}";
        _multText.text = $"x{_mult}";
    }

    public void AddPoints(int points)
    {
        _points += points;
        _pointsText.text = $"+{_points}";
        //StartCoroutine(FadeText(_pointsText));
    }

    public void AddMult(float mult)
    {
        _mult = mult;
        _multText.text = $"x{_mult}";
        //StartCoroutine(FadeText(_multText));
    }

    public void AddTotalPoints(int points)
    {
        _totalPoints += points;
        _totalPointsText.text = $"Total: {_totalPoints}";
        //StartCoroutine(FadeText(_totalPointsText));
    }

    public void CalculateTotalPoints()
    {
        int total = _points * (int)_mult;
        AddTotalPoints(total);
        _points = 0;
        _pointsText.text = $"+{_points}";
        _mult = 1;
        _multText.text = $"x{_mult}";
    }

    // private IEnumerator FadeText(TextMeshProUGUI text)
    // {
    //     text.CrossFadeAlpha(0, 0, true);
    //     text.CrossFadeAlpha(1, _fadeDuration, true);
    //     yield return new WaitForSeconds(_fadeDuration);
    //     text.CrossFadeAlpha(0, _fadeDuration, true);
    // }
}
