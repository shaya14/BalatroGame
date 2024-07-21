using System.Collections;
using System.Collections.Generic;
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



    public bool IsPair(List<Card> handToCheck)
    {
        for (int i = 0; i < handToCheck.Count; i++)
        {
            for (int j = i + 1; j < handToCheck.Count; j++)
            {
                if (handToCheck[i].Rank == handToCheck[j].Rank)
                {
                    ListsManager.Instance.UpdateScoredCards(handToCheck[i]);
                    ListsManager.Instance.UpdateScoredCards(handToCheck[j]);
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsTwoPair(List<Card> handToCheck)
    {
        Dictionary<string, int> rankCount = new Dictionary<string, int>();
        foreach (var card in handToCheck)
        {
            if (rankCount.ContainsKey(card.Rank))
            {
                rankCount[card.Rank]++;
                ListsManager.Instance.UpdateScoredCards(card);
                ListsManager.Instance.UpdateScoredCards(card);
            }
            else
            {
                rankCount[card.Rank] = 1;
            }
        }

        int pairs = 0;
        foreach (var count in rankCount.Values)
        {
            if (count == 2)
            {
                pairs++;
            }
        }

        return pairs == 2;
    }


    public bool IsThreeOfAKind(List<Card> handToCheck)
    {
        for (int i = 0; i < handToCheck.Count; i++)
        {
            for (int j = i + 1; j < handToCheck.Count; j++)
            {
                for (int k = j + 1; k < handToCheck.Count; k++)
                {
                    if (handToCheck[i].Rank == handToCheck[j].Rank && handToCheck[j].Rank == handToCheck[k].Rank)
                    {
                        ListsManager.Instance.UpdateScoredCards(handToCheck[i]);
                        ListsManager.Instance.UpdateScoredCards(handToCheck[j]);
                        ListsManager.Instance.UpdateScoredCards(handToCheck[k]);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool IsStraight(List<Card> handToCheck)
    {
        List<int> ranks = new List<int>();
        foreach (Card card in handToCheck)
        {
            switch (card.Rank)
            {
                case "2":
                    ranks.Add(2);
                    break;
                case "3":
                    ranks.Add(3);
                    break;
                case "4":
                    ranks.Add(4);
                    break;
                case "5":
                    ranks.Add(5);
                    break;
                case "6":
                    ranks.Add(6);
                    break;
                case "7":
                    ranks.Add(7);
                    break;
                case "8":
                    ranks.Add(8);
                    break;
                case "9":
                    ranks.Add(9);
                    break;
                case "10":
                    ranks.Add(10);
                    break;
                case "J":
                    ranks.Add(11);
                    break;
                case "Q":
                    ranks.Add(12);
                    break;
                case "K":
                    ranks.Add(13);
                    break;
                case "A":
                    ranks.Add(14);
                    break;
            }
        }
        ranks.Sort();
        // Check for a sequence of exactly 5 cards
        for (int i = 0; i <= ranks.Count - 5; i++)
        {
            if (ranks[i] + 1 == ranks[i + 1] &&
                ranks[i + 1] + 1 == ranks[i + 2] &&
                ranks[i + 2] + 1 == ranks[i + 3] &&
                ranks[i + 3] + 1 == ranks[i + 4])
            {
                ListsManager.Instance.UpdateScoredCards(handToCheck[i]);
                ListsManager.Instance.UpdateScoredCards(handToCheck[i + 1]);
                ListsManager.Instance.UpdateScoredCards(handToCheck[i + 2]);
                ListsManager.Instance.UpdateScoredCards(handToCheck[i + 3]);
                ListsManager.Instance.UpdateScoredCards(handToCheck[i + 4]);
                return true;
            }
        }
        return false;
    }


    public bool IsFlush(List<Card> handToCheck)
    {
        if (handToCheck.Count < 5)
        {
            return false; // Not enough cards to form a flush
        }
        else if (handToCheck.Count == 5)
        {
            // Check if all cards have the same suit
            string suit = handToCheck[0].Suit;
            foreach (var card in handToCheck)
            {
                if (card.Suit != suit)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }


    public bool IsFullHouse(List<Card> handToCheck)
    {
        Dictionary<string, int> rankCount = new Dictionary<string, int>();
        foreach (var card in handToCheck)
        {
            if (rankCount.ContainsKey(card.Rank))
            {
                rankCount[card.Rank]++;
            }
            else
            {
                rankCount[card.Rank] = 1;
            }
        }

        bool hasThreeOfAKind = false;
        bool hasPair = false;

        foreach (var count in rankCount.Values)
        {
            if (count == 3)
            {
                hasThreeOfAKind = true;
            }
            if (count == 2)
            {
                hasPair = true;
            }
        }

        return hasThreeOfAKind && hasPair;
    }


    public bool IsFourOfAKind(List<Card> handToCheck)
    {
        for (int i = 0; i < handToCheck.Count; i++)
        {
            for (int j = i + 1; j < handToCheck.Count; j++)
            {
                for (int k = j + 1; k < handToCheck.Count; k++)
                {
                    for (int l = k + 1; l < handToCheck.Count; l++)
                    {
                        if (handToCheck[i].Rank == handToCheck[j].Rank && handToCheck[j].Rank == handToCheck[k].Rank && handToCheck[k].Rank == handToCheck[l].Rank)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool IsStraightFlush(List<Card> handToCheck)
    {
        if (handToCheck.Count < 5)
        {
            return false;
        }

        // Convert ranks to integer values and sort the hand
        List<Card> sortedHand = new List<Card>(handToCheck);
        sortedHand.Sort((a, b) => GetRankValue(a.Rank).CompareTo(GetRankValue(b.Rank)));

        for (int i = 0; i <= sortedHand.Count - 5; i++)
        {
            List<Card> potentialStraightFlush = sortedHand.GetRange(i, 5);
            if (IsFlush(potentialStraightFlush) && IsStraight(potentialStraightFlush))
            {
                return true;
            }
        }

        return false;
    }

    private int GetRankValue(string rank)
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
            case "A": return 14;
            default: return 0; // Handle invalid rank if necessary
        }
    }



    public bool IsRoyalFlush(List<Card> handToCheck)
    {
        if (handToCheck.Count < 5)
        {
            return false;
        }

        // Convert ranks to integer values and sort the hand
        List<Card> sortedHand = new List<Card>(handToCheck);
        sortedHand.Sort((a, b) => GetRankValue(a.Rank).CompareTo(GetRankValue(b.Rank)));

        for (int i = 0; i <= sortedHand.Count - 5; i++)
        {
            List<Card> potentialRoyalFlush = sortedHand.GetRange(i, 5);
            if (IsFlush(potentialRoyalFlush) && IsStraight(potentialRoyalFlush) && GetRankValue(potentialRoyalFlush[0].Rank) == 10)
            {
                return true;
            }
        }

        return false;
    }

}
