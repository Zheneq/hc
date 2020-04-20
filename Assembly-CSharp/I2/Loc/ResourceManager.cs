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
				bool flag = ResourceManager.mInstance == null;
				if (ResourceManager.mInstance == null)
				{
					ResourceManager.mInstance = (ResourceManager)UnityEngine.Object.FindObjectOfType(typeof(ResourceManager));
				}
				if (ResourceManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("I2ResourceManager", new Type[]
					{
						typeof(ResourceManager)
					});
					gameObject.hideFlags |= HideFlags.HideAndDontSave;
					ResourceManager.mInstance = gameObject.GetComponent<ResourceManager>();
				}
				if (flag && Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(ResourceManager.mInstance.gameObject);
				}
				return ResourceManager.mInstance;
			}
		}

		private void OnEnable()
		{
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			LocalizationManager.UpdateSources();
		}

		public T GetAsset<T>(string Name) where T : UnityEngine.Object
		{
			T t = this.FindAsset(Name) as T;
			if (t != null)
			{
				return t;
			}
			return this.LoadFromResources<T>(Name);
		}

		private UnityEngine.Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				int i = 0;
				int num = this.Assets.Length;
				while (i < num)
				{
					if (this.Assets[i] != null && this.Assets[i].name == Name)
					{
						return this.Assets[i];
					}
					i++;
				}
			}
			return null;
		}

		public bool HasAsset(UnityEngine.Object Obj)
		{
			if (this.Assets == null)
			{
				return false;
			}
			return Array.IndexOf<UnityEngine.Object>(this.Assets, Obj) >= 0;
		}

		public T LoadFromResources<T>(string Path) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(Path))
			{
				return (T)((object)null);
			}
			UnityEngine.Object @object;
			if (this.mResourcesCache.TryGetValue(Path, out @object))
			{
				if (@object != null)
				{
					return @object as T;
				}
			}
			T t = (T)((object)null);
			if (Path.EndsWith("]", StringComparison.OrdinalIgnoreCase))
			{
				int num = Path.LastIndexOf("[", StringComparison.OrdinalIgnoreCase);
				int length = Path.Length - num - 2;
				string value = Path.Substring(num + 1, length);
				Path = Path.Substring(0, num);
				T[] array = Resources.LoadAll<T>(Path);
				int i = 0;
				int num2 = array.Length;
				while (i < num2)
				{
					if (array[i].name.Equals(value))
					{
						t = array[i];
						goto IL_122;
					}
					i++;
				}
			}
			else
			{
				t = Resources.Load<T>(Path);
			}
			IL_122:
			if (t == null)
			{
				t = Resources.Load<T>("Localization/" + Path);
			}
			this.mResourcesCache[Path] = t;
			if (!this.mCleaningScheduled)
			{
				base.Invoke("CleanResourceCache", 0.1f);
				this.mCleaningScheduled = true;
			}
			return t;
		}

		public void CleanResourceCache()
		{
			this.mResourcesCache.Clear();
			if (ClientQualityComponentEnabler.OptimizeForMemory())
			{
				Resources.UnloadUnusedAssets();
			}
			base.CancelInvoke();
			this.mCleaningScheduled = false;
		}
	}
}
