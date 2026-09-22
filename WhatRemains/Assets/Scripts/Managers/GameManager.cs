using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static bool IsBossDead = false;

    [SerializeField] private ServerTest serverTest;

    [SerializeField] private Player player;
    
    public Player Player => player;

    public GameOverMenu gameOverMenu;

    private void Start()
    {
        IsBossDead = false;
        //serverTest.CheckServerConnection();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            player.ResetPlayer();
        }
    }

    public void AddPlayerExp(float expAmount)
    {
        PlayerExp playerExp = player.GetComponent<PlayerExp>();
        playerExp.AddExp(expAmount);
    }
}
