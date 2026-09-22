using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using Unity.VisualScripting;
using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
    [SerializeField] private float coinTest = 100;

    public float Coins { get; private set; }
    private const string COIN_KEY = "Coins";

    private void Start()
    {
        Coins = SaveGame.Load(COIN_KEY, coinTest);
    }

    public void SetCoins(float amount)
    {
        Coins = amount;
        SaveGame.Save(COIN_KEY, Coins);
    }

    public void AddCoins(float amount)
    {
        Coins += amount;
        SaveGame.Save(COIN_KEY, Coins);
    }

    public void RemoveCoins(float amount)
    {
        if(Coins >= amount)
        {
            Coins -= amount;
            SaveGame.Save(COIN_KEY, Coins);
        }
    }
}
