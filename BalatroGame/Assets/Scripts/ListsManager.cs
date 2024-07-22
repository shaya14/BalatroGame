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
    public List<Card> _scoredCards = new List<Card>();
    [SerializeField] private HandHolder _handHolder;
    [SerializeField] private HandHolder _playedCards;
    [SerializeField] private DisableCanvas _disableCanvas;
    private bool _isPlayingHand = false;
    private bool _isScoringHand = false;
    private bool _isCoroutineRunning = false; // Flag to track if the coroutine is running
    private float _originalYPositon = 145.74f;
    private float _playedCardOriginalYPosition = 125.74f;

    public bool IsPlayingHand => _isPlayingHand;
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
                RectTransform rectTransform = card.GetComponent<RectTransform>();
                if (!_scoredCards.Contains(card))
                {
                    if (_isPlayingHand)
                    {
                        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, _originalYPositon, rectTransform.localPosition.z);
                    }
                    else
                    {
                        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, _originalYPositon + 20, rectTransform.localPosition.z);
                    }
                }
            }
        }

        if (_scoredCards.Count > 0 && !_isCoroutineRunning)
        {
            _isScoringHand = true;
            StartCoroutine(DelayedScoredCards(_scoredCards, 1));
        }
    }

    public void UpdateScoredCards(Card card)
    {
        _scoredCards.Add(card);
    }

    public void ClearScoredCards()
    {
        _scoredCards.Clear();
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
            card.GetComponent<RectTransform>().localPosition =
            new Vector3(card.GetComponent<RectTransform>().localPosition.x,
            _originalYPositon, card.GetComponent<RectTransform>().localPosition.z);
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

    public void PlayedHand()
    {
        if (_isPlayingHand)
        {
            return;
        }
        PokerSystem.Instance.DefinePokerHand(_selectedCards);
        StartCoroutine(PlayedHandCoroutine());
        _isPlayingHand = true;
    }

    private IEnumerator PlayedHandCoroutine()
    {
        Debug.Log("Starting PlayedHand coroutine");
        foreach (Card card in _selectedCards)
        {
            card.transform.SetParent(_playedCards.transform);
            _disableCanvas.DisableCanvasGroup();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator DelayedDiscardHand(int seconds)
    {
        Debug.Log("DelayedDiscardHand coroutine started");
        yield return new WaitForSeconds(seconds);
        DiscardHand();
        yield return new WaitForSeconds(1);
        _scoredCards.Clear();
        _isPlayingHand = false;
        _disableCanvas.EnableCanvasGroup();
    }

    private IEnumerator DelayedScoredCards(List<Card> scoreCards, int seconds)
    {
        _isCoroutineRunning = true; // Set the flag to true
        Debug.Log("DelayedScoredCards coroutine started");

        yield return new WaitForSeconds(seconds);

        foreach (Card card in scoreCards)
        {
            RectTransform rectTransform = card.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, _playedCardOriginalYPosition + 40, rectTransform.localPosition.z);
            //Debug.Log("Card position updated: " + card.name);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1);

        foreach (Card card in scoreCards)
        {
            card.SetTextEnabled(true);
            //Debug.Log("Enabling text for card: " + card.name);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(2);

        ClearScoredCards();
        StartCoroutine(DelayedDiscardHand(3));

        yield return new WaitForSeconds(1);

        _isScoringHand = false;
        _isCoroutineRunning = false;
        _isPlayingHand = false;
        Debug.Log("DelayedScoredCards coroutine ended");
    }


}
