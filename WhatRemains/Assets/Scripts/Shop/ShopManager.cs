using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerResponse {
    public bool success;
    public PlayerData player;
}

[System.Serializable]
public class PlayerData {
    public int id;
    public int coins;
    public int discount;
}

public class ShopManager : Singleton<ShopManager>
{
    [Header("Config")]
    [SerializeField] private ShopCard shopCardPrefab;
    [SerializeField] private Transform shopContainer;

    [Header("Items")]
    [SerializeField] private ShopItem[] items;

    [Header("Server")]
    public float playerDiscount = 0f;
    public float pollInterval = 5f;
    private bool keepPolling = true;

    private void Start()
    {
        LoadShop();
        StartPolling();
    }

    public void StartPolling()
    {
        keepPolling = true;
        StartCoroutine(PollStatusLoop());
    }

    public void StopPolling()
    {
        keepPolling = false;
    }

    private IEnumerator PollStatusLoop()
    {
        while (keepPolling)
        {
            yield return StartCoroutine(FetchDiscountCoroutine());
            yield return new WaitForSeconds(pollInterval);
        }
    }

    private void LoadShop()
    {
        for(int i = 0; i < items.Length; i++)
        {
            ShopCard card = Instantiate(shopCardPrefab, shopContainer);
            card.ConfigShopCard(items[i]);
        }
    }

    public void ApplyDiscountToShopItems()
    {
        // For each ShopItem or each ShopCard in the scene:
        ShopCard[] shopCards = FindObjectsOfType<ShopCard>();
        foreach (var card in shopCards)
        {
            card.ApplyDiscount(playerDiscount);
        }
    }

    private IEnumerator FetchDiscountCoroutine()
    {
        string url = "https://whatremains-server.vercel.app/player/1";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || 
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string responseJson = request.downloadHandler.text;
            Debug.Log("Player Data: " + responseJson);

            // parse JSON
            PlayerResponse resp = JsonUtility.FromJson<PlayerResponse>(responseJson);
            if (resp != null && resp.success)
            {
                // store discount in a field
                playerDiscount = resp.player.discount;
                Debug.Log("Discount from server = " + playerDiscount);

                // Optionally re-load or re-apply shop prices
                ApplyDiscountToShopItems();
            }
        }
    }
}
