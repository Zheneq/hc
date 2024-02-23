using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetBundleManager : MonoBehaviour
{
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
		string path = Application.dataPath;
		path += Application.isEditor
			? new StringBuilder().Append("/../editor_").Append(bundleName).Append(".json").ToString()
			: new StringBuilder().Append("/Bundles/scenes/").Append(bundleName).Append(".json").ToString();
		if (!File.Exists(path))
		{
			return list;
		}

		JsonConvert.PopulateObject(File.ReadAllText(path), list);
		
		return list;
	}

	public bool SceneExistsInBundle(string bundleName, string sceneName)
	{
		IEnumerable<string> scenesInBundle = GetScenesInBundle(bundleName);
		return scenesInBundle.Select(s => s.ToLower()).Contains(sceneName.ToLower());
	}

	public bool SceneAssetBundleExists(string bundleName)
	{
		return File.Exists(GetSceneAssetBundlePath(bundleName));
	}

	public string GetSceneAssetBundlePath(string bundleName)
	{
		return Application.isEditor
			? new StringBuilder().Append(Application.dataPath).Append("/../Bundles/scenes/").Append(bundleName).Append(".bundle").ToString()
			: new StringBuilder().Append(Application.dataPath).Append("/Bundles/scenes/").Append(bundleName).Append(".bundle").ToString();
	}

	public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
	{
		return LoadSceneAsync(sceneName, null, loadSceneMode);
	}

	public IEnumerator LoadSceneAsync(string sceneName, string bundleName, LoadSceneMode loadSceneMode)
	{
		return LoadSceneAsync(new LoadSceneAsyncOperation
		{
			sceneName = sceneName,
			bundleName = bundleName,
			loadSceneMode = loadSceneMode
		});
	}

	public IEnumerator LoadSceneAsync(LoadSceneAsyncOperation operation)
	{
		yield return LoadAssetBundleInternal(operation);
		yield return LoadSceneInternal(operation);
	}

	private IEnumerator LoadAssetBundleInternal(LoadSceneAsyncOperation operation)
	{
		if (Application.isEditor)
		{
			yield break;
		}
		if (operation.bundleName.IsNullOrEmpty())
		{
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
			Log.Info(Log.Category.Loading, "AssetBundle | <- Canceled loading scene asset bundle {0} ({1}%, isDone {2})",
				operation.name, operation.assetBundleOperation.request.progress * 100f, operation.assetBundleOperation.request.isDone);
			if (operation.assetBundleOperation.request.assetBundle != null)
			{
				operation.assetBundleOperation.request.assetBundle.Unload(false);
			}
			m_postedLoadAssetBundleAsyncOperations.Remove(operation.bundleName);
		}
		else
		{
			if (operation.assetBundleOperation.request.assetBundle == null)
			{
				string arg;
				if (FileSystemUtils.TryRead(bundlePath, out arg))
				{
					arg = "AsyncOperation error";
				}
				throw new Exception(new StringBuilder().Append("AssetBundle | <- Failed to load scene asset bundle ").Append(operation.name).Append(" (").Append(arg).Append(")").ToString());
			}
			float num = Time.realtimeSinceStartup - operation.assetBundleOperation.loadStartTimestamp;
		}
	}

	private IEnumerator LoadSceneInternal(LoadSceneAsyncOperation operation)
	{
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().RecordFrameTimeForHitch(new StringBuilder().Append("Loading scene ").Append(operation.name).ToString());
		}
		LoadSceneAsyncOperation postedLoadSceneAsyncOperation = m_postedLoadSceneAsyncOperations.TryGetValue(operation.sceneName);
		if (postedLoadSceneAsyncOperation != null)
		{
			operation = postedLoadSceneAsyncOperation;
			operation.isCanceled = false;
			if (!operation.sceneOperation.isDone)
			{
				yield return new WaitWhile(() => !operation.sceneOperation.isDone);
				m_postedLoadSceneAsyncOperations.Remove(operation.sceneName);
			}
		}
		else
		{
			operation.sceneOperation = SceneManager.LoadSceneAsync(operation.sceneName, operation.loadSceneMode);
			operation.isCanceled = false;
			operation.loadStartTimestamp = Time.realtimeSinceStartup;
			m_postedLoadSceneAsyncOperations.Add(operation.sceneName, operation);
			yield return operation.sceneOperation;
		}
		if (operation.isCanceled)
		{
			Log.Info(Log.Category.Loading, "AssetBundle | <- Canceled loading scene {0} ({1}%, isDone {2})",
				operation.name, operation.sceneOperation.progress * 100f, operation.sceneOperation.isDone);
			SceneManager.UnloadSceneAsync(operation.sceneName);
		}
		else
		{
			float num = Time.realtimeSinceStartup - operation.loadStartTimestamp;
		}
		m_postedLoadSceneAsyncOperations.Remove(operation.sceneName);
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().RecordFrameTimeForHitch(new StringBuilder().Append("Loaded scene ").Append(operation.name).ToString());
		}
	}

	public void UnloadScene(string sceneName, string bundleName = null)
	{
		if (bundleName.IsNullOrEmpty())
		{
			bundleName = sceneName;
		}
		UnloadAssetBundleInternal(bundleName);
		UnloadSceneInternal(sceneName);
	}

	private void UnloadAssetBundleInternal(string bundleName)
	{
		if (Application.isEditor)
		{
			return;
		}
		LoadAssetBundleAsyncOperation loadAssetBundleAsyncOperation = m_postedLoadAssetBundleAsyncOperations.TryGetValue(bundleName);
		if (loadAssetBundleAsyncOperation != null && --loadAssetBundleAsyncOperation.referenceCount == 0)
		{
			if (!loadAssetBundleAsyncOperation.request.isDone)
			{
				Log.Info(Log.Category.Loading, "AssetBundle | Cancel loading asset bundle {0} ({1}%, isDone {2}) ...",
					bundleName, loadAssetBundleAsyncOperation.request.progress, loadAssetBundleAsyncOperation.request.isDone);
				loadAssetBundleAsyncOperation.isCanceled = true;
			}
			else
			{
				if (loadAssetBundleAsyncOperation.request.assetBundle != null)
				{
					loadAssetBundleAsyncOperation.request.assetBundle.Unload(false);
				}
				m_postedLoadAssetBundleAsyncOperations.Remove(bundleName);
			}
		}
	}

	private void UnloadSceneInternal(string sceneName)
	{
		LoadSceneAsyncOperation loadSceneAsyncOperation = m_postedLoadSceneAsyncOperations.TryGetValue(sceneName);
		if (loadSceneAsyncOperation != null && !loadSceneAsyncOperation.isDone)
		{
			Log.Info(Log.Category.Loading, "AssetBundle | Cancel loading scene {0} ({1}%, isDone {2}) ...",
				sceneName, loadSceneAsyncOperation.sceneOperation.progress, loadSceneAsyncOperation.sceneOperation.isDone);
			loadSceneAsyncOperation.isCanceled = true;
			return;
		}
		try
		{
			if (SceneManager.GetSceneByName(sceneName).IsValid())
			{
				SceneManager.UnloadSceneAsync(sceneName);
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		m_postedLoadSceneAsyncOperations.Remove(sceneName);
	}

	public class LoadAssetBundleAsyncOperation
	{
		public AssetBundleCreateRequest request;
		public int referenceCount;
		public bool isCanceled;
		public float loadStartTimestamp;

		public bool isDone
		{
			get { return request != null && request.isDone; }
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

		public string name
		{
			get { return new StringBuilder().Append(bundleName).Append(".").Append(sceneName).ToString(); }
		}

		public bool isDone
		{
			get { return sceneOperation != null && sceneOperation.isDone; }
		}

		public float progress
		{
			get
			{
				float num = 0f;
				if (assetBundleOperation != null)
				{
					num += assetBundleOperation.request.progress / 2f;
				}
				if (sceneOperation != null)
				{
					num += sceneOperation.progress / 2f;
				}
				return num;
			}
		}
	}
}
