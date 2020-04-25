using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ClientVFXLoader : MonoBehaviour, IGameEventListener
{
	private const String EXTENSION = ".pkfx";
	private const String FOLDER = "packfx";

	private static ClientVFXLoader s_instance;

	private HashSet<string> m_preloadedPKFXPaths = new HashSet<string>();

	private int m_nextIndexToPreload;

	private IEnumerator m_preloadingCoroutine;

	public float Progress
	{
		get
		{
			float result = 0f;
			if (m_preloadedPKFXPaths.Count > 0)
			{
				result = Mathf.Clamp((float)m_nextIndexToPreload / (float)m_preloadedPKFXPaths.Count, 0f, 1f);
			}
			return result;
		}
	}

	internal static ClientVFXLoader Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameObjectsDestroyed);
	}

	private void Start()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameObjectsDestroyed);
	}

	internal bool IsPreloadQueueEmpty()
	{
		return m_nextIndexToPreload >= m_preloadedPKFXPaths.Count;
	}

	public bool IsPreloadInProgress()
	{
		return m_preloadingCoroutine != null;
	}

	private void HandleOnGameStarted()
	{
		if (!IsPreloadQueueEmpty())
		{
			Log.Error("PKFX preloading did not finish before starting game");
		}
	}

	internal void PreloadQueuedPKFX()
	{
		if (m_preloadingCoroutine == null && m_preloadedPKFXPaths.Count > 0)
		{
			m_preloadingCoroutine = PreloadOnePKFXPerFrame();
			StartCoroutine(m_preloadingCoroutine);
		}
	}

	internal void QueuePKFXDirectoryForPreload(string pathRelativeToStreamingAssets)
	{
		if (!string.IsNullOrEmpty(pathRelativeToStreamingAssets))
		{
			try
			{
				pathRelativeToStreamingAssets = pathRelativeToStreamingAssets.Replace("\\\\", "/");
				pathRelativeToStreamingAssets = pathRelativeToStreamingAssets.Replace("\\", "/");
				foreach (string text in Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, pathRelativeToStreamingAssets)))
				{
					if (!string.IsNullOrEmpty(text) && text.EndsWith(EXTENSION, StringComparison.OrdinalIgnoreCase))
					{
						int num = text.IndexOf(FOLDER, StringComparison.OrdinalIgnoreCase);
						if (num >= 0)
						{
							string pathRelativeToFolder = text.Substring(num + FOLDER.Length);
							if (!string.IsNullOrEmpty(pathRelativeToFolder))
							{
								while (pathRelativeToFolder[0] == Path.DirectorySeparatorChar || pathRelativeToFolder[0] == Path.AltDirectorySeparatorChar)
								{
									pathRelativeToFolder = pathRelativeToFolder.Substring(1);
								}
								pathRelativeToFolder = pathRelativeToFolder.Replace("\\\\", "/");
								pathRelativeToFolder = pathRelativeToFolder.Replace("\\", "/");
								m_preloadedPKFXPaths.Add(pathRelativeToFolder);
							}
							else
							{
								Log.Error("Check character resource link pkfx directory for invalid directory for pkfx {0}", text);
							}
						}
					}
					else
					{
						Log.Error("Invalid empty or null string");
					}
				}
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
			}
		}
		else
		{
			Log.Error("Invalid PKFX path {0} does not contain the PackFx directory.", pathRelativeToStreamingAssets ?? "NULL");
		}
	}

	private IEnumerator PreloadOnePKFXPerFrame()
	{
		while (!IsPreloadQueueEmpty())
		{
			string path = m_preloadedPKFXPaths.ElementAt(m_nextIndexToPreload);
			PKFxManager.PreLoadFxIFN(path);
			PKFxManager.m_preloadedPKFXPaths.Add(path);
			yield return null;
			m_nextIndexToPreload++;
		}
		m_preloadingCoroutine = null;
		m_preloadedPKFXPaths.Clear();
		m_nextIndexToPreload = 0;
		yield break;
	}

	private void StopPreloadingCoroutine()
	{
		if (m_preloadingCoroutine != null)
		{
			StopCoroutine(m_preloadingCoroutine);
			m_preloadingCoroutine = null;
		}
	}

	private void UnloadAll()
	{
		StopPreloadingCoroutine();
		PKFxManager.UnloadAll();
		m_preloadedPKFXPaths.Clear();
		m_nextIndexToPreload = 0;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		UnloadAll();
	}
}
