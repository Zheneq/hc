using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	public class ResourceManager : MonoBehaviour
	{
		private static ResourceManager mInstance;

		public UnityEngine.Object[] Assets;

		private Dictionary<string, UnityEngine.Object> mResourcesCache = new Dictionary<string, UnityEngine.Object>();

		private bool mCleaningScheduled;

		public static ResourceManager pInstance
		{
			get
			{
				bool flag = mInstance == null;
				if (mInstance == null)
				{
					mInstance = (ResourceManager)UnityEngine.Object.FindObjectOfType(typeof(ResourceManager));
				}
				if (mInstance == null)
				{
					GameObject gameObject = new GameObject("I2ResourceManager", typeof(ResourceManager));
					gameObject.hideFlags |= HideFlags.HideAndDontSave;
					mInstance = gameObject.GetComponent<ResourceManager>();
				}
				if (flag && Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(mInstance.gameObject);
				}
				return mInstance;
			}
		}

		private void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			LocalizationManager.UpdateSources();
		}

		public T GetAsset<T>(string Name) where T : UnityEngine.Object
		{
			T val = FindAsset(Name) as T;
			if ((UnityEngine.Object)val != (UnityEngine.Object)null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return val;
					}
				}
			}
			return LoadFromResources<T>(Name);
		}

		private UnityEngine.Object FindAsset(string Name)
		{
			if (Assets != null)
			{
				int i = 0;
				for (int num = Assets.Length; i < num; i++)
				{
					if (!(Assets[i] != null) || !(Assets[i].name == Name))
					{
						continue;
					}
					while (true)
					{
						return Assets[i];
					}
				}
			}
			return null;
		}

		public bool HasAsset(UnityEngine.Object Obj)
		{
			if (Assets == null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			return Array.IndexOf(Assets, Obj) >= 0;
		}

		public T LoadFromResources<T>(string Path) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(Path))
			{
				return (T)null;
			}
			if (mResourcesCache.TryGetValue(Path, out UnityEngine.Object value))
			{
				if (value != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return value as T;
						}
					}
				}
			}
			T val = (T)null;
			if (Path.EndsWith("]", StringComparison.OrdinalIgnoreCase))
			{
				int num = Path.LastIndexOf("[", StringComparison.OrdinalIgnoreCase);
				int length = Path.Length - num - 2;
				string value2 = Path.Substring(num + 1, length);
				Path = Path.Substring(0, num);
				T[] array = Resources.LoadAll<T>(Path);
				int num2 = 0;
				int num3 = array.Length;
				while (true)
				{
					if (num2 < num3)
					{
						if (array[num2].name.Equals(value2))
						{
							val = array[num2];
							break;
						}
						num2++;
						continue;
					}
					break;
				}
			}
			else
			{
				val = Resources.Load<T>(Path);
			}
			if ((UnityEngine.Object)val == (UnityEngine.Object)null)
			{
				val = Resources.Load<T>("Localization/" + Path);
			}
			mResourcesCache[Path] = val;
			if (!mCleaningScheduled)
			{
				Invoke("CleanResourceCache", 0.1f);
				mCleaningScheduled = true;
			}
			return val;
		}

		public void CleanResourceCache()
		{
			mResourcesCache.Clear();
			if (ClientQualityComponentEnabler.OptimizeForMemory())
			{
				Resources.UnloadUnusedAssets();
			}
			CancelInvoke();
			mCleaningScheduled = false;
		}
	}
}
