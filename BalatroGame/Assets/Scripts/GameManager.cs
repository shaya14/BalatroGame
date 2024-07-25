using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    // Serialized fields
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _winScreen;


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
        Time.timeScale = 1;
    }

    public void MakeTimeFast(int num)
    {
        Time.timeScale = num;
    }

    public void GameOver()
    {
        _gameOverPanel.gameObject.SetActive(true);
    }

    public void WinGame()
    {
        _winScreen.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        _gameOverPanel.gameObject.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
