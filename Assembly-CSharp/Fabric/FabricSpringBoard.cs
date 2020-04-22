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
			_isPresent = true;
		}

		private void OnEnable()
		{
			_isPresent = true;
		}

		private void Awake()
		{
			Load();
		}

		public void Load()
		{
			FabricManager fabricManagerInEditor = GetFabricManagerInEditor();
			if ((bool)fabricManagerInEditor)
			{
				return;
			}
			while (true)
			{
				GameObject gameObject = Resources.Load(_fabricManagerPrefabPath, typeof(GameObject)) as GameObject;
				if ((bool)gameObject)
				{
					while (true)
					{
						Object.Instantiate(gameObject);
						return;
					}
				}
				return;
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
