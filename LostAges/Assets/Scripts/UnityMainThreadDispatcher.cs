using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.CSharp;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using Cainos.PixelArtTopDown_Basic;
using System.Threading;
using UnityEngine.AI;
using System.Globalization;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();
    private static UnityMainThreadDispatcher _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Verhindert doppelte Instanzen
        }
    }

    public static UnityMainThreadDispatcher Instance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<UnityMainThreadDispatcher>();
            if (_instance == null)
            {
                Debug.LogError("❌ Kein UnityMainThreadDispatcher in der Szene gefunden! Füge ein GameObject mit diesem Skript hinzu.");
            }
        }
        return _instance;
    }
    public void Enqueue(Action action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(() =>
        {
            action.Invoke(); // Ensure action (exec) is called on the main thread
        });
        }
    }
    void Update()
    {
        // Process all actions in the queue
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                Action action = _executionQueue.Dequeue();
                action.Invoke();  // Execute the action
            }
        }
    }

    public void exec(string paymentId, string payerId, string accessToken)
    {
        StartCoroutine(ExecutePayment(paymentId, payerId, accessToken));
    }

    private IEnumerator ExecutePayment(string paymentId, string payerId, string aT)
    {
        string url = $"https://api.sandbox.paypal.com/v1/payments/payment/{paymentId}/execute";
        string jsonData = $"{{\"payer_id\": \"{payerId}\"}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + aT);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Zahlung abgeschlossen: " + request.downloadHandler.text);
            var response = JsonUtility.FromJson<PayPalPaymentResponse>(request.downloadHandler.text);

            if (response.state == "approved")
            {
                Debug.Log("✅ Zahlung erfolgreich abgeschlossen!");
            }
            else
            {
                Debug.LogError("⚠ Zahlung nicht abgeschlossen: " + response.state);
            }
        }
        else
        {
            Debug.LogError("❌ Fehler beim Abschluss der Zahlung: " + request.error);
        }
    }
}