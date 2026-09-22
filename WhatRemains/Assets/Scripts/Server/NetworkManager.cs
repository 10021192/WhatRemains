using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class StatusResponse
{
    public bool success;
    public string lastClaimedAt;
    public int rewardQuantity;
    public int timeLeftSeconds;
}

public class NetworkManager : MonoBehaviour
{
    [Header("Server")]
    public float pollInterval = 5f;
    private bool keepPolling = true;

    // We'll store the last known claimed time so we know if it changed
    private string lastKnownClaimTime;

    private void Start()
    {
        // Load the previous known claim time, if any
        lastKnownClaimTime = PlayerPrefs.GetString("lastClaimedTimeKey", "");
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
            yield return StartCoroutine(CheckRewardStatusCoroutine());
            yield return new WaitForSeconds(pollInterval);
        }
    }

    private IEnumerator CheckRewardStatusCoroutine()
    {
        string url = "https://whatremains-server.vercel.app/hourly_rewards/status/1";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError 
            || request.result == UnityWebRequest.Result.ProtocolError)
        {
            //Debug.LogError("CheckRewardStatus error: " + request.error + "\n" + request.downloadHandler.text);
        }
        else
        {
            string responseBody = request.downloadHandler.text;

            StatusResponse resp = JsonUtility.FromJson<StatusResponse>(responseBody);
            if (resp != null && resp.success)
            {
                // (Optional) Log how many seconds remain until the NEXT claim is allowed
                if (resp.timeLeftSeconds > 0)
                {
                    //Debug.Log($"Reward is on cooldown for {resp.timeLeftSeconds} seconds.");
                }

                // Regardless of cooldown, check if there's a NEW claim time we haven't processed yet
                if (!string.IsNullOrEmpty(resp.lastClaimedAt) && resp.lastClaimedAt != lastKnownClaimTime)
                {
                    // That means the companion app just claimed the reward at a new time.
                    CoinManager.Instance.AddCoins(resp.rewardQuantity);

                    // Remember this claim time, so we don't add again on next poll
                    lastKnownClaimTime = resp.lastClaimedAt;
                    PlayerPrefs.SetString("lastClaimedTimeKey", lastKnownClaimTime);
                    PlayerPrefs.Save();
                }
            }
        }
    }
}