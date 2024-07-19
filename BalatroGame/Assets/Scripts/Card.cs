
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;

public class Card : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _suit;
    [SerializeField] private string _rank;

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

    public void SetSuit(string suit)
    {
        _suit = suit;
    }

    public void SetRank(string rank)
    {
        _rank = rank;
    }
}
