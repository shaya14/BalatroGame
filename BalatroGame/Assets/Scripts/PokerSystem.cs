using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public void DefinePokerHand(List<Card> handToCheck)
    {
        if (IsRoyalFlush(handToCheck)) SetHandType(HandType.RoyalFlush, "Royal Flush");
        else if (IsStraightFlush(handToCheck)) SetHandType(HandType.StraightFlush, "Straight Flush");
        else if (IsFourOfAKind(handToCheck)) SetHandType(HandType.FourOfAKind, "Four of a Kind");
        else if (IsFullHouse(handToCheck)) SetHandType(HandType.FullHouse, "Full House");
        else if (IsFlush(handToCheck)) SetHandType(HandType.Flush, "Flush");
        else if (IsStraight(handToCheck)) SetHandType(HandType.Straight, "Straight");
        else if (IsThreeOfAKind(handToCheck)) SetHandType(HandType.ThreeOfAKind, "Three of a Kind");
        else if (IsTwoPair(handToCheck)) SetHandType(HandType.TwoPair, "Two Pair");
        else if (IsPair(handToCheck)) SetHandType(HandType.Pair, "Pair");
        else
        {
            SetHandType(HandType.HighCard, "High Card");
            FindHighestCard(handToCheck);
        }
    }

    private void SetHandType(HandType handType, string debugMessage)
    {
        _handType = handType;
        Debug.Log(debugMessage);
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
        return ranks.Count >= 5 && (CheckSequentialRanks(ranks) || CheckAceLowStraight(ranks, handToCheck));
    }

    private bool IsFlush(List<Card> handToCheck) => 
        handToCheck.Count >= 5 && handToCheck.All(card => card.Suit == handToCheck[0].Suit);

    private bool IsFullHouse(List<Card> handToCheck) => 
        CheckGroupCount(handToCheck, 3, 1) && CheckGroupCount(handToCheck, 2, 1);

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
        var highestCard = handToCheck.OrderByDescending(card => GetRankValue(card.Rank)).FirstOrDefault();
        if (highestCard != null)
        {
            ListsManager.Instance.UpdateScoredCards(highestCard);
        }
    }
}
