using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHolder : MonoBehaviour
{
    private static HandHolder _instance;
    [SerializeField] private int _cardToSpawn;
    [SerializeField] private GameObject _discardedCards;

    public static HandHolder Instance => _instance;
    public int CardToSpawn => _cardToSpawn;
    public GameObject DiscardedCards => _discardedCards;

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
}
