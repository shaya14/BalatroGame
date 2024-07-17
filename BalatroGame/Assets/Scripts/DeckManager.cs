using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private static DeckManager _instance;
    [SerializeField] private Card _cardPrefab;
    private List<Card> _deck = new List<Card>();

    public static DeckManager Instance => _instance;

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
    private void Start()
    {
        CreateDeck();
        ShuffleDeck();
        AddToHand();
    }

    public void CreateDeck()
    {
        string[] suits = new string[] { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                Card newCard = Instantiate(_cardPrefab, transform);
                newCard._suit = suit;
                newCard._rank = rank;
                newCard.name = rank + " of " + suit;
                _deck.Add(newCard);
            }
        }
    }

    public void ShuffleDeck()
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

    public void AddToHand()
    {
        for (int i = 0; i < HandHolder.Instance.CardToSpawn; i++)
        {
            Card card = _deck[0];
            _deck.RemoveAt(0);
            ListsManager.Instance.AddToHand(card);
        }
    }

    public void AddToHand(int numOfCardToAdd)
    {
        if(_deck.Count < numOfCardToAdd)
        {
            numOfCardToAdd = _deck.Count;
        }
        for (int i = 0; i < numOfCardToAdd; i++)
        {
            Card card = _deck[0];
            _deck.RemoveAt(0);
            ListsManager.Instance.AddToHand(card);
        }
    }
}
