using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPicture : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Card _cardPlaceholder;
    private Image _image;
    private RectTransform _rectTransform;

    public RectTransform rectTransform => _rectTransform;
    public float speed => _speed;

    public void Awake() {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(Card card)
    {
        gameObject.SetActive(true);
        _image.sprite = Resources.Load<Sprite>($"Cards/{card.Suit}_{card.Rank}");
        _cardPlaceholder = card;
        transform.SetParent(CardPicturesTransformParent.instance.transform);
    }

    public void Update() {
        _rectTransform.position = Vector2.MoveTowards(
            _image.rectTransform.position,
            _cardPlaceholder.rectTransform.position,
            _speed * Time.deltaTime
        );
    }
}
