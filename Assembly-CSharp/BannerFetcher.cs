using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

#if EVOS
// Custom BannerFetcher
public class BannerFetcher
{
    public delegate void OnBannersFetched(Dictionary<string, string> bannersByHandle);
    public event OnBannersFetched BannersFetched;
    private readonly Dictionary<string, Sprite> imageCache = new Dictionary<string, Sprite>();

    public IEnumerator FetchBannersFromApi()
    {
        var apiUrl = HydrogenConfig.Get().ApiBannerUrl;
        yield return HttpGetCoroutine(apiUrl, OnHttpResponse);
    }

    private void OnHttpResponse(string response, string error)
    {
        if (error != null)
        {
            Debug.Log($"Request error: {error}");
            BannersFetched?.Invoke(null);
            return;
        }
        try
        {
            JObject jsonDoc = JObject.Parse(response);
            var data = jsonDoc["data"] as JArray;
            if (data != null)
            {
                var bannersByHandle = new Dictionary<string, string>();
                foreach (var item in data.Children<JObject>())
                {
                    var attributes = item["attributes"] as JObject;
                    var handle = (string)attributes["handle"];
                    var bannerUrl = (string)attributes["banner"];
                    bannersByHandle[handle.Split('#')[0].Trim()] = bannerUrl;
                }
                BannersFetched?.Invoke(bannersByHandle);
            }
            else
            {
                BannersFetched?.Invoke(null);
            }
        }
        catch (JsonException ex)
        {
            Debug.LogError($"JSON parsing error: {ex.Message}");
            BannersFetched?.Invoke(null);
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

    public IEnumerator LoadImageFromURL(string url, Action<Sprite> callback)
    {
        // Check if the image is already cached
        if (imageCache.TryGetValue(url, out Sprite cachedSprite))
        {
            callback(cachedSprite);
            yield break;
        }

        using (WWW www = new WWW(url))
        {
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                Texture2D texture = www.texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                imageCache[url] = sprite;
                callback(sprite);
            }
            else
            {
                Debug.LogError($"Failed to load image: {www.error}");
                callback(null);
            }
        }
    }
}
#endif