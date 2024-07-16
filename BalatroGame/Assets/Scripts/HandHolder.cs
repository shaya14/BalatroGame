using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHolder : MonoBehaviour
{
    private static HandHolder _instance;
    [SerializeField] private int _cardToSpawn;

    public static HandHolder Instance => _instance;
    public int CardToSpawn => _cardToSpawn;

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
