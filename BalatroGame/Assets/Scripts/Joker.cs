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
    private bool _isBought = false;

    [Foldout("Joker Settings")]
    [SerializeField] private int _points;
    [SerializeField] private int _multPoints;
    [SerializeField] private int _mult;
    [SerializeField] private float _multMult;
    [SerializeField] private int _gold;
    [SerializeField] private int _cost;
    private float _fadeDuration = 2.0f;

    public int Cost => _cost;
    public bool IsBought => _isBought;

    private void Start()
    {
        _costText.text = "$" + _cost.ToString();
        _buyButton.onClick.AddListener(BuyJoker);
    }

    public void ClearJokerInfo()
    {
        _descriptionText.text = "";
    }

    public void JokerInfo()
    {
        if (_gold > 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_gold} Gold";
            _descriptionText.color = Color.yellow;

            _jokerActionText.text = "+" + _gold + " Gold";
            _jokerActionText.color = Color.yellow;
            return;
        }

        if (_points > 0 && _multPoints == 0 && _mult == 0 && _multMult == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_points} Points";
            _descriptionText.color = Color.blue;

            _jokerActionText.text = "+" + _points + " Points";
            _jokerActionText.color = Color.blue;
        }
        else if (_mult > 0 && _points == 0 && _multPoints == 0 && _multMult == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_mult} Mult";
            _descriptionText.color = Color.red;

            _jokerActionText.text = "+" + _mult + " Mult";
            _jokerActionText.color = Color.red;
        }
        else if (_multPoints > 0 && _points == 0 && _mult == 0 && _multMult == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n *{_multPoints} Points";
            _descriptionText.color = Color.blue;

            _jokerActionText.text = "*" + _multPoints + " Points";
            _jokerActionText.color = Color.blue;
        }
        else if (_multMult > 0 && _points == 0 && _mult == 0 && _multPoints == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n *{_multMult} Mult";
            _descriptionText.color = Color.red;

            _jokerActionText.text = "*" + _multMult + " Mult";
            _jokerActionText.color = Color.red;
        }
        else if (_points > 0 && _mult > 0 && _multPoints == 0 && _multMult == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_points} Points <color=red>+{_mult} Mult</color>";
            _descriptionText.color = Color.blue;

            _jokerActionText.text = "+" + _points + " Points\n<color=red>+" + _mult + " Mult</color>";
            _jokerActionText.color = Color.blue;
        }
        else if (_points > 0 && _multPoints > 0 && _mult == 0 && _multMult == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_points} Points <color=blue>*{_multPoints} Points</color>";
            _descriptionText.color = Color.blue;

            _jokerActionText.text = "+" + _points + " Points\n<color=blue>*" + _multPoints + " Points</color>";
            _jokerActionText.color = Color.blue;
        }
        else if (_points > 0 && _multMult > 0 && _mult == 0 && _multPoints == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_points} Points <color=red>*{_multMult} Mult</color>";
            _descriptionText.color = Color.blue;

            _jokerActionText.text = "+" + _points + " Points\n<color=red>*" + _multMult + " Mult</color>";
            _jokerActionText.color = Color.blue;
        }
        else if (_mult > 0 && _multPoints > 0 && _points == 0 && _multMult == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_mult} Mult <color=blue>*{_multPoints} Points</color>";
            _descriptionText.color = Color.red;

            _jokerActionText.text = "+" + _mult + " Mult\n<color=blue>*" + _multPoints + " Points</color>";
            _jokerActionText.color = Color.red;
        }
        else if (_mult > 0 && _multMult > 0 && _points == 0 && _multPoints == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_mult} Mult <color=red>*{_multMult} Mult</color>";
            _descriptionText.color = Color.red;

            _jokerActionText.text = "+" + _mult + " Mult\n<color=red>*" + _multMult + " Mult</color>";
            _jokerActionText.color = Color.red;
        }
        else if (_points > 0 && _multPoints > 0 && _multMult > 0 && _mult == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_points} Points \n<color=blue>*{_multPoints} Points</color> <color=red>*{_multMult} Mult</color>";
            _descriptionText.color = Color.blue;

            _jokerActionText.text = "+" + _points + " Points\n<color=blue>*" + _multPoints + " Points</color>\n<color=red>*" + _multMult + " Mult</color>";
            _jokerActionText.color = Color.blue;
        }
        else if (_mult > 0 && _multPoints > 0 && _multMult > 0 && _points == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_mult} Mult <color=blue>*{_multPoints} Points</color>\n <color=red>*{_multMult} Mult</color>";
            _descriptionText.color = Color.red;

            _jokerActionText.text = "+" + _mult + " Mult\n<color=blue>*" + _multPoints + " Points</color>\n<color=red>*" + _multMult + " Mult</color>";
            _jokerActionText.color = Color.red;
        }
        else if (_points > 0 && _mult > 0 && _multMult > 0 && _multPoints == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_points} Points <color=red>+{_mult} Mult</color>\n <color=blue>*{_multMult} Mult</color>";
            _descriptionText.color = Color.blue;

            _jokerActionText.text = "+" + _points + " Points\n<color=red>+" + _mult + " Mult</color>\n<color=blue>*" + _multMult + " Mult</color>";
            _jokerActionText.color = Color.blue;
        }
        else if (_points > 0 && _mult > 0 && _multPoints > 0 && _multMult == 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_points} Points <color=red>+{_mult} Mult</color>\n <color=blue>*{_multPoints} Points</color>";
            _descriptionText.color = Color.blue;

            _jokerActionText.text = "+" + _points + " Points\n<color=red>+" + _mult + " Mult</color>\n<color=blue>*" + _multPoints + " Points</color>";
            _jokerActionText.color = Color.blue;
        }
        else if (_points > 0 && _mult > 0 && _multPoints > 0 && _multMult > 0)
        {
            _descriptionText.text = $"<color=yellow>{_name}</color>\n +{_points} Points <color=red>+{_mult} Mult</color>\n <color=blue>*{_multPoints} Points</color> <color=red>*{_multMult} Mult</color>";
            _descriptionText.color = Color.blue;

            _jokerActionText.text = "+" + _points + " Points\n<color=red>+" + _mult + " Mult</color>\n<color=blue>*" + _multPoints + " Points</color>\n<color=red>*" + _multMult + " Mult</color>";
            _jokerActionText.color = Color.blue;
        }
    }

    public void JokerAction()
    {
        Debug.Log("Joker Action");

        // Show Action Text
        SetActionTextEnabled(true);
        ChooseJokerAction(_points, _mult, _multPoints, _multMult);
    }

    private void ChooseJokerAction(int points, int _mult, int _multPoints, float _multMult)
    {
        if (_gold > 0)
        {
            GameHandler.Instance.UpdatePlayerMoney(_gold);
        }
        else

                if (points > 0 && _multPoints == 0 && _mult == 0 && _multMult == 0)
            PointsHandler.Instance.AddPoints(points, true);
        else if (_mult > 0 && points == 0 && _multPoints == 0 && _multMult == 0)
            PointsHandler.Instance.AddMult(_mult);
        else if (_multPoints > 0 && points == 0 && _mult == 0 && _multMult == 0)
            PointsHandler.Instance.AddMultPoints(_multPoints, true);
        else if (_multMult > 0 && points == 0 && _mult == 0 && _multPoints == 0)
            PointsHandler.Instance.AddMultiplyMult(_multMult);
        else if (points > 0 && _mult > 0 && _multPoints == 0 && _multMult == 0)
        {
            PointsHandler.Instance.AddPoints(points, true);
            PointsHandler.Instance.AddMult(_mult);
        }
        else if (points > 0 && _multPoints > 0 && _mult == 0 && _multMult == 0)
        {
            PointsHandler.Instance.AddPoints(points, true);
            PointsHandler.Instance.AddMultPoints(_multPoints, true);
        }
        else if (points > 0 && _multMult > 0 && _mult == 0 && _multPoints == 0)
        {
            PointsHandler.Instance.AddPoints(points, true);
            PointsHandler.Instance.AddMultiplyMult(_multMult);
        }
        else if (_mult > 0 && _multPoints > 0 && points == 0 && _multMult == 0)
        {
            PointsHandler.Instance.AddMult(_mult);
            PointsHandler.Instance.AddMultPoints(_multPoints, true);
        }
        else if (_mult > 0 && _multMult > 0 && points == 0 && _multPoints == 0)
        {
            PointsHandler.Instance.AddMult(_mult);
            PointsHandler.Instance.AddMultiplyMult(_multMult);
        }
        else if (points > 0 && _multPoints > 0 && _multMult > 0 && _mult == 0)
        {
            PointsHandler.Instance.AddPoints(points, true);
            PointsHandler.Instance.AddMultPoints(_multPoints, true);
            PointsHandler.Instance.AddMultiplyMult(_multMult);
        }
        else if (_mult > 0 && _multPoints > 0 && _multMult > 0 && points == 0)
        {
            PointsHandler.Instance.AddMult(_mult);
            PointsHandler.Instance.AddMultPoints(_multPoints, true);
            PointsHandler.Instance.AddMultiplyMult(_multMult);
        }
        else if (points > 0 && _mult > 0 && _multMult > 0 && _multPoints == 0)
        {
            PointsHandler.Instance.AddPoints(points, true);
            PointsHandler.Instance.AddMult(_mult);
            PointsHandler.Instance.AddMultiplyMult(_multMult);
        }
        else if (points > 0 && _mult > 0 && _multPoints > 0 && _multMult == 0)
        {
            PointsHandler.Instance.AddPoints(points, true);
            PointsHandler.Instance.AddMult(_mult);
            PointsHandler.Instance.AddMultPoints(_multPoints, true);
        }
        else if (points > 0 && _mult > 0 && _multPoints > 0 && _multMult > 0)
        {
            PointsHandler.Instance.AddPoints(points, true);
            PointsHandler.Instance.AddMultPoints(_multPoints, true);
            PointsHandler.Instance.AddMult(_mult);
            PointsHandler.Instance.AddMultiplyMult(_multMult);
        }
    }

    public void ResetTextColors()
    {
        _descriptionText.color = Color.white;
        _jokerActionText.color = Color.white;
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
        ShopHandler.Instance.RemoveBoughtJoker(this);
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
