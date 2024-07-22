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
            FindHighestCard(handToCheck);
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
        bool isRoyalFlush = handToCheck.All(card => royalRanks.Contains(card.Rank));

        if (isRoyalFlush)
        {
            // Clear previous scored cards and add only the Royal Flush cards
            ListsManager.Instance.ClearScoredCards();
            foreach (var card in handToCheck)
            {
                ListsManager.Instance.UpdateScoredCards(card);
            }
        }

        return isRoyalFlush;
    }

    public bool IsPair(List<Card> handToCheck)
    {
        var pair = handToCheck.GroupBy(card => card.Rank).FirstOrDefault(group => group.Count() == 2);
        if (pair != null)
        {
            foreach (var card in pair)
            {
                ListsManager.Instance.UpdateScoredCards(card);
            }
            return true;
        }
        return false;
    }

    public bool IsTwoPair(List<Card> handToCheck)
    {
        var pairs = handToCheck.GroupBy(card => card.Rank).Where(group => group.Count() == 2).ToList();
        if (pairs.Count == 2)
        {
            foreach (var pair in pairs)
            {
                foreach (var card in pair)
                {
                    ListsManager.Instance.UpdateScoredCards(card);
                }
            }
            return true;
        }
        return false;
    }

    public bool IsThreeOfAKind(List<Card> handToCheck)
    {
        var threeOfAKind = handToCheck.GroupBy(card => card.Rank).FirstOrDefault(group => group.Count() == 3);
        if (threeOfAKind != null)
        {
            foreach (var card in threeOfAKind)
            {
                ListsManager.Instance.UpdateScoredCards(card);
            }
            return true;
        }
        return false;
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
                // Clear previous scored cards and add the new ones
                ListsManager.Instance.ClearScoredCards();
                for (int j = i; j < i + 5; j++)
                {
                    ListsManager.Instance.UpdateScoredCards(handToCheck[j]);
                }
                return true;
            }
        }

        // Check for Ace-low straight
        if (ranks.Contains(14) && ranks.Contains(2) && ranks.Contains(3) && ranks.Contains(4) && ranks.Contains(5))
        {
            // Clear previous scored cards and add the new ones
            ListsManager.Instance.ClearScoredCards();
            foreach (var card in handToCheck)
            {
                if (GetRankValue(card.Rank) == 14 || GetRankValue(card.Rank) <= 5)
                {
                    ListsManager.Instance.UpdateScoredCards(card);
                }
            }
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
        bool isFlush = handToCheck.All(card => card.Suit == suit);

        if (isFlush)
        {
            // Clear previous scored cards and add the new ones
            ListsManager.Instance.ClearScoredCards();
            foreach (var card in handToCheck)
            {
                ListsManager.Instance.UpdateScoredCards(card);
            }
        }

        return isFlush;
    }

    public bool IsFullHouse(List<Card> handToCheck)
    {
        var groupedRanks = handToCheck.GroupBy(card => card.Rank).ToList();
        bool hasThreeOfAKind = groupedRanks.Any(group => group.Count() == 3);
        bool hasPair = groupedRanks.Any(group => group.Count() == 2);

        if (hasThreeOfAKind && hasPair)
        {
            // Clear previous scored cards and add the new ones
            ListsManager.Instance.ClearScoredCards();
            foreach (var group in groupedRanks)
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

    public bool IsFourOfAKind(List<Card> handToCheck)
    {
        var fourOfAKind = handToCheck.GroupBy(card => card.Rank).FirstOrDefault(group => group.Count() == 4);
        if (fourOfAKind != null)
        {
            // Clear previous scored cards and add the new ones
            ListsManager.Instance.ClearScoredCards();
            foreach (var card in fourOfAKind)
            {
                ListsManager.Instance.UpdateScoredCards(card);
            }
            return true;
        }
        return false;
    }

    public bool IsStraightFlush(List<Card> handToCheck)
    {
        if (IsFlush(handToCheck) && IsStraight(handToCheck))
        {
            // Clear previous scored cards and add the new ones
            ListsManager.Instance.ClearScoredCards();
            foreach (var card in handToCheck)
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