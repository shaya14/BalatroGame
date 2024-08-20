using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

// A card consists of a placeholder, and a "CardPicture".
// The CardPicture separates from the parent transform on Init(), to avoid moving the placeholder
// together with the picture. This separation allows the dragging,drawing,selecting animations..
public class Card : MonoBehaviour
{
    // Serialized fields
    [SerializeField] private CardPicture _cardPicture;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private float _fadeDuration;

    // TODO: move this elsewhere
    [SerializeField] private float _unselectedY;
    [SerializeField] private float _selectedY;

    // Private fields
    private string _suit;
    private string _rank;

    // Components
    private RectTransform _rectTransform;

    // Properties
    public string Suit => _suit;
    public string Rank => _rank;
    public RectTransform rectTransform => _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void InitCard(string suit, string rank)
    {
        // TODO: remove 'this' - you already have the _ to signify a private-field.
        this._suit = suit;
        this._rank = rank;
        _cardPicture.Init(this);
        _pointsText.text = $"+{PointsValue}";
    }

    public void Hide() {
        _cardPicture.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
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

    public int PointsValue
    {
        get
        {
            return _rank switch
            {
                "J" or "Q" or "K" => 10,
                "A" => 11,
                _ => int.TryParse(_rank, out var value) ? value : 0
            };
        }
    }

    public void OnClick(PointerEventData eventData)
    {
        if (ListsManager.Instance.IsCardSelected(this))
        {
            ListsManager.Instance.RemoveCardFromSelection(this);
            _rectTransform.localPosition = new Vector3(
                _rectTransform.localPosition.x,
                _unselectedY,
                _rectTransform.localPosition.z
            );
        }
        else if (ListsManager.Instance.SelectedCards.Count < 5 && !ListsManager.Instance.IsPlayingHand)
        {
            ListsManager.Instance.AddCardToSelection(this);
            _rectTransform.localPosition = new Vector3(
                _rectTransform.localPosition.x,
                _selectedY,
                _rectTransform.localPosition.z
            );
        }
    }

    public float timeForPictureToReachPlaceholder =>
      Vector2.Distance(_rectTransform.position, _cardPicture.rectTransform.position)
      / _cardPicture.speed;

}
