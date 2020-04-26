using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetBundleManager : MonoBehaviour
{
	private static AssetBundleManager s_instance;

	private Dictionary<string, AssetBundleManager.LoadSceneAsyncOperation> m_postedLoadSceneAsyncOperations = new Dictionary<string, AssetBundleManager.LoadSceneAsyncOperation>();

	private Dictionary<string, AssetBundleManager.LoadAssetBundleAsyncOperation> m_postedLoadAssetBundleAsyncOperations = new Dictionary<string, AssetBundleManager.LoadAssetBundleAsyncOperation>();

	public static AssetBundleManager Get()
	{
		return AssetBundleManager.s_instance;
	}

	private void Awake()
	{
		AssetBundleManager.s_instance = this;
	}

	public List<string> GetScenesInBundle(string bundleName)
	{
		List<string> list = new List<string>();
		string text = Application.dataPath;
		if (!Application.isEditor)
		{
			text = text + "/Bundles/scenes/" + bundleName + ".json";
		}
		else
		{
			text = text + "/../editor_" + bundleName + ".json";
		}
		if (!File.Exists(text))
		{
			return list;
		}
		string value = File.ReadAllText(text);
		JsonConvert.PopulateObject(value, list);
		return list;
	}

	public bool SceneExistsInBundle(string bundleName, string sceneName)
	{
		IEnumerable<string> scenesInBundle = this.GetScenesInBundle(bundleName);
		
		return scenesInBundle.Select(((string s) => s.ToLower())).Contains(sceneName.ToLower());
	}

	public bool SceneAssetBundleExists(string bundleName)
	{
		return File.Exists(this.GetSceneAssetBundlePath(bundleName));
	}

	public string GetSceneAssetBundlePath(string bundleName)
	{
		if (!Application.isEditor)
		{
			return Application.dataPath + "/Bundles/scenes/" + bundleName + ".bundle";
		}
		return Application.dataPath + "/../Bundles/scenes/" + bundleName + ".bundle";
	}

	public IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode)
	{
		return this.LoadSceneAsync(sceneName, null, loadSceneMode);
	}

	public IEnumerator LoadSceneAsync(string sceneName, string bundleName, LoadSceneMode loadSceneMode)
	{
		AssetBundleManager.LoadSceneAsyncOperation operation = new AssetBundleManager.LoadSceneAsyncOperation
		{
			sceneName = sceneName,
			bundleName = bundleName,
			loadSceneMode = loadSceneMode
		};
		return this.LoadSceneAsync(operation);
	}

	public IEnumerator LoadSceneAsync(AssetBundleManager.LoadSceneAsyncOperation operation)
	{
		yield return this.LoadAssetBundleInternal(operation);
		yield return this.LoadSceneInternal(operation);
		yield break;
	}

	private IEnumerator LoadAssetBundleInternal(AssetBundleManager.LoadSceneAsyncOperation operation)
	{
		if (Application.isEditor)
		{
			yield break;
		}
		if (operation.bundleName.IsNullOrEmpty())
		{
			operation.bundleName = operation.sceneName;
		}
		string bundlePath = this.GetSceneAssetBundlePath(operation.bundleName);
		AssetBundleManager.LoadAssetBundleAsyncOperation postedAssetBundleAsyncOperation = this.m_postedLoadAssetBundleAsyncOperations.TryGetValue(operation.bundleName);
		if (postedAssetBundleAsyncOperation != null)
		{
			operation.assetBundleOperation = postedAssetBundleAsyncOperation;
			operation.assetBundleOperation.referenceCount++;
			operation.assetBundleOperation.isCanceled = false;
			if (operation.assetBundleOperation.request.isDone)
			{
			}
			else
			{
				yield return new WaitWhile(() => !operation.assetBundleOperation.request.isDone);
			}
		}
		else
		{
			operation.assetBundleOperation = new AssetBundleManager.LoadAssetBundleAsyncOperation();
			operation.assetBundleOperation.request = AssetBundle.LoadFromFileAsync(bundlePath);
			operation.assetBundleOperation.referenceCount++;
			operation.assetBundleOperation.isCanceled = false;
			operation.assetBundleOperation.loadStartTimestamp = Time.realtimeSinceStartup;
			this.m_postedLoadAssetBundleAsyncOperations.Add(operation.bundleName, operation.assetBundleOperation);
			yield return operation.assetBundleOperation.request;
		}
		if (operation.assetBundleOperation.isCanceled)
		{
			Log.Info(Log.Category.Loading, "AssetBundle | <- Canceled loading scene asset bundle {0} ({1}%, isDone {2})", new object[]
			{
				operation.name,
				operation.assetBundleOperation.request.progress * 100f,
				operation.assetBundleOperation.request.isDone
			});
			if (operation.assetBundleOperation.request.assetBundle != null)
			{
				operation.assetBundleOperation.request.assetBundle.Unload(false);
			}
			this.m_postedLoadAssetBundleAsyncOperations.Remove(operation.bundleName);
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
				throw new Exception(string.Format("AssetBundle | <- Failed to load scene asset bundle {0} ({1})", operation.name, arg));
			}
			float num = Time.realtimeSinceStartup - operation.assetBundleOperation.loadStartTimestamp;
		}
		yield break;
	}

	private IEnumerator LoadSceneInternal(AssetBundleManager.LoadSceneAsyncOperation operation)
	{
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().RecordFrameTimeForHitch("Loading scene " + operation.name);
		}
		AssetBundleManager.LoadSceneAsyncOperation postedLoadSceneAsyncOperation = this.m_postedLoadSceneAsyncOperations.TryGetValue(operation.sceneName);
		if (postedLoadSceneAsyncOperation != null)
		{
			operation = postedLoadSceneAsyncOperation;
			operation.isCanceled = false;
			if (operation.sceneOperation.isDone)
			{
			}
			else
			{
				yield return new WaitWhile(() => !operation.sceneOperation.isDone);
				this.m_postedLoadSceneAsyncOperations.Remove(operation.sceneName);
			}
		}
		else
		{
			operation.sceneOperation = SceneManager.LoadSceneAsync(operation.sceneName, operation.loadSceneMode);
			operation.isCanceled = false;
			operation.loadStartTimestamp = Time.realtimeSinceStartup;
			this.m_postedLoadSceneAsyncOperations.Add(operation.sceneName, operation);
			yield return operation.sceneOperation;
		}
		if (operation.isCanceled)
		{
			Log.Info(Log.Category.Loading, "AssetBundle | <- Canceled loading scene {0} ({1}%, isDone {2})", new object[]
			{
				operation.name,
				operation.sceneOperation.progress * 100f,
				operation.sceneOperation.isDone
			});
			SceneManager.UnloadSceneAsync(operation.sceneName);
		}
		else
		{
			float num = Time.realtimeSinceStartup - operation.loadStartTimestamp;
		}
		this.m_postedLoadSceneAsyncOperations.Remove(operation.sceneName);
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().RecordFrameTimeForHitch("Loaded scene " + operation.name);
		}
		yield break;
	}

	public void UnloadScene(string sceneName, string bundleName = null)
	{
		if (bundleName.IsNullOrEmpty())
		{
			bundleName = sceneName;
		}
		this.UnloadAssetBundleInternal(bundleName);
		this.UnloadSceneInternal(sceneName);
	}

	private void UnloadAssetBundleInternal(string bundleName)
	{
		if (Application.isEditor)
		{
			return;
		}
		AssetBundleManager.LoadAssetBundleAsyncOperation loadAssetBundleAsyncOperation = this.m_postedLoadAssetBundleAsyncOperations.TryGetValue(bundleName);
		if (loadAssetBundleAsyncOperation != null)
		{
			if (--loadAssetBundleAsyncOperation.referenceCount == 0)
			{
				if (!loadAssetBundleAsyncOperation.request.isDone)
				{
					Log.Info(Log.Category.Loading, "AssetBundle | Cancel loading asset bundle {0} ({1}%, isDone {2}) ...", new object[]
					{
						bundleName,
						loadAssetBundleAsyncOperation.request.progress,
						loadAssetBundleAsyncOperation.request.isDone
					});
					loadAssetBundleAsyncOperation.isCanceled = true;
				}
				else
				{
					if (loadAssetBundleAsyncOperation.request.assetBundle != null)
					{
						loadAssetBundleAsyncOperation.request.assetBundle.Unload(false);
					}
					this.m_postedLoadAssetBundleAsyncOperations.Remove(bundleName);
				}
			}
		}
	}

	private void UnloadSceneInternal(string sceneName)
	{
		AssetBundleManager.LoadSceneAsyncOperation loadSceneAsyncOperation = this.m_postedLoadSceneAsyncOperations.TryGetValue(sceneName);
		if (loadSceneAsyncOperation != null)
		{
			if (!loadSceneAsyncOperation.isDone)
			{
				Log.Info(Log.Category.Loading, "AssetBundle | Cancel loading scene {0} ({1}%, isDone {2}) ...", new object[]
				{
					sceneName,
					loadSceneAsyncOperation.sceneOperation.progress,
					loadSceneAsyncOperation.sceneOperation.isDone
				});
				loadSceneAsyncOperation.isCanceled = true;
				return;
			}
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
		this.m_postedLoadSceneAsyncOperations.Remove(sceneName);
	}

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
				bool result;
				if (this.request != null)
				{
					result = this.request.isDone;
				}
				else
				{
					result = false;
				}
				return result;
			}
		}
	}

	public class LoadSceneAsyncOperation
	{
		public string sceneName;

		public string bundleName;

		public LoadSceneMode loadSceneMode;

		public AssetBundleManager.LoadAssetBundleAsyncOperation assetBundleOperation;

		public AsyncOperation sceneOperation;

		public bool isCanceled;

		public float loadStartTimestamp;

		public string name
		{
			get
			{
				return string.Format("{0}.{1}", this.bundleName, this.sceneName);
			}
		}

		public bool isDone
		{
			get
			{
				bool result;
				if (this.sceneOperation != null)
				{
					result = this.sceneOperation.isDone;
				}
				else
				{
					result = false;
				}
				return result;
			}
		}

		public float progress
		{
			get
			{
				float num = 0f;
				if (this.assetBundleOperation != null)
				{
					num += this.assetBundleOperation.request.progress / 2f;
				}
				if (this.sceneOperation != null)
				{
					num += this.sceneOperation.progress / 2f;
				}
				return num;
			}
		}
	}
}
