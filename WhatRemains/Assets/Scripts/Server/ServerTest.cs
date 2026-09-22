using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ServerTest : MonoBehaviour
{
    public void CheckServerConnection()
    {
        StartCoroutine(CheckServerConnectionCoroutine());
    }

    private IEnumerator CheckServerConnectionCoroutine()
    {
        string url = "https://whatremains-server.vercel.app/test-connection";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error + "\n" + request.downloadHandler.text);
        }
        else
        {
            // Success
            string responseBody = request.downloadHandler.text;
        }
    }
}