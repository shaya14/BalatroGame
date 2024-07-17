
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;

public class Card : MonoBehaviour
{
    public Image image;
    public Sprite _sprite;
    public string _suit;
    public string _rank;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        image.sprite = Resources.Load<Sprite>("Cards/" + _suit + "_" + _rank);
    }
}
