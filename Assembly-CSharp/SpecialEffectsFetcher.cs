using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

#if EVOS
// Custom SpecialEffectsFetcher
public class SpecialEffectsFetcher
{
    public delegate void OnSpecialEffectsFetched(Dictionary<string, List<string>> effectsByHandle);
    public event OnSpecialEffectsFetched SpecialEffectsFetched;

    public IEnumerator FetchSpecialEffectsFromApi()
    {
        var apiUrl = HydrogenConfig.Get().ApiSpecialEffects;
        yield return HttpGetCoroutine(apiUrl, OnHttpResponse);
    }

    private void OnHttpResponse(string response, string error)
    {
        if (error != null)
        {
            Debug.Log($"Request error: {error}");
            SpecialEffectsFetched?.Invoke(null);
            return;
        }

        try
        {
            JObject jsonDoc = JObject.Parse(response);
            var data = jsonDoc["data"] as JArray;
            if (data != null)
            {
                var effectsByHandle = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
                foreach (var item in data.Children<JObject>())
                {
                    var attributes = item["attributes"] as JObject;
                    if (attributes == null)
                        continue;

                    // Prefer explicit playername/effectname fields when present
                    var playerName = (string)attributes["playername"];
                    var effectName = (string)attributes["effectname"];
                    if (!string.IsNullOrEmpty(playerName) && !string.IsNullOrEmpty(effectName))
                    {
                        string key = playerName.Trim();
                        Console.WriteLine(key);
                        if (!effectsByHandle.TryGetValue(key, out var list))
                        {
                            list = new List<string>();
                            effectsByHandle[key] = list;
                            Console.WriteLine(list);
                        }
                        // avoid duplicates
                        if (!list.Contains(effectName))
                        {
                            list.Add(effectName);
                        }
                    }
                }

                SpecialEffectsFetched?.Invoke(effectsByHandle);
            }
            else
            {
                SpecialEffectsFetched?.Invoke(null);
            }
        }
        catch (JsonException ex)
        {
            Debug.LogError($"JSON parsing error: {ex.Message}");
            SpecialEffectsFetched?.Invoke(null);
        }
    }

    private IEnumerator HttpGetCoroutine(string url, Action<string, string> callback)
    {
        using (WWW client = new WWW(url))
        {
            yield return client;
            if (!string.IsNullOrEmpty(client.error))
            {
                callback(null, client.error);
            }
            else
            {
                callback(client.text, null);
            }
        }
    }
}
#endif
