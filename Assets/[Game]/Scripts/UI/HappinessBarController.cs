﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HappinessBarController : MonoBehaviour
{
    public Slider progressBar;
    public Image fillArea;
    
    public float happinessAmount; //Check To Do
    public float defaultHappiness = 20f;
    private float happinessTotal = 0;
    private float HappinessTotal 
    { 
        get 
        { 
            if(happinessTotal == 0) 
            {
                happinessTotal = LevelManager.Instance.CurrentLevel.happinessTotal;
            }
            return happinessTotal;
        } 
    }
    private float currentHappiness;
    private float tweenDelay = 0.3f;
    private Color color;

    private void OnEnable()
    {
        EventManager.OnGameStarted.AddListener(ResetHappinesBar);
        EventManager.OnOrderCompleted.AddListener(IncreaseHappiness);
        EventManager.OnOrderFailed.AddListener(DecreaseHappiness);
    }

    private void OnDisable()
    {
        EventManager.OnGameStarted.RemoveListener(ResetHappinesBar);
        EventManager.OnOrderCompleted.RemoveListener(IncreaseHappiness);
        EventManager.OnOrderFailed.RemoveListener(DecreaseHappiness);
    }

    private void Start()
    {
        color = fillArea.color;
        ResetHappinesBar();
    }

    private void ResetHappinesBar() 
    {
        currentHappiness = defaultHappiness;
        fillArea.color = color;
        UpdateHappinesBar();
    }

    public void IncreaseHappiness() 
    {
        currentHappiness += happinessAmount;
        UpdateHappinesBar();
        if (currentHappiness >= HappinessTotal)
        {
            EventManager.OnLevelSuccesed.Invoke();
        }
        else
        {
            OrderManager.Instance.NewOrder();
        }
    }

    public void DecreaseHappiness() 
    {        
        currentHappiness -= happinessAmount/2f;
        UpdateHappinesBar();
        if (currentHappiness <= 0)
        {
            EventManager.OnLevelFailed.Invoke();
        }

        else 
        {
            OrderManager.Instance.NewOrder();
        }
    }

    private void UpdateHappinesBar()
    {
        float ratio = currentHappiness / HappinessTotal;
        DOTween.To(() => progressBar.value, (a) => progressBar.value = a, ratio, tweenDelay).
            OnComplete(() => {                
                if (ratio >= 1) fillArea.color = Color.green;
                else if (ratio <= 0) fillArea.color = Color.red;                
            });
    }
}
