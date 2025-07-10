using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Logger : MonoBehaviour
{
    private StreamWriter logWriter;
    private string logFilePath;
    private string fileName;
    private bool isUploading = false;
    [SerializeField] private escMenuController escMC;

    void Awake()
    {
        fileName = "log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
        logFilePath = Path.Combine(Application.persistentDataPath, fileName);
        logWriter = new StreamWriter(logFilePath, true, Encoding.UTF8);

        Application.logMessageReceived += HandleLog;
    }

    void Update()
    {
        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                logWriter?.WriteLine($"[{DateTime.Now:HH:mm:ss}] [key] {key}");
            }
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (logWriter == null) return;

        string time = DateTime.Now.ToString("HH:mm:ss");
        string tag = type switch
        {
            LogType.Error => "[error]",
            LogType.Warning => "[warning]",
            LogType.Exception => "[exception]",
            _ => "[log]"
        };

        logWriter.WriteLine($"[{time}] {tag} {logString}");
        if (type == LogType.Exception)
        {
            logWriter.WriteLine($"[stacktrace] {stackTrace}");
        }
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;

        if (logWriter != null)
        {
            logWriter.Close();
            logWriter = null;
        }

        if (!isUploading)
        {
            isUploading = true;
            UploadLogFile(logFilePath, fileName);
        }
    }

    public async void UploadLogFile(string filePath, string filename)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("[log] Logfile nicht gefunden.");
            return;
        }

        if (escMC.playerData.diagnostics == "1")
        {
            try
            {
                string fileContent = File.ReadAllText(filePath);

                // JSON manuell erstellen, weil Unity's JsonUtility unterstützt keine verschachtelten Objekte gut
                string jsonPayload = $"{{\"log\":{JsonEscape(fileContent)},\"filename\":\"{filename}\"}}";

                using (HttpClient client = new HttpClient())
                {
                    var queryString = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/diagnostics", queryString);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    Debug.Log("[log] Server Response: " + response.StatusCode + " - " + responseBody);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("[error] Fehler beim Senden des Logfiles: " + ex.Message);
            }
        }
        else
        { 
            Debug.LogWarning("[log] Logfile Upload deaktiviert, da 'diagnostics' nicht aktiviert ist.");
        }
    }

    // Escape JSON-String damit Sonderzeichen korrekt übertragen werden
    private string JsonEscape(string str)
    {
        return "\"" + str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r") + "\"";
    }
}
