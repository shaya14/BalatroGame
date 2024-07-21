using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
    private static PokerSystem _instance;

    public HandType _handType;

    public static PokerSystem Instance => _instance;

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
        if (IsRoyalFlush(handToCheck))
        {
            _handType = HandType.RoyalFlush;
            Debug.Log("Royal Flush");
        }
        else if (IsStraightFlush(handToCheck))
        {
            _handType = HandType.StraightFlush;
            Debug.Log("Straight Flush");
        }
        else if (IsFourOfAKind(handToCheck))
        {
            _handType = HandType.FourOfAKind;
            Debug.Log("Four of a Kind");
        }
        else if (IsFullHouse(handToCheck))
        {
            _handType = HandType.FullHouse;
            Debug.Log("Full House");
        }
        else if (IsFlush(handToCheck))
        {
            _handType = HandType.Flush;
            Debug.Log("Flush");
        }
        else if (IsStraight(handToCheck))
        {
            _handType = HandType.Straight;
            Debug.Log("Straight");
        }
        else if (IsThreeOfAKind(handToCheck))
        {
            _handType = HandType.ThreeOfAKind;
            Debug.Log("Three of a Kind");
        }
        else if (IsTwoPair(handToCheck))
        {
            _handType = HandType.TwoPair;
            Debug.Log("Two Pair");
        }
        else if (IsPair(handToCheck))
        {
            _handType = HandType.Pair;
            Debug.Log("Pair");
        }
        else
        {
            _handType = HandType.HighCard;
            Debug.Log("High Card");
        }
    }

    private int GetRankValue(string rank, bool aceAsOne = false)
    {
        switch (rank)
        {
            case "2": return 2;
            case "3": return 3;
            case "4": return 4;
            case "5": return 5;
            case "6": return 6;
            case "7": return 7;
            case "8": return 8;
            case "9": return 9;
            case "10": return 10;
            case "J": return 11;
            case "Q": return 12;
            case "K": return 13;
            case "A": return aceAsOne ? 1 : 14;
            default: throw new ArgumentException("Invalid card rank");
        }
    }

    public bool IsRoyalFlush(List<Card> handToCheck)
    {
        if (handToCheck.Count != 5)
        {
            return false;
        }

        if (!IsFlush(handToCheck))
        {
            return false;
        }

        HashSet<string> royalRanks = new HashSet<string> { "10", "J", "Q", "K", "A" };
        return handToCheck.All(card => royalRanks.Contains(card.Rank));
    }

    public bool IsPair(List<Card> handToCheck)
    {
        return handToCheck.GroupBy(card => card.Rank).Any(group => group.Count() == 2);
    }

    public bool IsTwoPair(List<Card> handToCheck)
    {
        return handToCheck.GroupBy(card => card.Rank).Count(group => group.Count() == 2) == 2;
    }

    public bool IsThreeOfAKind(List<Card> handToCheck)
    {
        return handToCheck.GroupBy(card => card.Rank).Any(group => group.Count() == 3);
    }

    public bool IsStraight(List<Card> handToCheck)
    {
        List<int> ranks = handToCheck.Select(card => GetRankValue(card.Rank)).Distinct().OrderBy(rank => rank).ToList();
        if (ranks.Count < 5)
        {
            return false;
        }

        // Check for standard straight
        for (int i = 0; i <= ranks.Count - 5; i++)
        {
            if (ranks[i] + 1 == ranks[i + 1] &&
                ranks[i + 1] + 1 == ranks[i + 2] &&
                ranks[i + 2] + 1 == ranks[i + 3] &&
                ranks[i + 3] + 1 == ranks[i + 4])
            {
                return true;
            }
        }

        // Check for Ace-low straight
        if (ranks.Contains(14) && ranks.Contains(2) && ranks.Contains(3) && ranks.Contains(4) && ranks.Contains(5))
        {
            return true;
        }

        return false;
    }

    public bool IsFlush(List<Card> handToCheck)
    {
        if (handToCheck.Count < 5)
        {
            return false;
        }

        string suit = handToCheck[0].Suit;
        return handToCheck.All(card => card.Suit == suit);
    }

    public bool IsFullHouse(List<Card> handToCheck)
    {
        var groupedRanks = handToCheck.GroupBy(card => card.Rank).ToList();
        bool hasThreeOfAKind = groupedRanks.Any(group => group.Count() == 3);
        bool hasPair = groupedRanks.Any(group => group.Count() == 2);
        return hasThreeOfAKind && hasPair;
    }

    public bool IsFourOfAKind(List<Card> handToCheck)
    {
        return handToCheck.GroupBy(card => card.Rank).Any(group => group.Count() == 4);
    }

    public bool IsStraightFlush(List<Card> handToCheck)
    {
        return IsFlush(handToCheck) && IsStraight(handToCheck);
    }
}


