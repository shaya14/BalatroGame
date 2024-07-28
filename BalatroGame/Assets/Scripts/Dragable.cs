using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Serialized fields
    [SerializeField] private Card _thisCard;
    [SerializeField] private DisableCanvas _disableCanvas;
    [SerializeField] private float _originalPositon = 145.74f;

    // Private fields
    private Transform _parentToReturnTo;
    private GameObject _placeHolder;
    private bool _toggleClick;
    private RectTransform _rectTransform;

    private Transform _originalPositonForJoker;

    private void Awake()
    {
        _thisCard = GetComponent<Card>();
        _disableCanvas = GetComponentInParent<DisableCanvas>();
        _rectTransform = GetComponent<RectTransform>();
        _originalPositonForJoker = GetComponent<Transform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CreatePlaceholder();

        _parentToReturnTo = transform.parent;
        transform.SetParent(transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_disableCanvas != null)
        {
            _disableCanvas.DisableCanvasGroup();
        }
        transform.position = eventData.position;
        UpdatePlaceholderPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_disableCanvas != null)
            _disableCanvas.EnableCanvasGroup();

        transform.SetParent(_parentToReturnTo);
        transform.SetSiblingIndex(_placeHolder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(_placeHolder);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Joker currentJoker = GetComponent<Joker>();
        
        if (currentJoker.IsBought)
        {
            return;
        }

        if (_toggleClick)
        {
            if (currentJoker && !currentJoker.IsBought)
            {
                _rectTransform.localPosition = new Vector3(_rectTransform.localPosition.x, _originalPositon, _rectTransform.localPosition.z);
                PlayerActions.Instance.GetJoker(null);
                GetComponent<Joker>().EnableBuyButton(false);
                _toggleClick = false;
                return;
            }

            ListsManager.Instance.UpdateSelectedCard(_thisCard);
            _rectTransform.localPosition = new Vector3(_rectTransform.localPosition.x, _originalPositon, _rectTransform.localPosition.z);
            _toggleClick = false;
        }
        else if (ListsManager.Instance.SelectedCards.Count < 5 && !ListsManager.Instance.IsPlayingHand)
        {
            if (currentJoker && !currentJoker.IsBought)
            {
                _rectTransform.localPosition = new Vector3(_rectTransform.localPosition.x, _originalPositon + 10, _rectTransform.localPosition.z);
                PlayerActions.Instance.GetJoker(currentJoker);
                GetComponent<Joker>().EnableBuyButton(true);
                _toggleClick = true;
                return;
            }

            ListsManager.Instance.UpdateSelectedCard(_thisCard);
            _toggleClick = true;
        }
    }

    public void SetDisableCanvas(DisableCanvas disableCanvas)
    {
        _disableCanvas = disableCanvas;
    }

    private void CreatePlaceholder()
    {
        _placeHolder = new GameObject();
        _placeHolder.transform.SetParent(transform.parent);
        RectTransform rectTransform = _placeHolder.AddComponent<RectTransform>();
        rectTransform.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        _placeHolder.transform.SetSiblingIndex(transform.GetSiblingIndex());
    }

    private void UpdatePlaceholderPosition()
    {
        if (_placeHolder.transform.parent != _parentToReturnTo)
        {
            _placeHolder.transform.SetParent(_parentToReturnTo);
        }

        int newSiblingIndex = _parentToReturnTo.childCount;

        for (int i = 0; i < _parentToReturnTo.childCount; i++)
        {
            if (transform.position.x < _parentToReturnTo.GetChild(i).position.x)
            {
                newSiblingIndex = i;
                if (_placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }
                break;
            }
        }

        _placeHolder.transform.SetSiblingIndex(newSiblingIndex);
    }
}
