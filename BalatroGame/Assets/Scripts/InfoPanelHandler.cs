
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using VInspector;

public class InfoPanelHandler : MonoBehaviour
{
    // Singleton instance
    private static InfoPanelHandler _instance;
    public static InfoPanelHandler Instance => _instance;

    // Serialized fields
    [SerializeField] private GameObject _infoPanel;


    [Foldout("Level Texts")]
    [SerializeField] private TextMeshProUGUI _highCardLevelText;
    [SerializeField] private TextMeshProUGUI _pairLevelText;
    [SerializeField] private TextMeshProUGUI _twoPairLevelText;
    [SerializeField] private TextMeshProUGUI _threeOfAKindLevelText;
    [SerializeField] private TextMeshProUGUI _straightLevelText;
    [SerializeField] private TextMeshProUGUI _flushLevelText;
    [SerializeField] private TextMeshProUGUI _fullHouseLevelText;
    [SerializeField] private TextMeshProUGUI _fourOfAKindLevelText;
    [SerializeField] private TextMeshProUGUI _straightFlushLevelText;
    [SerializeField] private TextMeshProUGUI _royalFlushLevelText;

    [Foldout("Points Texts")]
    [SerializeField] private TextMeshProUGUI _highCardPointsText;
    [SerializeField] private TextMeshProUGUI _pairPointsText;
    [SerializeField] private TextMeshProUGUI _twoPairPointsText;
    [SerializeField] private TextMeshProUGUI _threeOfAKindPointsText;
    [SerializeField] private TextMeshProUGUI _straightPointsText;
    [SerializeField] private TextMeshProUGUI _flushPointsText;
    [SerializeField] private TextMeshProUGUI _fullHousePointsText;
    [SerializeField] private TextMeshProUGUI _fourOfAKindPointsText;
    [SerializeField] private TextMeshProUGUI _straightFlushPointsText;
    [SerializeField] private TextMeshProUGUI _royalFlushPointsText;

    [Foldout("Mult Texts")]
    [SerializeField] private TextMeshProUGUI _highCardMultText;
    [SerializeField] private TextMeshProUGUI _pairMultText;
    [SerializeField] private TextMeshProUGUI _twoPairMultText;
    [SerializeField] private TextMeshProUGUI _threeOfAKindMultText;
    [SerializeField] private TextMeshProUGUI _straightMultText;
    [SerializeField] private TextMeshProUGUI _flushMultText;
    [SerializeField] private TextMeshProUGUI _fullHouseMultText;
    [SerializeField] private TextMeshProUGUI _fourOfAKindMultText;
    [SerializeField] private TextMeshProUGUI _straightFlushMultText;
    [SerializeField] private TextMeshProUGUI _royalFlushMultText;
    private int _highCardPoints = 5;
    private int _pairPoints = 10;
    private int _twoPairPoints = 20;
    private int _threeOfAKindPoints = 30;
    private int _straightPoints = 30;
    private int _flushPoints = 35;
    private int _fullHousePoints = 40;
    private int _fourOfAKindPoints = 60;
    private int _straightFlushPoints = 100;
    private int _royalFlushPoints = 100;

