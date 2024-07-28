using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class Joker : MonoBehaviour
{
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Transform _boughtJoskerPosition;
    private bool _isBought = false;

    [Foldout("Joker Settings")]
    [SerializeField] private int _points;
    [SerializeField] private int _mult;
    [SerializeField] private int _cost;

    public int Cost => _cost;
    public bool IsBought => _isBought;

    private void Start()
    {
        _costText.text = _cost.ToString();
    }

    public void JokerAction()
    {
        Debug.Log("Joker Action");

        if (_points > 0)
            PointsHandler.Instance.AddPoints(_points);
        else if (_mult > 0)
            PointsHandler.Instance.AddMult(_mult);
    }

    public void BuyJoker()
    {
        if (GameHandler.Instance.PlayerMoney < _cost)
            return;

        if (_isBought)
            return;

        GameHandler.Instance.UpdatePlayerMoney(-_cost);
        _buyButton.gameObject.SetActive(false);
        transform.SetParent(_boughtJoskerPosition);
        ListsManager.Instance.UpdateOwnedJokers(this);
        _isBought = true;
        Debug.Log("Joker Bought");
    }

    public void EnableBuyButton(bool enable)
    {
        _buyButton.gameObject.SetActive(enable);
    }
}
