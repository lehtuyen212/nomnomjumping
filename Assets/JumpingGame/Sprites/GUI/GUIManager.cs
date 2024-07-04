using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager> 
{
    public GameObject mainMenu;
    public GameObject gameplay;
    public Text scoreTxt;
    public PauseDialog pauseDialog;
    public GameoverDialog gameoverDialog;

    public override void Awake()
    {
        MakeSingleton(false);
    }
    public void ShowGameplay(bool isShow)
    {
        if (gameplay) 
        {
            gameplay.SetActive(isShow);
        }
        
    }
    public void ShowMainmenu(bool isShow)
    {
        if (mainMenu)
        {
            mainMenu.SetActive(isShow);
        }
    }
    public void UpdateScore(int  score)
    {
        if(scoreTxt)
        {
            scoreTxt.text = "Score: "+ score.ToString();
        }
    }
}
