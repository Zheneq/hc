using System.Collections.Generic;

#if !VANILLA && !SERVER
// Custom titles
public class PlayerTitleManager
{
    public TitleFetcher titleFetcher;
    private Dictionary<string, string> cachedTitlesByHandle;
    private static PlayerTitleManager m_instance;

    public static PlayerTitleManager GetInstance()
    {
        if (m_instance == null)
        {
            m_instance = new PlayerTitleManager();
        }
        return m_instance;
    }

    private PlayerTitleManager()
    {
    }

    public void Init()
    {
        titleFetcher = new TitleFetcher();
        titleFetcher.TitlesFetched += OnTitlesFetched;
        CoroutineRunner.Instance.RunCoroutine(titleFetcher.FetchTitlesFromApi());
    }

    // Titles are cached request from api anyway so does not matter to me how fast this is updated. But 5 min should be enough
    public void RefreshTitles()
    {
        CoroutineRunner.Instance.RunCoroutine(titleFetcher.FetchTitlesFromApi());
    }

    private void OnTitlesFetched(Dictionary<string, string> titlesByHandle)
    {
        if (titlesByHandle != null)
        {
            cachedTitlesByHandle = titlesByHandle;
        }
    }

    public string GetTitle(string handle, string returnOnEmptyOverride = "")
    {
        string normalizedHandle = handle.Split('#')[0].Trim();
        if (cachedTitlesByHandle == null)
        {
            Log.Info("cachedTitlesByHandle is null.");
            return returnOnEmptyOverride;
        }

        if (cachedTitlesByHandle.TryGetValue(normalizedHandle, out var title))
        {
            return title;
        }

        return returnOnEmptyOverride;
    }
}
#endif