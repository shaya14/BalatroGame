
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

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
    //    _sprite = Resources.Load<Sprite>("Cards/" + _suit + "_" + _rank);
        image.sprite = Resources.Load<Sprite>("Cards/" + _suit + "_" + _rank);
        //Debug.Log("Loaded: " + _suit + "_" + _rank);
    }
}
