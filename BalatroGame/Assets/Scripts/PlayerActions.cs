using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private Joker _currentJoker;
    private Planet _currentPlanet;
    private static PlayerActions _instance;
    public static PlayerActions Instance => _instance;



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

    public void PlayHand()
    {
        if (GameHandler.Instance.RemainingHands <= 0)
            return;

        ListsManager.Instance.PlayedHand();
        GameHandler.Instance.UpdateReminingHands();
    }

    public void DiscardHand()
    {
        if (GameHandler.Instance.RemainingDiscards <= 0)
            return;

        ListsManager.Instance.DiscardHand();
        GameHandler.Instance.UpdateReminingDiscards();
    }

    public void NextRound()
    {
        GameManager.Instance.SetShopPanel(false);
    }

    public void GetJoker(Joker joker)
    {
        _currentJoker = joker;
    }

    public void GetPlanet(Planet planet)
    {
        _currentPlanet = planet;
    }

    public void BuyItem()
    {
        if (_currentPlanet)
            _currentPlanet.BuyPlanet();
            
        else if (_currentJoker)
            _currentJoker.BuyJoker();
    }
}
