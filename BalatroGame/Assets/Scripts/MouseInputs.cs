using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputs : MonoBehaviour
{
    void Update()
    {
        if(!ListsManager.Instance.IsPlayingHand)
        {
            if (Input.GetMouseButtonDown(1))
            {
                ListsManager.Instance.ClearSelectedCardList();
            }
        }
    }
}
