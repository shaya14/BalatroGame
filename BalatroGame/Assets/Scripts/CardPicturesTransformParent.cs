using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An empty singleton that sits under 'Canvas', and holds the card-pictures.
public class CardPicturesTransformParent : MonoBehaviour
{
    private static CardPicturesTransformParent _instance;
    public static CardPicturesTransformParent instance => _instance;

    void Awake()
    {
        _instance = this;
    }
}