    private float _highCardMult = 1;
    private float _pairMult = 2;
    private float _twoPairMult = 2;
    private float _threeOfAKindMult = 3;
    private float _straightMult = 4;
    private float _flushMult = 4;
    private float _fullHouseMult = 4;
    private float _fourOfAKindMult = 7;
    private float _straightFlushMult = 8;
    private float _royalFlushMult = 8;

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
        _infoPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        SetPointsAndMultTextOnStart();
    }

    private void SetPointsAndMultTextOnStart()
    {
        _highCardPointsText.text = _highCardPoints.ToString();
        _pairPointsText.text = _pairPoints.ToString();
        _twoPairPointsText.text = _twoPairPoints.ToString();
        _threeOfAKindPointsText.text = _threeOfAKindPoints.ToString();
        _straightPointsText.text = _straightPoints.ToString();
        _flushPointsText.text = _flushPoints.ToString();
        _fullHousePointsText.text = _fullHousePoints.ToString();
        _fourOfAKindPointsText.text = _fourOfAKindPoints.ToString();
        _straightFlushPointsText.text = _straightFlushPoints.ToString();
        _royalFlushPointsText.text = _royalFlushPoints.ToString();

        _highCardMultText.text = _highCardMult.ToString();
        _pairMultText.text = _pairMult.ToString();
        _twoPairMultText.text = _twoPairMult.ToString();
        _threeOfAKindMultText.text = _threeOfAKindMult.ToString();
        _straightMultText.text = _straightMult.ToString();
        _flushMultText.text = _flushMult.ToString();
        _fullHouseMultText.text = _fullHouseMult.ToString();
        _fourOfAKindMultText.text = _fourOfAKindMult.ToString();
        _straightFlushMultText.text = _straightFlushMult.ToString();
        _royalFlushMultText.text = _royalFlushMult.ToString();
    }

    public void UpdateText(Planet planet, int points, float mult)
    {
        switch (planet._handType)
        {
            case HandType.HighCard:
                _highCardLevelText.text = "Level: " + planet.Level.ToString();
                _highCardLevelText.color = Color.red;
                _highCardPoints += points;
                _highCardMult += mult;
                _highCardPointsText.text = _highCardPoints.ToString();
                _highCardMultText.text = _highCardMult.ToString();
                break;
            case HandType.Pair:
                _pairLevelText.text = "Level: " + planet.Level.ToString();
                _pairLevelText.color = Color.red;
                _pairPoints += points;
                _pairMult += mult;
                _pairPointsText.text = _pairPoints.ToString();
                _pairMultText.text = _pairMult.ToString();
                break;
            case HandType.TwoPair:
                _twoPairLevelText.text = "Level: " + planet.Level.ToString();
                _twoPairLevelText.color = Color.red;
                _twoPairPoints += points;
                _twoPairMult += mult;
                _twoPairPointsText.text = _twoPairPoints.ToString();
                _twoPairMultText.text = _twoPairMult.ToString();
                break;
            case HandType.ThreeOfAKind:
                _threeOfAKindLevelText.text = "Level: " + planet.Level.ToString();
                _threeOfAKindLevelText.color = Color.red;
                _threeOfAKindPoints += points;
                _threeOfAKindMult += mult;
                _threeOfAKindPointsText.text = _threeOfAKindPoints.ToString();
                _threeOfAKindMultText.text = _threeOfAKindMult.ToString();
                break;
            case HandType.Straight:
                _straightLevelText.text = "Level: " + planet.Level.ToString();
                _straightLevelText.color = Color.red;
                _straightPoints += points;
                _straightMult += mult;
                _straightPointsText.text = _straightPoints.ToString();
                _straightMultText.text = _straightMult.ToString();
                break;
            case HandType.Flush:
                _flushLevelText.text = "Level: " + planet.Level.ToString();
                _flushLevelText.color = Color.red;
                _flushPoints += points;
                _flushMult += mult;
                _flushPointsText.text = _flushPoints.ToString();
                _flushMultText.text = _flushMult.ToString();
                break;
            case HandType.FullHouse:
                _fullHouseLevelText.text = "Level: " + planet.Level.ToString();
                _fullHouseLevelText.color = Color.red;
                _fullHousePoints += points;
                _fullHouseMult += mult;
                _fullHousePointsText.text = _fullHousePoints.ToString();
                _fullHouseMultText.text = _fullHouseMult.ToString();
                break;
            case HandType.FourOfAKind:
                _fourOfAKindLevelText.text = "Level: " + planet.Level.ToString();
                _fourOfAKindLevelText.color = Color.red;
                _fourOfAKindPoints += points;
                _fourOfAKindMult += mult;
                _fourOfAKindPointsText.text = _fourOfAKindPoints.ToString();
                _fourOfAKindMultText.text = _fourOfAKindMult.ToString();
                break;
            case HandType.StraightFlush:
                _straightFlushLevelText.text = "Level: " + planet.Level.ToString();
                _straightFlushLevelText.color = Color.red;
                _straightFlushPoints += points;
                _straightFlushMult += mult;
                _straightFlushPointsText.text = _straightFlushPoints.ToString();
                _straightFlushMultText.text = _straightFlushMult.ToString();
                break;
            case HandType.RoyalFlush:
                _royalFlushLevelText.text = "Level: " + planet.Level.ToString();
                _royalFlushLevelText.color = Color.red;
                _royalFlushPoints += points;
                _royalFlushMult += mult;
                _royalFlushPointsText.text = _royalFlushPoints.ToString();
                _royalFlushMultText.text = _royalFlushMult.ToString();
                break;
        }
    }
    public void ShowInfoPanel()
    {
        _infoPanel.SetActive(true);
    }

    public void HideInfoPanel()
    {
        _infoPanel.SetActive(false);
    }
}
