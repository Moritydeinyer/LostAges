using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


public class FormattedGameData
{
    public string authentication_id { get; set; }
    public string spielstand_id { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public int z { get; set; }
    public int y_rot { get; set; }
    public string name { get; set; }
    public string ltp { get; set; }
    public int unlocked_areas { get; set; }
    public string story_id { get; set; }
    public string waypoints { get; set; }
    public int health { get; set; }
    public string respw { get; set; }
}

public class FormattedPlayerSettings
{
    public string authentication_id { get; set; }
    public string volume { get; set; }
    public string bindings { get; set; }
    public string diagnostics { get; set; }
    public string autosave { get; set; }
}



public class SyncAction
{
    public string actionType { get; set; } // "save", "delete", "update_player"
    public string id { get; set; } // GameID oder 0 für PlayerData
    public string jsonData { get; set; }
}


public class saveManager : MonoBehaviour
{
    public static saveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public static string LocalPath(string filename)
    {
        return Path.Combine(Application.persistentDataPath, filename);
    }



    public async Task<jsonPlayerData> getPlayerData(string authentication_id)
    {
        jsonPlayerData playerData = LoadPlayerDataLocally();
        //Debug.LogWarning(playerData.authentication_id);
        if (playerData != null && (authentication_id == null || authentication_id == ""))
        {
            authentication_id = playerData.authentication_id; // Use the stored authentication ID if available
        }
        try
        {
            string data = "{\"authentication_id\":\"" + authentication_id + "\"}";
            HttpClient client = new HttpClient();
            StringContent queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/get_user_data", queryString);
            string responseBody = await response.Content.ReadAsStringAsync();
            playerData = JsonConvert.DeserializeObject<jsonPlayerData>(responseBody);
            playerData.authentication_id = authentication_id; // Set the authentication ID
            SavePlayerDataLocally(playerData);
            return playerData;
        }
        catch (Exception e)
        {
            return playerData;
        }
    }

    public async Task<jsonGameData> getSpielstand(string id, string authentication_id)
    {
        jsonGameData gameData = null;
        try
        {
            string data = "{\"spielstand_id\":\"" + id + "\",\"authentication_id\":\"" + authentication_id + "\"}";
            HttpClient client = new HttpClient();
            StringContent queryString = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/get_spielstand", queryString);
            string responseBody = await response.Content.ReadAsStringAsync();
            gameData = JsonConvert.DeserializeObject<jsonGameData>(responseBody);
            SaveGameDataLocally(gameData);
            return gameData;
        }
        catch (Exception e)
        {
            return LoadGameDataLocally(id);
        }
    }






    public jsonPlayerData LoadPlayerDataLocally()
    {
        string path = LocalPath("playerdata.json");
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        jsonPlayerData playerData = JsonConvert.DeserializeObject<jsonPlayerData>(json);

        if (playerData.authentication_id == null || playerData.authentication_id == "")
            playerData = null;

        return playerData;
    }

    public void SavePlayerDataLocally(jsonPlayerData playerData)
    {
        string path = LocalPath("playerdata.json");
        File.WriteAllText(path, JsonConvert.SerializeObject(playerData));
    }

    public jsonGameData LoadGameDataLocally(string id)
    {
        string path = LocalPath("gamedata" + id + ".json");
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        jsonGameData gameData = JsonConvert.DeserializeObject<jsonGameData>(json);
        return gameData;
    }

    public void SaveGameDataLocally(jsonGameData gameData)
    {
        string path = LocalPath("gamedata" + gameData.id + ".json");
        File.WriteAllText(path, JsonConvert.SerializeObject(gameData));
    }





