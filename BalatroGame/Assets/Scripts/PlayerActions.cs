using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
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
        PokerSystem.Instance.DefinePokerHand(ListsManager.Instance.SelectedCards);
    }
}
