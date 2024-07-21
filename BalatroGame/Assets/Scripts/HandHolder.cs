using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHolder : MonoBehaviour
{
    [SerializeField] private int _cardToSpawn;
    [SerializeField] private GameObject _discardedCards;
    public int CardToSpawn => _cardToSpawn;
    public GameObject DiscardedCards => _discardedCards;
}
