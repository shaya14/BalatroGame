using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private static Deck _instance;
    [SerializeField] private Card _cardPrefab;
    [SerializeField] private HandHolder _handHolder;
    private List<Card> _deck = new List<Card>();

    public static Deck Instance => _instance;

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

    private void CreateDeck()
    {
        string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                Card newCard = Instantiate(_cardPrefab, transform);
                newCard.InitCard(suit, rank);
                newCard.name = $"{rank} of {suit}";
                _deck.Add(newCard);
            }
        }
    }

    private void ShuffleDeck()
    {
        int n = _deck.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Card temp = _deck[k];
            _deck[k] = _deck[n];
            _deck[n] = temp;
        }
    }

    public void NewRoundDeck()
    {
        ResetNewDeck();
        AddToHand();
    }

    public void CleanLastDeck()
    {
        foreach (Card card in _deck)
        {
            Destroy(card.gameObject);
        }
        _deck.Clear();
    }

    public void ResetNewDeck()
    {
        CreateDeck();
        ShuffleDeck();
    }

    public void AddToHand()
    {
        StartCoroutine(AddToHandCoroutine(_handHolder.CardToSpawn));
    }

    public void AddToHand(int numOfCardsToAdd)
    {
        StartCoroutine(AddToHandCoroutine(numOfCardsToAdd));
    }

    private IEnumerator AddToHandCoroutine(int numOfCardsToAdd)
    {
        numOfCardsToAdd = Mathf.Min(numOfCardsToAdd, _deck.Count);

        for (int i = 0; i < numOfCardsToAdd; i++)
        {
            Card card = _deck[_deck.Count - 1];
            _deck.RemoveAt(_deck.Count - 1);
            ListsManager.Instance.AddToHand(card);
            yield return new WaitForSeconds(card.timeForPictureToReachPlaceholder);
        }
    }
}
