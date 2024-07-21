using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                newCard.InitCard(suit, rank);
                newCard.name = rank + " of " + suit;
                //newCard.gameObject.SetActive(false);
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
        StartCoroutine(AddToHandCoroutine());
    }

    public void AddToHand(int numOfCardToAdd)
    {
        StartCoroutine(AddToHandCoroutine(numOfCardToAdd));
    }

    private IEnumerator AddToHandCoroutine()
    {
        for (int i = 0; i < _handHolder.CardToSpawn; i++)
        {
            Card card = _deck[0];
            _deck.RemoveAt(0);
            ListsManager.Instance.AddToHand(card);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator AddToHandCoroutine(int numOfCardToAdd)
    {
        if(_deck.Count < numOfCardToAdd)
        {
            numOfCardToAdd = _deck.Count;
        }
        for (int i = 0; i < numOfCardToAdd; i++)
        {
            yield return new WaitForSeconds(0.2f);
            Card card = _deck[0];
            _deck.RemoveAt(0);
            ListsManager.Instance.AddToHand(card);
        }
    }
}
