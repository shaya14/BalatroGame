
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _suit;
    [SerializeField] private string _rank;
    [SerializeField] private TextMeshProUGUI _piointsText;
    private int _pointsValue;
    
    public string Suit => _suit;
    public string Rank => _rank;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        image.sprite = Resources.Load<Sprite>("Cards/" + _suit + "_" + _rank);
    }
    public void InitCard(string suit, string rank)
    {
        _suit = suit;
        _rank = rank;
        SetPointsValue(rank);
    }

    public void SetPointsValue(string rank)
    {
        if (rank == "J" || rank == "Q" || rank == "K")
        {
            _pointsValue = 10;
            _piointsText.text = "+" + _pointsValue.ToString();
        }
        else if (rank == "A")
        {
            _pointsValue = 11;
            _piointsText.text = "+" + _pointsValue.ToString();
        }
        else
        {
            _pointsValue = int.Parse(rank);
            _piointsText.text = "+" + _pointsValue.ToString();
        }
    }

    public void SetSuit(string suit)
    {
        _suit = suit;
    }

    public void SetRank(string rank)
    {
        _rank = rank;
    }
}
