using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Card : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _suit;
    [SerializeField] private string _rank;
    [SerializeField] private float _fadeDuration = 2.0f;

    // Private fields
    private int _pointsValue;

    // Properties
    public string Suit => _suit;
    public string Rank => _rank;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        LoadCardSprite();
    }

    public void InitCard(string suit, string rank)
    {
        this._suit = suit;
        this._rank = rank;
        SetPointsValue(rank);
        LoadCardSprite();
    }

    private void LoadCardSprite()
    {
        _image.sprite = Resources.Load<Sprite>($"Cards/{_suit}_{_rank}");
    }

    public void SetPointsValue(string rank)
    {
        _pointsValue = rank switch
        {
            "J" or "Q" or "K" => 10,
            "A" => 11,
            _ => int.TryParse(rank, out var value) ? value : 0
        };
        _pointsText.text = $"+{_pointsValue}";
    }

    public void SetTextEnabled(bool value)
    {
        _pointsText.gameObject.SetActive(value);
        if (value)
        {
            StartCoroutine(FadeOutText());
        }
    }

    private IEnumerator FadeOutText()
    {
        yield return new WaitForSeconds(0.1f);
        float elapsedTime = 0f;
        Color startColor = _pointsText.color;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / _fadeDuration);
            _pointsText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        _pointsText.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        _pointsText.gameObject.SetActive(false); // Optionally disable the text object after fade-out
    }

    public void SetSuit(string suit)
    {
        this._suit = suit;
        LoadCardSprite();
    }

    public void SetRank(string rank)
    {
        this._rank = rank;
        LoadCardSprite();
    }
}
