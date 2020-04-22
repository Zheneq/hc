using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetBundleManager : MonoBehaviour
{
	public class LoadAssetBundleAsyncOperation
	{
		public AssetBundleCreateRequest request;

		public int referenceCount;

		public bool isCanceled;

		public float loadStartTimestamp;

		public bool isDone
		{
			get
			{
				int result;
				if (request != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					result = (request.isDone ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			}
		}
	}

	public class LoadSceneAsyncOperation
	{
		public string sceneName;

		public string bundleName;

		public LoadSceneMode loadSceneMode;

		public LoadAssetBundleAsyncOperation assetBundleOperation;

		public AsyncOperation sceneOperation;

		public bool isCanceled;

		public float loadStartTimestamp;

		public string name => $"{bundleName}.{sceneName}";

		public bool isDone
		{
			get
			{
				int result;
				if (sceneOperation != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					result = (sceneOperation.isDone ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			}
		}

		public float progress
		{
			get
			{
				float num = 0f;
				if (assetBundleOperation != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					num += assetBundleOperation.request.progress / 2f;
				}
				if (sceneOperation != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					num += sceneOperation.progress / 2f;
				}
				return num;
			}
		}
	}

	private static AssetBundleManager s_instance;

	private Dictionary<string, LoadSceneAsyncOperation> m_postedLoadSceneAsyncOperations = new Dictionary<string, LoadSceneAsyncOperation>();

	private Dictionary<string, LoadAssetBundleAsyncOperation> m_postedLoadAssetBundleAsyncOperations = new Dictionary<string, LoadAssetBundleAsyncOperation>();

	public static AssetBundleManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	public List<string> GetScenesInBundle(string bundleName)
	{
		List<string> list = new List<string>();
		string dataPath = Application.dataPath;
		if (!Application.isEditor)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			dataPath = dataPath + "/Bundles/scenes/" + bundleName + ".json";
		}
		else
		{
			dataPath = dataPath + "/../editor_" + bundleName + ".json";
		}
		if (!File.Exists(dataPath))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
		string value = File.ReadAllText(dataPath);
		JsonConvert.PopulateObject(value, list);
		return list;
	}

	public bool SceneExistsInBundle(string bundleName, string sceneName)
	{
		List<string> scenesInBundle = GetScenesInBundle(bundleName);
		if (_003C_003Ef__am_0024cache0 == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			_003C_003Ef__am_0024cache0 = ((string s) => s.ToLower());
		}
		return scenesInBundle.Select(_003C_003Ef__am_0024cache0).Contains(sceneName.ToLower());
	}

	public bool SceneAssetBundleExists(string bundleName)
	{
		return File.Exists(GetSceneAssetBundlePath(bundleName));
	}

	public string GetSceneAssetBundlePath(string bundleName)
	{
		if (!Application.isEditor)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return Application.dataPath + "/Bundles/scenes/" + bundleName + ".bundle";
				}
			}
		}
		return Application.dataPath + "/../Bundles/scenes/" + bundleName + ".bundle";
	}

	public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
	{
		return LoadSceneAsync(sceneName, null, loadSceneMode);
	}

	public IEnumerator LoadSceneAsync(string sceneName, string bundleName, LoadSceneMode loadSceneMode)
	{
		LoadSceneAsyncOperation loadSceneAsyncOperation = new LoadSceneAsyncOperation();
		loadSceneAsyncOperation.sceneName = sceneName;
		loadSceneAsyncOperation.bundleName = bundleName;
		loadSceneAsyncOperation.loadSceneMode = loadSceneMode;
		LoadSceneAsyncOperation operation = loadSceneAsyncOperation;
		return LoadSceneAsync(operation);
	}

	public IEnumerator LoadSceneAsync(LoadSceneAsyncOperation operation)
	{
		yield return LoadAssetBundleInternal(operation);
		yield return LoadSceneInternal(operation);
		/*Error: Unable to find new state assignment for yield return*/;
	}

	private IEnumerator LoadAssetBundleInternal(LoadSceneAsyncOperation operation)
	{
		if (Application.isEditor)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					yield break;
				}
			}
		}
		if (operation.bundleName.IsNullOrEmpty())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			operation.bundleName = operation.sceneName;
		}
		string bundlePath = GetSceneAssetBundlePath(operation.bundleName);
		LoadAssetBundleAsyncOperation postedAssetBundleAsyncOperation = m_postedLoadAssetBundleAsyncOperations.TryGetValue(operation.bundleName);
		if (postedAssetBundleAsyncOperation != null)
		{
			operation.assetBundleOperation = postedAssetBundleAsyncOperation;
			operation.assetBundleOperation.referenceCount++;
			operation.assetBundleOperation.isCanceled = false;
			if (!operation.assetBundleOperation.request.isDone)
			{
				yield return new WaitWhile(() => !operation.assetBundleOperation.request.isDone);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		else
		{
			operation.assetBundleOperation = new LoadAssetBundleAsyncOperation();
			operation.assetBundleOperation.request = AssetBundle.LoadFromFileAsync(bundlePath);
			operation.assetBundleOperation.referenceCount++;
			operation.assetBundleOperation.isCanceled = false;
			operation.assetBundleOperation.loadStartTimestamp = Time.realtimeSinceStartup;
			m_postedLoadAssetBundleAsyncOperations.Add(operation.bundleName, operation.assetBundleOperation);
			yield return operation.assetBundleOperation.request;
		}
		if (operation.assetBundleOperation.isCanceled)
		{
			Log.Info(Log.Category.Loading, "AssetBundle | <- Canceled loading scene asset bundle {0} ({1}%, isDone {2})", operation.name, operation.assetBundleOperation.request.progress * 100f, operation.assetBundleOperation.request.isDone);
			if (operation.assetBundleOperation.request.assetBundle != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				operation.assetBundleOperation.request.assetBundle.Unload(false);
			}
			m_postedLoadAssetBundleAsyncOperations.Remove(operation.bundleName);
			yield break;
		}
		if (operation.assetBundleOperation.request.assetBundle == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					if (FileSystemUtils.TryRead(bundlePath, out string errorMessage))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						errorMessage = "AsyncOperation error";
					}
					throw new Exception($"AssetBundle | <- Failed to load scene asset bundle {operation.name} ({errorMessage})");
				}
				}
			}
		}
		_ = Time.realtimeSinceStartup - operation.assetBundleOperation.loadStartTimestamp;
	}

	private IEnumerator LoadSceneInternal(LoadSceneAsyncOperation operation)
	{
		LoadSceneAsyncOperation operation2 = operation;
		if (HitchDetector.Get() != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			HitchDetector.Get().RecordFrameTimeForHitch("Loading scene " + operation2.name);
		}
		LoadSceneAsyncOperation postedLoadSceneAsyncOperation = m_postedLoadSceneAsyncOperations.TryGetValue(operation2.sceneName);
		if (postedLoadSceneAsyncOperation != null)
		{
			operation2 = postedLoadSceneAsyncOperation;
			operation2.isCanceled = false;
			if (operation2.sceneOperation.isDone)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			else
			{
				yield return new WaitWhile(() => !operation2.sceneOperation.isDone);
				m_postedLoadSceneAsyncOperations.Remove(operation2.sceneName);
			}
		}
		else
		{
			operation2.sceneOperation = SceneManager.LoadSceneAsync(operation2.sceneName, operation2.loadSceneMode);
			operation2.isCanceled = false;
			operation2.loadStartTimestamp = Time.realtimeSinceStartup;
			m_postedLoadSceneAsyncOperations.Add(operation2.sceneName, operation2);
			yield return operation2.sceneOperation;
		}
		if (operation2.isCanceled)
		{
			Log.Info(Log.Category.Loading, "AssetBundle | <- Canceled loading scene {0} ({1}%, isDone {2})", operation2.name, operation2.sceneOperation.progress * 100f, operation2.sceneOperation.isDone);
			SceneManager.UnloadSceneAsync(operation2.sceneName);
		}
		else
		{
			_ = Time.realtimeSinceStartup - operation2.loadStartTimestamp;
		}
		m_postedLoadSceneAsyncOperations.Remove(operation2.sceneName);
		if (!(HitchDetector.Get() != null))
		{
			yield break;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			HitchDetector.Get().RecordFrameTimeForHitch("Loaded scene " + operation2.name);
			yield break;
		}
	}

	public void UnloadScene(string sceneName, string bundleName = null)
	{
		if (bundleName.IsNullOrEmpty())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			bundleName = sceneName;
		}
		UnloadAssetBundleInternal(bundleName);
		UnloadSceneInternal(sceneName);
	}

	private void UnloadAssetBundleInternal(string bundleName)
	{
		if (Application.isEditor)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		LoadAssetBundleAsyncOperation loadAssetBundleAsyncOperation = m_postedLoadAssetBundleAsyncOperations.TryGetValue(bundleName);
		if (loadAssetBundleAsyncOperation == null)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (--loadAssetBundleAsyncOperation.referenceCount != 0)
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (!loadAssetBundleAsyncOperation.request.isDone)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							Log.Info(Log.Category.Loading, "AssetBundle | Cancel loading asset bundle {0} ({1}%, isDone {2}) ...", bundleName, loadAssetBundleAsyncOperation.request.progress, loadAssetBundleAsyncOperation.request.isDone);
							loadAssetBundleAsyncOperation.isCanceled = true;
							return;
						}
					}
				}
				if (loadAssetBundleAsyncOperation.request.assetBundle != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					loadAssetBundleAsyncOperation.request.assetBundle.Unload(false);
				}
				m_postedLoadAssetBundleAsyncOperations.Remove(bundleName);
				return;
			}
		}
	}

	private void UnloadSceneInternal(string sceneName)
	{
		LoadSceneAsyncOperation loadSceneAsyncOperation = m_postedLoadSceneAsyncOperations.TryGetValue(sceneName);
		if (loadSceneAsyncOperation != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!loadSceneAsyncOperation.isDone)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						Log.Info(Log.Category.Loading, "AssetBundle | Cancel loading scene {0} ({1}%, isDone {2}) ...", sceneName, loadSceneAsyncOperation.sceneOperation.progress, loadSceneAsyncOperation.sceneOperation.isDone);
						loadSceneAsyncOperation.isCanceled = true;
						return;
					}
				}
			}
		}
		try
		{
			if (SceneManager.GetSceneByName(sceneName).IsValid())
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						SceneManager.UnloadSceneAsync(sceneName);
						goto end_IL_007e;
					}
				}
			}
			end_IL_007e:;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		m_postedLoadSceneAsyncOperations.Remove(sceneName);
	}
}
