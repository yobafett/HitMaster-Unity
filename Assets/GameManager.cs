using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private EnemyHealth[] enemyHealths;
    [SerializeField] private int[] enemiesPerPlatform;

    public delegate void GameManagerEventHandler();
    public static event GameManagerEventHandler OnLevelEnd;
    
    private PlayerMover _playerMover;
    private PlayerShooter _playerShooter;
    private int _platformCount;
    private int _scoreCount;

    private void Awake()
    {
        _playerMover = player.GetComponent<PlayerMover>();
        _playerShooter = player.GetComponent<PlayerShooter>();
    }

    private void OnEnable()
    {
        UIManager.OnStartBtnClick += _playerMover.MoveForward;
        UIManager.OnRestartBtnClick += RestartGame;
        PlayerMover.OnMovementDone += EnterShootingState;
        EnemyHealth.OnEnemyKilled += CheckEnemiesCount;
    }

    private void OnDisable()
    {
        UIManager.OnStartBtnClick -= _playerMover.MoveForward;
        UIManager.OnRestartBtnClick -= RestartGame;
        PlayerMover.OnMovementDone -= EnterShootingState;
        EnemyHealth.OnEnemyKilled -= CheckEnemiesCount;
    }

    private void EnterShootingState()
    {
        if (_platformCount >= enemiesPerPlatform.Length)
        {
            OnLevelEnd?.Invoke();
        }
        else
        {
            _playerShooter.EnableShoot();
        }
    }
    
    private void CheckEnemiesCount()
    {
        _scoreCount++;
        if (_scoreCount >= enemiesPerPlatform[_platformCount])
        {
            _playerShooter.DisableShoot();
            _playerMover.MoveForward();
            _scoreCount = 0;
            _platformCount++;
        }
    }

    private void RestartGame()
    {
        _scoreCount = 0;
        _platformCount = 0;
        
        _playerMover.RestartPlayer();
        
        foreach (var enemyHealth in enemyHealths)
            enemyHealth.Revive();
    }
}
