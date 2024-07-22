
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _suit;
    [SerializeField] private string _rank;
    [SerializeField] private TextMeshProUGUI _pointsText;
    private int _pointsValue;
    [SerializeField] private float fadeDuration = 2.0f;
    private Color originalColor;

    public string Suit => _suit;
    public string Rank => _rank;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalColor = _pointsText.color;
    }

    private void Start()
    {
        image.sprite = Resources.Load<Sprite>("Cards/" + _suit + "_" + _rank);
    }
    public void InitCard(string suit, string rank)
    {
        _suit = suit;
        _rank = rank;
        SetPointsValue(rank);
    }

    public void SetPointsValue(string rank)
    {
        if (rank == "J" || rank == "Q" || rank == "K")
        {
            _pointsValue = 10;
            _pointsText.text = "+" + _pointsValue.ToString();
        }
        else if (rank == "A")
        {
            _pointsValue = 11;
            _pointsText.text = "+" + _pointsValue.ToString();
        }
        else
        {
            _pointsValue = int.Parse(rank);
            _pointsText.text = "+" + _pointsValue.ToString();
        }
    }

    public void SetTextEnabled(bool value)
    {
        _pointsText.gameObject.SetActive(value);
        StartCoroutine(FadeOutText());
    }

    private IEnumerator FadeOutText()
    {
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0f;
        Color startColor = _pointsText.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            _pointsText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        _pointsText.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        _pointsText.gameObject.SetActive(false); // Optionally disable the text object after fade-out
    }

    public void SetSuit(string suit)
    {
        _suit = suit;
    }

    public void SetRank(string rank)
    {
        _rank = rank;
    }
}
