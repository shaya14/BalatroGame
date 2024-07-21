using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Transform _parentToReturnTo = null;
    GameObject _placeHolder = null;
    private bool _toggleClick = false;
    [SerializeField] private Card _thisCard;
    [SerializeField] private DisableCanvas _disableCanvas;

    private void Awake()
    {
        _thisCard = GetComponent<Card>();
        _disableCanvas = GetComponentInParent<DisableCanvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _placeHolder = new GameObject();
        _placeHolder.transform.SetParent(this.transform.parent);
        RectTransform rectTransform = _placeHolder.AddComponent<RectTransform>();
        rectTransform.sizeDelta = this.GetComponent<RectTransform>().sizeDelta;
        _placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        _parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _disableCanvas.DisableCanvasGroup();
        this.transform.position = eventData.position;

        if (_placeHolder.transform.parent != _parentToReturnTo)
        {
            _placeHolder.transform.SetParent(_parentToReturnTo);
        }

        int newSiblingIndex = _parentToReturnTo.childCount;

        for (int i = 0; i < _parentToReturnTo.childCount; i++)
        {
            if (this.transform.position.x < _parentToReturnTo.GetChild(i).position.x)
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

    public void OnEndDrag(PointerEventData eventData)
    {
        _disableCanvas.EnableCanvasGroup();
        this.transform.SetParent(_parentToReturnTo);
        this.transform.SetSiblingIndex(_placeHolder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(_placeHolder);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_toggleClick)
        {
            ListsManager.Instance.UpdateSelecedCard(_thisCard);
            this.transform.localScale = new Vector3(1f, 1f, 1f);
            _toggleClick = false;
        }
        else if (ListsManager.Instance.SelectedCards.Count < 5)
        {
            ListsManager.Instance.UpdateSelecedCard(_thisCard);
            _toggleClick = true;
        }
    }

    public void SetDisableCanvas(DisableCanvas disableCanvas)
    {
        _disableCanvas = disableCanvas;
    }
}
