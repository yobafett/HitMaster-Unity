using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button restartBtn;

    public delegate void UIManagerEventHandler();
    public static event UIManagerEventHandler OnStartBtnClick;
    public static event UIManagerEventHandler OnRestartBtnClick;
    
    private void Awake()
    {
        startBtn.onClick.AddListener((() =>
        {
            startBtn.gameObject.SetActive(false);
            OnStartBtnClick?.Invoke();
        }));
        
        restartBtn.onClick.AddListener((() =>
        {
            restartBtn.gameObject.SetActive(false);
            startBtn.gameObject.SetActive(true);
            OnRestartBtnClick?.Invoke();
        }));
    }

    private void OnEnable()
    {
        GameManager.OnLevelEnd += ShowRestart;
    }

    private void OnDisable()
    {
        GameManager.OnLevelEnd -= ShowRestart;
    }
    
    private void ShowRestart() => restartBtn.gameObject.SetActive(true);
}
