using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum HandType
{
    HighCard,
    Pair,
    TwoPair,
    ThreeOfAKind,
    Straight,
    Flush,
    FullHouse,
    FourOfAKind,
    StraightFlush,
    RoyalFlush
}

public class PokerSystem : MonoBehaviour
{
    // Singleton instance
    private static PokerSystem _instance;
    public static PokerSystem Instance => _instance;
    [SerializeField] private TextMeshProUGUI _pokerHandText;

    private int _basePoints;
    private float _baseMult;

    // Hand type
    public HandType _handType { get; private set; }

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
    }
    private void Update()
    {
        // if (ListsManager.Instance.SelectedCards.Count >= 1)
        // {
        //     DefinePokerHand(ListsManager.Instance.SelectedCards);
        // }
    }

    // public void SetPointsAndMultTextToPanel(TextMeshProUGUI text)
    // {
    //     text.text = $"{_basePoints} x {_baseMult}";
    // }

    public void DefinePokerHand(List<Card> handToCheck)
    {
        if (IsRoyalFlush(handToCheck))
        {
            SetHandType(HandType.RoyalFlush, "Royal Flush");
        }
        else if (IsStraightFlush(handToCheck))
        {
            SetHandType(HandType.StraightFlush, "Straight Flush");
        }
        else if (IsFourOfAKind(handToCheck))
        {
            SetHandType(HandType.FourOfAKind, "Four of a Kind");
        }
        else if (IsFullHouse(handToCheck))
        {
            SetHandType(HandType.FullHouse, "Full House");
        }
        else if (IsFlush(handToCheck))
        {
            SetHandType(HandType.Flush, "Flush");
        }
        else if (IsStraight(handToCheck))
        {
            SetHandType(HandType.Straight, "Straight");
        }
        else if (IsThreeOfAKind(handToCheck))
        {
            SetHandType(HandType.ThreeOfAKind, "Three of a Kind");
        }
        else if (IsTwoPair(handToCheck))
        {
            SetHandType(HandType.TwoPair, "Two Pair");
        }
        else if (IsPair(handToCheck))
        {
            SetHandType(HandType.Pair, "Pair");
        }
        else
        {
            SetHandType(HandType.HighCard, "High Card");
            FindHighestCard(handToCheck);
        }
    }

    private void SetHandType(HandType handType, string debugMessage)
    {
        _handType = handType;
        PointsHandler.Instance.SetBasePointsAndMults(handType);
        PointsHandler.Instance.SetBasePointsAndMults(_basePoints, _baseMult);
        _pokerHandText.text = $"{debugMessage}";
        Debug.Log($"Hand Type: {debugMessage} Points: {_basePoints} Multiplier: {_baseMult}");
    }

    public void SetPointAndMults(int points, float mult)
    {
        _basePoints = points;
        _baseMult = mult;
    }

    public void SetTextToEmpty()
    {
        _pokerHandText.text = "";
    }

    private int GetRankValue(string rank, bool aceAsOne = false)
    {
        return rank switch
        {
            "2" => 2,
            "3" => 3,
            "4" => 4,
            "5" => 5,
            "6" => 6,
            "7" => 7,
            "8" => 8,
            "9" => 9,
            "10" => 10,
            "J" => 11,
            "Q" => 12,
            "K" => 13,
            "A" => aceAsOne ? 1 : 14,
            _ => throw new ArgumentException("Invalid card rank"),
        };
    }

    private bool IsRoyalFlush(List<Card> handToCheck) =>
        IsFlush(handToCheck) && handToCheck.All(card => new HashSet<string> { "10", "J", "Q", "K", "A" }.Contains(card.Rank));

    private bool IsPair(List<Card> handToCheck) =>
        CheckGroupCount(handToCheck, 2, 1);

    private bool IsTwoPair(List<Card> handToCheck) =>
        CheckGroupCount(handToCheck, 2, 2);

    private bool IsThreeOfAKind(List<Card> handToCheck) =>
        CheckGroupCount(handToCheck, 3, 1);

    private bool IsStraight(List<Card> handToCheck)
    {
        var ranks = handToCheck.Select(card => GetRankValue(card.Rank)).Distinct().OrderBy(rank => rank).ToList();
        bool isStraight = ranks.Count >= 5 && (CheckSequentialRanks(ranks) || CheckAceLowStraight(ranks, handToCheck));
        CheckGroupCount(handToCheck, 1, 5);
        return isStraight;
    }

    private bool IsFlush(List<Card> handToCheck)
    {
        bool isFlush = handToCheck.Count >= 5 && handToCheck.All(card => card.Suit == handToCheck[0].Suit);
        CheckGroupCount(handToCheck, 1, 5);
        return isFlush;
    }

    private bool IsFullHouse(List<Card> handToCheck)
    {
        var groups = handToCheck.GroupBy(card => card.Rank).ToList();
        if (groups.Count(g => g.Count() == 3) == 1 && groups.Count(g => g.Count() == 2) >= 1)
        {
            ListsManager.Instance.ClearScoredCards();
            foreach (var card in groups.Where(g => g.Count() == 3).SelectMany(g => g))
            {
                ListsManager.Instance.UpdateScoredCards(card);
            }
            foreach (var card in groups.Where(g => g.Count() == 2).SelectMany(g => g).Take(2))
            {
                ListsManager.Instance.UpdateScoredCards(card);
            }
            return true;
        }
        return false;
    }


    private bool IsFourOfAKind(List<Card> handToCheck) =>
        CheckGroupCount(handToCheck, 4, 1);

    private bool IsStraightFlush(List<Card> handToCheck) =>
        IsFlush(handToCheck) && IsStraight(handToCheck);

    private bool CheckGroupCount(List<Card> handToCheck, int count, int expectedGroups)
    {
        var groups = handToCheck.GroupBy(card => card.Rank).Where(group => group.Count() == count).ToList();
        if (groups.Count == expectedGroups)
        {
            ListsManager.Instance.ClearScoredCards();
            foreach (var group in groups)
            {
                foreach (var card in group)
                {
                    ListsManager.Instance.UpdateScoredCards(card);
                }
            }
            return true;
        }
        return false;
    }

    private bool CheckSequentialRanks(List<int> ranks)
    {
        for (int i = 0; i <= ranks.Count - 5; i++)
        {
            if (ranks.Skip(i).Take(5).SequenceEqual(Enumerable.Range(ranks[i], 5)))
            {
                ListsManager.Instance.ClearScoredCards();
                return true;
            }
        }
        return false;
    }

    private bool CheckAceLowStraight(List<int> ranks, List<Card> handToCheck)
    {
        if (ranks.Contains(14) && ranks.Contains(2) && ranks.Contains(3) && ranks.Contains(4) && ranks.Contains(5))
        {
            ListsManager.Instance.ClearScoredCards();
            foreach (var card in handToCheck.Where(card => GetRankValue(card.Rank) == 14 || GetRankValue(card.Rank) <= 5))
            {
                ListsManager.Instance.UpdateScoredCards(card);
            }
            return true;
        }
        return false;
    }

    public void FindHighestCard(List<Card> handToCheck)
    {
        // Clear the scored cards list
        ListsManager.Instance.ClearScoredCards();

        // Find the highest card
        var highestCard = handToCheck.OrderByDescending(card => GetRankValue(card.Rank)).FirstOrDefault();

        // Update the scored cards with the highest card
        if (highestCard != null)
        {
            ListsManager.Instance.UpdateScoredCards(highestCard);
        }
    }

}
