using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHandler : MonoBehaviour
{
    private static ShopHandler _instance;
    [SerializeField] private List<Joker> _jokers;
    [SerializeField] private int _numOfJokersToSpawn;
    [SerializeField] private Planet _planetPrefab;
    [SerializeField] private int _numOfPlanetsToSpawn;
    [SerializeField] private Transform _shopPlacement;
    [SerializeField] private CanvasGroup _canvasGroup;

    private Planet _currentPlanet;

    private bool _isShopOpen = false;
    private bool _isJokerActivated = false;

    public static ShopHandler Instance => _instance;

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

    private void Start()
    {
        foreach (Joker joker in _jokers)
        {
            joker.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_isShopOpen && !_isJokerActivated)
        {
            SetAllJokersActive(false);
            ActiveRandomJokers();
            DestroyLastPlanet();
            InstantiatePlanet();
            StartCoroutine(OpenShopCoroutine());
            _isJokerActivated = true;
        }
    }

    private void SetAllJokersActive(bool value)
    {
        foreach (Joker joker in _jokers)
        {
            joker.gameObject.SetActive(value);
        }
    }

    private void ActiveRandomJokers()
    {
        List<Joker> availableJokers = new List<Joker>(_jokers);

        for (int i = 0; i < _numOfJokersToSpawn; i++)
        {
            if (availableJokers.Count == 0)
                break;

            int randomIndex = Random.Range(0, availableJokers.Count);
            availableJokers[randomIndex].gameObject.SetActive(true);
            availableJokers.RemoveAt(randomIndex);
        }
    }

    private void InstantiatePlanet()
    {
        for (int i = 0; i < _numOfPlanetsToSpawn; i++)
        {
            _currentPlanet = Instantiate(_planetPrefab, _shopPlacement);
        }
    }

    private void DestroyLastPlanet()
    {
        if (_currentPlanet != null)
        {
            Destroy(_currentPlanet.gameObject);
        }
    }

    public void SetIsShopOpen(bool value)
    {
        _isShopOpen = value;
        _isJokerActivated = false;
    }

    private IEnumerator OpenShopCoroutine()
    {
        yield return null; // Wait one frame to ensure changes happen off-screen

        _canvasGroup.alpha = 1; // Make the shop visible
        _canvasGroup.blocksRaycasts = true; // Enable raycasts for the shop
    }

    public void RemoveBoughtJoker(Joker joker)
    {
        _jokers.Remove(joker);
    }
}
