using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Planet : MonoBehaviour
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    private int _level = 1;
    private string _name;
    private int _cost = 3;
    private int _points;
    private int _mult;

    public int Level => _level;

    public HandType _handType;

    private void Start()
    {
        RandomHandType();
        SetStatsToHand(_handType);
        _costText.text = "$" + _cost.ToString();
        _descriptionText = GameObject.Find("Shop Description Text").GetComponent<TextMeshProUGUI>();
        _buyButton.onClick.AddListener(BuyPlanet);
    }

    private void RandomHandType()
    {
        _handType = (HandType)Random.Range(0, 9);
    }

    public void PlanetInfo()
    {
        _descriptionText.text = $"{_name}\n +1 Level";
    }

    public void EnableBuyButton(bool value)
    {
        _buyButton.gameObject.SetActive(value);
    }

    public void ClearPlanetInfo()
    {
        _descriptionText.text = "";
    }

    public void PlanetAction()
    {
        Debug.Log("Planet Action");
        _level++;
        InfoPanelHandler.Instance.UpdateText(this, _points, _mult);
        PointsHandler.Instance.UpgradeHand(_points, _mult);
    }
    public void SetStatsToHand(HandType handType)
    {
        switch (handType)
        {
            case HandType.HighCard:
                _name = "High Card";
                _points = 10;
                _mult = 1;
                break;
            case HandType.Pair:
                _name = "Pair";
                _points = 15;
                _mult = 1;
                break;
            case HandType.TwoPair:
                _name = "Two Pair";
                _points = 20;
                _mult = 2;
                break;
            case HandType.ThreeOfAKind:
                _name = "Three Of A Kind";
                _points = 25;
                _mult = 2;
                break;
            case HandType.Straight:
                _name = "Straight";
                _points = 30;
                _mult = 3;
                break;
            case HandType.Flush:
                _name = "Flush";
                _points = 35;
                _mult = 2;
                break;
            case HandType.FullHouse:
                _name = "Full House";
                _points = 40;
                _mult = 2;
                break;
            case HandType.FourOfAKind:
                _name = "Four Of A Kind";
                _points = 40;
                _mult = 3;
                break;
            case HandType.StraightFlush:
                _name = "Straight Flush";
                _points = 40;
                _mult = 4;
                break;
            case HandType.RoyalFlush:
                _name = "Royal Flush";
                _points = 50;
                _mult = 5;
                break;
        }
    }

    public void BuyPlanet()
    {
        if (GameHandler.Instance.PlayerMoney < _cost)
        {
            return;
        }

        GameHandler.Instance.UpdatePlayerMoney(-_cost);
        PlanetAction();
        Destroy(gameObject);
    }
}
