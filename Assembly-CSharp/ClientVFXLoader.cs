using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ClientVFXLoader : MonoBehaviour, IGameEventListener
{
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
		if (IsPreloadQueueEmpty())
		{
			return;
		}
		while (true)
		{
			Log.Error("PKFX preloading did not finish before starting game");
			return;
		}
	}

	internal void PreloadQueuedPKFX()
	{
		if (m_preloadingCoroutine != null)
		{
			return;
		}
		while (true)
		{
			if (m_preloadedPKFXPaths.Count > 0)
			{
				while (true)
				{
					m_preloadingCoroutine = PreloadOnePKFXPerFrame();
					StartCoroutine(m_preloadingCoroutine);
					return;
				}
			}
			return;
		}
	}

	internal void QueuePKFXDirectoryForPreload(string pathRelativeToStreamingAssets)
	{
		if (!string.IsNullOrEmpty(pathRelativeToStreamingAssets))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					try
					{
						pathRelativeToStreamingAssets = pathRelativeToStreamingAssets.Replace("\\\\", "/");
						pathRelativeToStreamingAssets = pathRelativeToStreamingAssets.Replace("\\", "/");
						string[] files = Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, pathRelativeToStreamingAssets));
						for (int i = 0; i < files.Length; i++)
						{
							string text = null;
							string text2 = files[i];
							if (!string.IsNullOrEmpty(text2))
							{
								if (text2.EndsWith(".pkfx", StringComparison.OrdinalIgnoreCase))
								{
									int num = text2.IndexOf("packfx", StringComparison.OrdinalIgnoreCase);
									if (num >= 0)
									{
										text = text2.Substring(num + "packfx".Length);
										if (!string.IsNullOrEmpty(text))
										{
											while (text[0] == Path.DirectorySeparatorChar || text[0] == Path.AltDirectorySeparatorChar)
											{
												text = text.Substring(1);
											}
											text = text.Replace("\\\\", "/");
											text = text.Replace("\\", "/");
											m_preloadedPKFXPaths.Add(text);
										}
										else
										{
											Log.Error("Check character resource link pkfx directory for invalid directory for pkfx {0}", text2);
										}
									}
								}
							}
							else
							{
								Log.Error("Invalid empty or null string");
							}
						}
						while (true)
						{
							switch (1)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					catch (Exception exception)
					{
						Log.Exception(exception);
					}
					return;
				}
			}
		}
		object[] array = new object[1];
		object obj;
		if (pathRelativeToStreamingAssets == null)
		{
			obj = "NULL";
		}
		else
		{
			obj = pathRelativeToStreamingAssets;
		}
		array[0] = obj;
		Log.Error("Invalid PKFX path {0} does not contain the PackFx directory.", array);
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
		while (true)
		{
			m_preloadingCoroutine = null;
			m_preloadedPKFXPaths.Clear();
			m_nextIndexToPreload = 0;
			yield break;
		}
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
