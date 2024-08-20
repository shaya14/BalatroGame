using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    // Serialized fields
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _speed;

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

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void InitCard(string suit, string rank)
    {
        this._suit = suit;
        this._rank = rank;
        LoadCardSprite();
        _pointsText.text = $"+{PointsValue}";
    }

    public void Update() {
        _image.rectTransform.localPosition = Vector2.MoveTowards(
          _image.rectTransform.localPosition,
          Vector2.zero,
          _speed * Time.deltaTime
        );
    }

    private void MovePlaceholder(Vector2 placeholderLocalPosition) {
      Vector2 pictureGlobalPosition = _image.rectTransform.position;

      _rectTransform.localPosition = placeholderLocalPosition;
      _image.rectTransform.position = pictureGlobalPosition;
    }

    private void LoadCardSprite()
    {
        _image.sprite = Resources.Load<Sprite>($"Cards/{_suit}_{_rank}");
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

  public int PointsValue { get {
      return _rank switch
        {
            "J" or "Q" or "K" => 10,
            "A" => 11,
            _ => int.TryParse(_rank, out var value) ? value : 0
        };
    }}

  public void OnPointerClick(PointerEventData eventData) {
  {
      if (ListsManager.Instance.IsCardSelected(this))
      {
          ListsManager.Instance.RemoveCardFromSelection(this);
          MovePlaceholder(new Vector3(_rectTransform.localPosition.x, _unselectedY, _rectTransform.localPosition.z));
      }
      else if (ListsManager.Instance.SelectedCards.Count < 5 && !ListsManager.Instance.IsPlayingHand)
      {
          ListsManager.Instance.AddCardToSelection(this);
          MovePlaceholder(new Vector3(_rectTransform.localPosition.x, _selectedY, _rectTransform.localPosition.z));
      }
    }
  }

}
