using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject]
    private PieceController pieceController;

    private bool _isGame = true;
    
    private int _score=0;

    private void OnEnable()
    {
        pieceController.Scored += SetScore;
    }

    private void OnDisable()
    {
        pieceController.Scored -= SetScore;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pieceController.OnClick();
        }
    }


    public void GameOver()
    {
        
    }

    public void SetScore()
    {
        _score++;
    }
    
    public int GetScore()
    {
        return _score;
    }
}