    public async Task ProcessSyncQueue(string authentication_id)
    {
        var queue = LoadSyncQueue();
        var client = new HttpClient();
        bool restartQueue;

        do
        {
            restartQueue = false;
            var newQueue = new List<SyncAction>();

            foreach (var action in queue)
            {
                try
                {
                    HttpResponseMessage response = null;
                    var content = new StringContent(action.jsonData, Encoding.UTF8, "application/json");

                    switch (action.actionType)
                    {
                        case "spielstand":
                            string oldPath = LocalPath("gamedata" + action.id + ".json");

                            response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/save_spielstand", content);
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Debug.Log(responseBody);

                            if (response.IsSuccessStatusCode && responseBody.Contains("sp_ip"))
                            {
                                JObject res = JObject.Parse(responseBody);
                                string newId = res["sp_ip"].ToString();

                                if (action.id.Contains("create") && File.Exists(oldPath))
                                    File.Delete(oldPath);

                                jsonGameData gameData = JsonConvert.DeserializeObject<jsonGameData>(action.jsonData);
                                gameData.id = newId;
                                SaveGameDataLocally(gameData);

                                jsonPlayerData playerData = LoadPlayerDataLocally();
                                var games = playerData.games.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();
                                games.Remove(action.id);
                                if (!games.Contains(newId.ToString()))
                                    games.Add(newId.ToString());

                                playerData.games = string.Join(";", games) + ";";
                                SavePlayerDataLocally(playerData);

                                string oldId = action.id;
                                List<SyncAction> syncQueue = new List<SyncAction>();
                                foreach (var a in LoadSyncQueue())
                                {
                                    if (a.id == oldId)
                                    {
                                        a.id = newId;
                                    }
                                    syncQueue.Add(a);
                                }
                                SaveSyncQueue(syncQueue);
                                restartQueue = true;
                                break;
                            }
                            break;
                        case "account":
                            var authObj = JsonConvert.DeserializeObject<FormattedPlayerSettings>(action.jsonData);
                            var authCheck = new StringContent("{\"authentication_id\":\"" + authObj.authentication_id + "\"}", Encoding.UTF8, "application/json");

                            try
                            {
                                response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/get_user_data", authCheck);
                                string createBody = await response.Content.ReadAsStringAsync();
                                Debug.Log("Account creation/check response: " + createBody);
                            }
                            catch (Exception e)
                            {
                                Debug.LogWarning("Account-Erstellung/Check fehlgeschlagen: " + e.Message);
                                newQueue.Add(action);
                                continue;
                            }

                            try
                            {
                                var updateContent = new StringContent(action.jsonData, Encoding.UTF8, "application/json");
                                response = await client.PostAsync("https://iab-services.ddns.net/api/gta_speichersdorf/update_account", updateContent);
                                if (!response.IsSuccessStatusCode)
                                {
                                    Debug.LogWarning("Update für Account fehlgeschlagen.");
                                    newQueue.Add(action);
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogWarning("Account-Update fehlgeschlagen: " + e.Message);
                                newQueue.Add(action);
                            }
                            break;

                        default:
                            throw new InvalidOperationException("Unknown action type: " + action.actionType);
                    }
                    if (response == null || !response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Fehlerhafte Antwort vom Server: {response}");
                    }
                    await getPlayerData(authentication_id); // Update player data after successful sync
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Sync fehlgeschlagen für {action.actionType} ID {action.id}: {e.Message}");
                    newQueue.Add(action);
                }
                if (restartQueue)
                {
                    break;
                }
            }
            if (!restartQueue)
            {
                SaveSyncQueue(newQueue);
            }
        }
        while (restartQueue);
    }

    public void AddToQueue(string actionType, string id, string jsonData)
    {
        var queue = LoadSyncQueue();
        queue.Add(new SyncAction { actionType = actionType, id = id, jsonData = jsonData });
        SaveSyncQueue(queue);
    }

    public void SaveSyncQueue(List<SyncAction> queue)
    {
        string path = LocalPath("offline_sync_queue.json");
        File.WriteAllText(path, JsonConvert.SerializeObject(queue));
    }

    public List<SyncAction> LoadSyncQueue()
    {
        string path = LocalPath("offline_sync_queue.json");
        if (!File.Exists(path)) return new List<SyncAction>();

        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<List<SyncAction>>(json);
    }

    public jsonPlayerData updatePlayerData(string authentication_id, jsonPlayerData playerDataNew)
    {
        playerDataNew.authentication_id = authentication_id;
        var formattedSettings = ConvertToFormattedSettings(playerDataNew);
        AddToQueue("account", "0", JsonConvert.SerializeObject(formattedSettings));
        SavePlayerDataLocally(playerDataNew);
        return playerDataNew;
    }

    public jsonGameData updateSpielstand(string authentication_id, jsonGameData gameDataNew, int action)
    {
        switch (action)
        {
            case 0: //delete 
                if (gameDataNew != null && !string.IsNullOrEmpty(gameDataNew.id))
                {
                    string data = "{\"authentication_id\":\"" + authentication_id + "\", \"spielstand_id\":\"delete\", \"deleteID\":\"" + gameDataNew.id + "\"}";
                    AddToQueue("spielstand", gameDataNew.id, data);
                    if (File.Exists(LocalPath("gamedata" + gameDataNew.id + ".json")))
                    {
                        File.Delete(LocalPath("gamedata" + gameDataNew.id + ".json"));
                    }

                    jsonPlayerData playerDatat = LoadPlayerDataLocally();
                    var gameList = playerDatat.games.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();
                    gameList.Remove(gameDataNew.id.ToString());
                    playerDatat.games = gameList.Count > 0 ? string.Join(";", gameList) + ";" : "";

                    SavePlayerDataLocally(playerDatat);
                }
                return null;
            case 1: //save
                if (gameDataNew.id == "create")
                {
                    gameDataNew.id = "create" + -(int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // Generate a unique ID for new game data
                }
                SaveGameDataLocally(gameDataNew);
                var formattedData = ConvertToFormatted(gameDataNew, authentication_id);
                AddToQueue("spielstand", gameDataNew.id, JsonConvert.SerializeObject(formattedData));

                jsonPlayerData playerData = LoadPlayerDataLocally();
                var games = playerData.games.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();
                if (!games.Contains(gameDataNew.id.ToString()))
                    games.Add(gameDataNew.id.ToString());

                playerData.games = string.Join(";", games) + ";";
                SavePlayerDataLocally(playerData);

                return gameDataNew;
            default:
                throw new ArgumentException("Ungültiger Aktionscode in updateSpielstand(): " + action);
        }
    }

    private FormattedGameData ConvertToFormatted(jsonGameData data, string auth_id)
    {
        return new FormattedGameData
        {
            authentication_id = auth_id,
            spielstand_id = data.id,
            x = data.x,
            y = data.y,
            z = data.z,
            y_rot = data.y_rot,
            name = data.name,
            ltp = data.ltp,
            unlocked_areas = data.unlocked_areas,
            story_id = data.story_id,
            waypoints = data.waypoints,
            health = data.health,
            respw = data.respw
        };
    }

    private FormattedPlayerSettings ConvertToFormattedSettings(jsonPlayerData data)
    {
        return new FormattedPlayerSettings
        {
            authentication_id = data.authentication_id,
            volume = data.settingsVolume.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture).Replace(".", ","), // ← wichtig: deutsches Komma
            bindings = data.bindings,
            diagnostics = data.diagnostics,
            autosave = data.autosave
        };
    }



    public void logout()
    {
        string dataPath = Application.persistentDataPath;
        string playerPath = LocalPath("playerdata.json");

        if (File.Exists(playerPath))
        {
            File.Delete(playerPath);
        }

        string[] gameFiles = Directory.GetFiles(dataPath, "gamedata*.json");
        foreach (string file in gameFiles)
        {
            File.Delete(file);
        }

        SaveSyncQueue(new List<SyncAction>());
    }


}