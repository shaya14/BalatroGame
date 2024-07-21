using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class ListsManager : MonoBehaviour
{
    private static ListsManager _instance;
    private List<Card> _selectedCards = new List<Card>();
    private List<Card> _hand = new List<Card>();
    [SerializeField] private HandHolder _handHolder;
    [SerializeField] private HandHolder _playedCards;

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
        card.gameObject.SetActive(true);
        card.transform.SetParent(_handHolder.transform);
        card.gameObject.GetComponent<Dragable>().SetDisableCanvas(_handHolder.GetComponent<DisableCanvas>());
        card.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void DiscardHand()
    {
        // Make a copy of the selected cards to avoid modifying the collection while iterating
        List<Card> cardsToDiscard = new List<Card>(_selectedCards);

        foreach (Card card in cardsToDiscard)
        {
            try
            {
                if (card == null)
                {
                    Debug.LogWarning("Card is null. Skipping...");
                    continue;
                }

                // Check if the card is still in the hand before attempting to remove it
                if (_hand.Contains(card))
                {
                    _hand.Remove(card);
                    card.transform.SetParent(_handHolder.DiscardedCards.transform);
                    card.gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("Card not found in hand. Skipping...");
                }
            }
            catch (ObjectDisposedException ex)
            {
                Debug.LogError($"ObjectDisposedException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message}");
            }
        }

        // Clear the selected cards after the removal
        _selectedCards.Clear();

        // If there's additional logic to add new cards to the hand, it should be done here
        Debug.Log(Deck.Instance);
        Deck.Instance.AddToHand(cardsToDiscard.Count);
    }

    public void PlayHand()
    {
        foreach (Card card in _selectedCards)
        {
            card.transform.SetParent(_playedCards.transform);
            StartCoroutine(DelayedDiscardHand(3));
        }
    }

    private IEnumerator DelayedDiscardHand(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        DiscardHand();
    }
}
