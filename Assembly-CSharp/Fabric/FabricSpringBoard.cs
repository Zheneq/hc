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
				GameObject gameObject = Resources.Load(this._fabricManagerPrefabPath, typeof(GameObject)) as GameObject;
				if (gameObject)
				{
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
