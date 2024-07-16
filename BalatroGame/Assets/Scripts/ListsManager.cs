using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListsManager : MonoBehaviour
{
    private static ListsManager _instance;
    private List<Card> _selectedCards = new List<Card>();
    private List<Card> _hand = new List<Card>();


    public List<Card> SelectedCards => _selectedCards;
    public List<Card> Hand => _hand;
    public static ListsManager Instance => _instance;

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
    public void Update()
    {
        if (_selectedCards.Count > 0)
        {
            foreach (Card card in _selectedCards)
            {
                card.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }
        }
    }

    public void UpdateSelecedCard(Card card)
    {
        if (!_selectedCards.Contains(card))
        {
            if (_selectedCards.Count <= 4)
            {
                _selectedCards.Add(card);
            }
        }
        else
        {
            _selectedCards.Remove(card);
        }
    }

    public void ClearSelectedCardList()
    {
        foreach (Card card in _selectedCards)
        {
            card.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        _selectedCards.Clear();
    }

    public void AddToHand(Card card)
    {
        _hand.Add(card);
        card.transform.SetParent(HandHolder.Instance.transform);
        card.gameObject.GetComponent<Dragable>()._disableCanvas = HandHolder.Instance.GetComponent<DisableCanvas>();
        card.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
