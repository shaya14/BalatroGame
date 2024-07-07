using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] int _touchHeight;
    [SerializeField] float originalHeight = 50.775f; // Ask shimon how to get the original position of the object
    private Transform parentToReturnTo = null;


    private bool _toggleClick = false;
    private bool _isClicked = false;



    public void OnBeginDrag(PointerEventData eventData)
    {
        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.position = new Vector3(this.transform.position.x, originalHeight + _touchHeight, this.transform.position.z);
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isClicked)
            return;

        this.transform.position = new Vector3(this.transform.position.x, originalHeight, this.transform.position.z);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_toggleClick)
        {
            this.transform.position = new Vector3(this.transform.position.x, originalHeight, this.transform.position.z);
            _toggleClick = false;
            _isClicked = false;
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x, originalHeight + _touchHeight, this.transform.position.z);
            _isClicked = true;
            _toggleClick = true;
        }
    }
}
