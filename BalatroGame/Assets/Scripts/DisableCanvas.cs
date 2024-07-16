using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCanvas : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void DisableCanvasGroup()
    {
        _canvasGroup.blocksRaycasts = false;
    }

    public void EnableCanvasGroup()
    {
        _canvasGroup.blocksRaycasts = true;
    }
}
