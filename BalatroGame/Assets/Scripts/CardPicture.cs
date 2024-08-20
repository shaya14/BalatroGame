using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPicture : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float _speed;

    private Card _cardPlaceholder;
    private Image _image;
    private RectTransform _rectTransform;

    private bool _isDragging;

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
        if (_isDragging) {
            return;
        }

        _rectTransform.position = Vector2.MoveTowards(
            _image.rectTransform.position,
            _cardPlaceholder.rectTransform.position,
            _speed * Time.deltaTime
        );
    }

    // TODO: make sure it is not possible to drag cards during drawing/playing animations.
    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.position = eventData.position;

        var index = _cardPlaceholder.transform.GetSiblingIndex();

        if (index > 0) {
            Card previousCard = _cardPlaceholder.transform.parent.GetChild(index - 1).GetComponent<Card>();
            if (eventData.position.x < previousCard.rectTransform.position.x) {
                _cardPlaceholder.transform.SetSiblingIndex(index - 1);
                return;
            }
        }
        
        if (index < _cardPlaceholder.transform.parent.childCount - 1) {
            Card nextCard = _cardPlaceholder.transform.parent.GetChild(index + 1).GetComponent<Card>();
            if (eventData.position.x > nextCard.rectTransform.position.x) {
                _cardPlaceholder.transform.SetSiblingIndex(index + 1);
                return;
            }
        }
    
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!eventData.dragging) {
            _cardPlaceholder.OnClick(eventData);
        }
    }


}
