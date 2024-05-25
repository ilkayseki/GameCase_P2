using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject]
    private PieceController pieceController;

    private bool isGame = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pieceController.OnClick();
        }
    }


    public void GameOver()
    {
        Debug.LogError("Game OVER");
        isGame = false;
    }
    
}
