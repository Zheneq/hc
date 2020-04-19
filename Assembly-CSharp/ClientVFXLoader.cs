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

	internal static ClientVFXLoader Get()
	{
		return ClientVFXLoader.s_instance;
	}

	private void Awake()
	{
		ClientVFXLoader.s_instance = this;
	}

	private void OnDestroy()
	{
		ClientVFXLoader.s_instance = null;
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameObjectsDestroyed);
	}

	private void Start()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameObjectsDestroyed);
	}

	internal bool IsPreloadQueueEmpty()
	{
		return this.m_nextIndexToPreload >= this.m_preloadedPKFXPaths.Count;
	}

	public float Progress
	{
		get
		{
			float result = 0f;
			if (this.m_preloadedPKFXPaths.Count > 0)
			{
				result = Mathf.Clamp((float)this.m_nextIndexToPreload / (float)this.m_preloadedPKFXPaths.Count, 0f, 1f);
			}
			return result;
		}
	}

	public bool IsPreloadInProgress()
	{
		return this.m_preloadingCoroutine != null;
	}

	private void HandleOnGameStarted()
	{
		if (!this.IsPreloadQueueEmpty())
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientVFXLoader.HandleOnGameStarted()).MethodHandle;
			}
			Log.Error("PKFX preloading did not finish before starting game", new object[0]);
		}
	}

	internal void PreloadQueuedPKFX()
	{
		if (this.m_preloadingCoroutine == null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientVFXLoader.PreloadQueuedPKFX()).MethodHandle;
			}
			if (this.m_preloadedPKFXPaths.Count > 0)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_preloadingCoroutine = this.PreloadOnePKFXPerFrame();
				base.StartCoroutine(this.m_preloadingCoroutine);
			}
		}
	}

	internal void QueuePKFXDirectoryForPreload(string pathRelativeToStreamingAssets)
	{
		if (!string.IsNullOrEmpty(pathRelativeToStreamingAssets))
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientVFXLoader.QueuePKFXDirectoryForPreload(string)).MethodHandle;
			}
			try
			{
				pathRelativeToStreamingAssets = pathRelativeToStreamingAssets.Replace("\\\\", "/");
				pathRelativeToStreamingAssets = pathRelativeToStreamingAssets.Replace("\\", "/");
				foreach (string text in Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, pathRelativeToStreamingAssets)))
				{
					if (!string.IsNullOrEmpty(text))
					{
						if (text.EndsWith(".pkfx", StringComparison.OrdinalIgnoreCase))
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							int num = text.IndexOf("packfx", StringComparison.OrdinalIgnoreCase);
							if (num >= 0)
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								string text2 = text.Substring(num + "packfx".Length);
								if (!string.IsNullOrEmpty(text2))
								{
									while (text2[0] == Path.DirectorySeparatorChar || text2[0] == Path.AltDirectorySeparatorChar)
									{
										text2 = text2.Substring(1);
									}
									for (;;)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									text2 = text2.Replace("\\\\", "/");
									text2 = text2.Replace("\\", "/");
									this.m_preloadedPKFXPaths.Add(text2);
								}
								else
								{
									Log.Error("Check character resource link pkfx directory for invalid directory for pkfx {0}", new object[]
									{
										text
									});
								}
							}
						}
					}
					else
					{
						Log.Error("Invalid empty or null string", new object[0]);
					}
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
			}
		}
		else
		{
			string message = "Invalid PKFX path {0} does not contain the PackFx directory.";
			object[] array = new object[1];
			int num2 = 0;
			object obj;
			if (pathRelativeToStreamingAssets == null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				obj = "NULL";
			}
			else
			{
				obj = pathRelativeToStreamingAssets;
			}
			array[num2] = obj;
			Log.Error(message, array);
		}
	}

	private IEnumerator PreloadOnePKFXPerFrame()
	{
		while (!this.IsPreloadQueueEmpty())
		{
			string path = this.m_preloadedPKFXPaths.ElementAt(this.m_nextIndexToPreload);
			PKFxManager.PreLoadFxIFN(path);
			PKFxManager.m_preloadedPKFXPaths.Add(path);
			yield return null;
			this.m_nextIndexToPreload++;
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(ClientVFXLoader.<PreloadOnePKFXPerFrame>c__Iterator0.MoveNext()).MethodHandle;
		}
		this.m_preloadingCoroutine = null;
		this.m_preloadedPKFXPaths.Clear();
		this.m_nextIndexToPreload = 0;
		yield break;
	}

	private void StopPreloadingCoroutine()
	{
		if (this.m_preloadingCoroutine != null)
		{
			base.StopCoroutine(this.m_preloadingCoroutine);
			this.m_preloadingCoroutine = null;
		}
	}

	private void UnloadAll()
	{
		this.StopPreloadingCoroutine();
		PKFxManager.UnloadAll();
		this.m_preloadedPKFXPaths.Clear();
		this.m_nextIndexToPreload = 0;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		this.UnloadAll();
	}
}
