using System;
using UnityEngine;
using YH.Players;

public class GameManager : MonoSingleton<GameManager>
{
    public bool IsGameOver { get; private set; } = false;

    [SerializeField] private GameOverUI _gameOverUI;

    public void GameOver()
    {
        _gameOverUI.Open();
        IsGameOver = true;
    }
}
