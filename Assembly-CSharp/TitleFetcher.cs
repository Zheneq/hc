using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

#if !VANILLA && !SERVER
// Custom titles
public class TitleFetcher
{
    public delegate void OnTitlesFetched(Dictionary<string, string> titlesByHandle);
    public event OnTitlesFetched TitlesFetched;

    public IEnumerator FetchTitlesFromApi()
    {
        var apiUrl = HydrogenConfig.Get().ApiTitleUrl;
        yield return HttpGetCoroutine(apiUrl, OnHttpResponse);
    }

    private void OnHttpResponse(string response, string error)
    {
        if (error != null)
        {
            Debug.Log($"Request error: {error}");
            TitlesFetched?.Invoke(null);
            return;
        }

        try
        {
            JObject jsonDoc = JObject.Parse(response);
            var data = jsonDoc["data"] as JArray;

            if (data != null)
            {
                var titlesByHandle = new Dictionary<string, string>();

                foreach (var item in data.Children<JObject>())
                {
                    var attributes = item["attributes"] as JObject;
                    var handle = (string)attributes["handle"];
                    var title = (string)attributes["title"];

                    titlesByHandle[handle.Split('#')[0].Trim()] = title;
                }

                TitlesFetched?.Invoke(titlesByHandle);
            }
            else
            {
                TitlesFetched?.Invoke(null);
            }
        }
        catch (JsonException ex)
        {
            TitlesFetched?.Invoke(null);
        }
    }

    private IEnumerator HttpGetCoroutine(string url, Action<string, string> callback)
    {
        bool flag = false;
        WWW client = null;

        try
        {
            client = new WWW(url);

            yield return client;

            flag = true;

            if (!string.IsNullOrEmpty(client.error))
            {
                callback(null, client.error);
            }
            else
            {
                callback(client.text, null);
            }
        }
        finally
        {
            if (!flag && client != null)
            {
                client.Dispose();
            }
        }
    }
}
#endif