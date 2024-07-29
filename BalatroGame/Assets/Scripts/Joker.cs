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
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _jokerActionText;
    [SerializeField] private Transform _boughtJoskerPosition;
    [SerializeField] private string _name;
    [SerializeField] private string _description;       
    private bool _isBought = false;

    [Foldout("Joker Settings")]
    [SerializeField] private int _points;
    [SerializeField] private int _mult;
    [SerializeField] private int _cost;
    private float _fadeDuration = 2.0f;

    public int Cost => _cost;
    public bool IsBought => _isBought;

    private void Start()
    {
        _costText.text = "$" + _cost.ToString();
    }

    public void ClearJokerInfo()
    {
        _descriptionText.text = "";
    }

    public void JokerInfo()
    {
        _descriptionText.text = $"{_name}\n{_description}";

        if(_description.Contains("Mult"))
        {
            _descriptionText.color = Color.red;
            
            _jokerActionText.text = "+ " + _mult + " Mult";
            _jokerActionText.color = Color.red;
        }
        else if (_description.Contains("Points"))
        {
            _descriptionText.color = Color.blue;

            _jokerActionText.text = "+ " + _points + " Points";
            _jokerActionText.color = Color.blue;
        }
    }   

    public void JokerAction()
    {
        Debug.Log("Joker Action");

        // Show Action Text
        SetActionTextEnabled(true);

        if (_points > 0)
            PointsHandler.Instance.AddPoints(_points);
        else if (_mult > 0)
            PointsHandler.Instance.AddMult(_mult);
    }

    private void SetActionTextEnabled(bool value)
    {
        _jokerActionText.gameObject.SetActive(value);
        if (value)
        {
            StartCoroutine(ShakeCard());
            StartCoroutine(FadeOutText());
        }
    }

    private IEnumerator FadeOutText()
    {
        yield return new WaitForSeconds(0.1f);
        Color originalColor = _jokerActionText.color;
        float elapsedTime = 0f;
        Color startColor = _jokerActionText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        while (elapsedTime < _fadeDuration)
        {
            _jokerActionText.color = Color.Lerp(startColor, endColor, elapsedTime / _fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _jokerActionText.gameObject.SetActive(false);
        _jokerActionText.color = originalColor;
    }

    private IEnumerator ShakeCard()
    {
        float duration = 0.2f;
        float elapsedTime = 0f;
        Vector3 originalPos = transform.position;
        while (elapsedTime < duration)
        {
            transform.position = originalPos + Random.insideUnitSphere * 10f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPos;
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
        _costText.gameObject.SetActive(false);
        _descriptionText.text = "";
        _isBought = true;
        Debug.Log("Joker Bought");
    }

    public void EnableBuyButton(bool enable)
    {
        _buyButton.gameObject.SetActive(enable);
    }
}
