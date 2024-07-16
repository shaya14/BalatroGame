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
                if (handToCheck[i]._rank == handToCheck[j]._rank)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsTwoPair(List<Card> handToCheck)
    {
        int pairs = 0;
        for (int i = 0; i < handToCheck.Count; i++)
        {
            for (int j = i + 1; j < handToCheck.Count; j++)
            {
                if (handToCheck[i]._rank == handToCheck[j]._rank)
                {
                    pairs++;
                }
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
                    if (handToCheck[i]._rank == handToCheck[j]._rank && handToCheck[j]._rank == handToCheck[k]._rank)
                    {
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
            switch (card._rank)
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
        for (int i = 0; i < ranks.Count - 1; i++)
        {
            if (ranks[i] + 1 != ranks[i + 1])
            {
                return false;
            }
        }
        return true;
    }

    public bool IsFlush(List<Card> handToCheck)
    {
        string suit = handToCheck[0]._suit;
        for (int i = 1; i < handToCheck.Count; i++)
        {
            if (handToCheck[i]._suit != suit)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsFullHouse(List<Card> handToCheck)
    {
        int pairs = 0;
        bool threeOfAKind = false;
        for (int i = 0; i < handToCheck.Count; i++)
        {
            for (int j = i + 1; j < handToCheck.Count; j++)
            {
                if (handToCheck[i]._rank == handToCheck[j]._rank)
                {
                    pairs++;
                }
            }
        }
        for (int i = 0; i < handToCheck.Count; i++)
        {
            for (int j = i + 1; j < handToCheck.Count; j++)
            {
                for (int k = j + 1; k < handToCheck.Count; k++)
                {
                    if (handToCheck[i]._rank == handToCheck[j]._rank && handToCheck[j]._rank == handToCheck[k]._rank)
                    {
                        threeOfAKind = true;
                    }
                }
            }
        }
        return pairs == 1 && threeOfAKind;
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
                        if (handToCheck[i]._rank == handToCheck[j]._rank && handToCheck[j]._rank == handToCheck[k]._rank && handToCheck[k]._rank == handToCheck[l]._rank)
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
        return IsStraight(handToCheck) && IsFlush(handToCheck);
    }

    public bool IsRoyalFlush(List<Card> handToCheck)
    {
        List<int> ranks = new List<int>();
        foreach (Card card in handToCheck)
        {
            switch (card._rank)
            {
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
        return IsStraightFlush(handToCheck) && ranks[0] == 10;
    }


}
