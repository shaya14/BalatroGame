using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Round : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roundTitleText;
    [SerializeField] private TextMeshProUGUI _roundPointsText;
    [SerializeField] private string _roundTitle;
    [SerializeField] private int _roundPoints;
    [SerializeField] private bool _isRoundAvailable;
    [SerializeField] private Color _roundLockedColor;
    [SerializeField] private Color _roundCompletedColor;
    [SerializeField] private Button _startRoundButton;


    private Color _originalColor;
    private bool _isRoundCompleted;
    public int RoundPoints => _roundPoints;
    public bool IsRoundAvailablel => _isRoundAvailable;
    public bool IsRoundCompleted => _isRoundCompleted;

    private void Awake()
    {
        _originalColor = GetComponent<Image>().color;
    }

    private void Start()
    {
        _isRoundCompleted = false;

        if (!_isRoundAvailable)
        {
            GetComponent<Image>().color = _roundLockedColor;
            _startRoundButton.gameObject.SetActive(false);
        }

        _roundTitleText.text = _roundTitle;
        _roundPointsText.text = _roundPoints.ToString();
    }

    private void Update()
    {
        if (_isRoundCompleted)
        {
            GetComponent<Image>().color = _roundCompletedColor;
            _startRoundButton.gameObject.SetActive(false);
        }
    }

    public void SetIsCompleted(bool value)
    {
        _isRoundCompleted = value;
    }

    public void SetIsAvailable(bool value)
    {
        _isRoundAvailable = value;
        _startRoundButton.gameObject.SetActive(value);
        GetComponent<Image>().color = _originalColor;
    }
}
