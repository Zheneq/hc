using System;
using UnityEngine;

namespace Fabric
{
	[ExecuteInEditMode]
	public class FabricSpringBoard : MonoBehaviour
	{
		public string _fabricManagerPrefabPath;

		public static bool _isPresent;

		public FabricSpringBoard()
		{
			FabricSpringBoard._isPresent = true;
		}

		private void OnEnable()
		{
			FabricSpringBoard._isPresent = true;
		}

		private void Awake()
		{
			this.Load();
		}

		public void Load()
		{
			FabricManager fabricManagerInEditor = FabricSpringBoard.GetFabricManagerInEditor();
			if (!fabricManagerInEditor)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FabricSpringBoard.Load()).MethodHandle;
				}
				GameObject gameObject = Resources.Load(this._fabricManagerPrefabPath, typeof(GameObject)) as GameObject;
				if (gameObject)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					UnityEngine.Object.Instantiate<GameObject>(gameObject);
				}
			}
		}

		public static FabricManager GetFabricManagerInEditor()
		{
			FabricManager[] array = Resources.FindObjectsOfTypeAll(typeof(FabricManager)) as FabricManager[];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].gameObject != null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(FabricSpringBoard.GetFabricManagerInEditor()).MethodHandle;
					}
					if (array[i].hideFlags != HideFlags.HideInHierarchy)
					{
						return array[i];
					}
				}
			}
			return null;
		}
	}
}
