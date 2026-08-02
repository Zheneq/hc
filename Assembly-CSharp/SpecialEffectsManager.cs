using System;
using System.Collections.Generic;
using UnityEngine;

#if EVOS
public class SpecialEffectsManager
{
    private SpecialEffectsFetcher effectsFetcher;
    private Dictionary<string, List<string>> cachedEffectsByPlayer;
    private bool debugEnabled = false;
    private static SpecialEffectsManager m_instance;

    public static SpecialEffectsManager GetInstance()
    {
        if (m_instance == null)
        {
            m_instance = new SpecialEffectsManager();
        }
        return m_instance;
    }



    public void SetDebugEnabled(bool enabled)
    {
        debugEnabled = enabled;
        Debug.Log($"SpecialEffectsManager: DebugEnabled set to {debugEnabled}");
    }

    public void DumpCache()
    {
        if (cachedEffectsByPlayer == null || cachedEffectsByPlayer.Count == 0)
        {
            Debug.Log("SpecialEffectsManager: Cache is empty");
            return;
        }
        Debug.Log($"SpecialEffectsManager: Dumping {cachedEffectsByPlayer.Count} cached entries:");
        foreach (var kv in cachedEffectsByPlayer)
        {
            Debug.Log($"SpecialEffectsManager: {kv.Key} => {string.Join(", ", kv.Value.ToArray())}");
        }
    }

    private SpecialEffectsManager()
    {
    }

    public void Init()
    {
        effectsFetcher = new SpecialEffectsFetcher();
        effectsFetcher.SpecialEffectsFetched += OnSpecialEffectsFetched;
        RefreshSpecialEffects();
    }

    public void RefreshSpecialEffects()
    {
        if (CoroutineRunner.Instance == null || effectsFetcher == null)
            return;

        CoroutineRunner.Instance.RunCoroutine(effectsFetcher.FetchSpecialEffectsFromApi());
    }

    // Return the effect name(s) for a given handle (normalized by removing suffix after '#').
    // If multiple effects exist, they are joined with ", ". Returns null if none.
    public string GetEffectForHandle(string handle)
    {
        if (string.IsNullOrEmpty(handle) || cachedEffectsByPlayer == null)
            return null;

        string key = handle.Trim();
        Console.WriteLine(key);
        if (cachedEffectsByPlayer.TryGetValue(key, out var list) && list != null && list.Count > 0)
        {
            Console.WriteLine(string.Join(", ", list.ToArray()));
            return string.Join(", ", list.ToArray());
        }

        return null;
    }

    private void OnSpecialEffectsFetched(Dictionary<string, List<string>> effectsByPlayer)
    {
        if (effectsByPlayer != null)
        {
            cachedEffectsByPlayer = effectsByPlayer;
            if (debugEnabled) Debug.Log($"SpecialEffectsManager: Cached effects for {cachedEffectsByPlayer.Count} players");
        }
        else
        {
            cachedEffectsByPlayer = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            if (debugEnabled) Debug.Log("SpecialEffectsManager: Received null effects map from fetcher");
        }
    }
}
#endif
