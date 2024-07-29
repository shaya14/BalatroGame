using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListsManager : MonoBehaviour
{
    // Singleton instance
    private static ListsManager _instance;

    // Card lists
    private List<Card> _selectedCards = new List<Card>();
    [SerializeField] private List<Joker> _ownedJokers = new List<Joker>();
    private List<Card> _hand = new List<Card>();
    private List<Card> _scoredCards = new List<Card>();

    // Serialized fields
    [SerializeField] private HandHolder _handHolder;
    [SerializeField] private HandHolder _playedCards;
    [SerializeField] private DisableCanvas _disableCanvas;
    [SerializeField] private float _timeToDiscard;

    // Flags
    private bool _isPlayingHand = false;
    private bool _isCoroutineRunning = false;

    // Card positions
    private float _originalYPositon = 145.74f;
    private float _playedCardOriginalYPosition = 113.74f;

    // Properties
    public bool IsPlayingHand => _isPlayingHand;
    public bool IsCoroutineRunning => _isCoroutineRunning;
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

    private void Update()
    {
        UpdateSelectedCardsPosition();
        if (_scoredCards.Count > 0 && !_isCoroutineRunning)
        {
            StartCoroutine(ScoredCards(_scoredCards, 1));
        }
    }

    public void UpdateSelectedCardsPosition()
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
                        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, _playedCardOriginalYPosition, rectTransform.localPosition.z);
                    }
                    else
                    {
                        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, _originalYPositon, rectTransform.localPosition.z);
                    }
                }
            }
        }
    }

    public void UpdateOwnedJokers(Joker joker)
    {
        _ownedJokers.Add(joker);
    }

    public void UpdateScoredCards(Card card)
    {
        _scoredCards.Add(card);
    }

    public void ClearScoredCards()
    {
        _scoredCards.Clear();
    }

    public void CleanHand()
    {
        foreach (Card card in _hand)
        {
            Destroy(card.gameObject);
        }
        _hand.Clear();
    }

    public void UpdateSelectedCard(Card card)
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
            if (_isPlayingHand)
            {
                card.GetComponent<RectTransform>().localPosition =
    new Vector3(card.GetComponent<RectTransform>().localPosition.x,
    _playedCardOriginalYPosition, card.GetComponent<RectTransform>().localPosition.z);
            }
            else
            {
                card.GetComponent<RectTransform>().localPosition =
    new Vector3(card.GetComponent<RectTransform>().localPosition.x,
    _originalYPositon, card.GetComponent<RectTransform>().localPosition.z);
            }

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

        _selectedCards.Clear();
        Deck.Instance.AddToHand(cardsToDiscard.Count);
    }

    public void DiscardHand(List<Card> cardsToDiscard)
    {
        foreach (Card card in cardsToDiscard)
        {
            try
            {
                if (card == null)
                {
                    Debug.LogWarning("Card is null. Skipping...");
                    continue;
                }

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

        _selectedCards.Clear();
        Deck.Instance.AddToHand(cardsToDiscard.Count);
    }

    public void PlayedHand()
    {
        if (_isPlayingHand) return;

        PokerSystem.Instance.DefinePokerHand(_selectedCards);
        StartCoroutine(PlayedHandCoroutine());
        _isPlayingHand = true;
    }

    private IEnumerator PlayedHandCoroutine()
    {
        //Debug.Log("Starting PlayedHand coroutine");
        foreach (Card card in _selectedCards)
        {
            card.transform.SetParent(_playedCards.transform);
            _disableCanvas.DisableCanvasGroup();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator DelayedDiscardHand(float seconds)
    {
        //Debug.Log("DelayedDiscardHand coroutine started");
        List<Card> cardsToDiscard = new List<Card>(_selectedCards);
        yield return new WaitForSeconds(seconds);
        DiscardHand(cardsToDiscard);
        yield return new WaitForSeconds(1);
        _disableCanvas.EnableCanvasGroup();
    }

    private IEnumerator ScoredCards(List<Card> scoreCards, int seconds)
    {
        _isCoroutineRunning = true;
        //Debug.Log("DelayedScoredCards coroutine started");

        yield return new WaitForSeconds(seconds);

        foreach (Card card in scoreCards)
        {
            RectTransform rectTransform = card.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, _playedCardOriginalYPosition + 40, rectTransform.localPosition.z);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1);

        foreach (Card card in scoreCards)
        {
            card.SetTextEnabled(true);
            PointsHandler.Instance.AddPoints(card.PointsValue);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1);

        foreach (Joker joker in _ownedJokers)
        {
            joker.JokerAction();
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(2);
        PointsHandler.Instance.CalculateTotalPoints();
        StartCoroutine(DelayedDiscardHand(_timeToDiscard));
        ClearSelectedCardList();
        ClearScoredCards();

        yield return new WaitForSeconds(1);

        _isCoroutineRunning = false;
        _isPlayingHand = false;
        //Debug.Log("DelayedScoredCards coroutine ended");
    }
}
