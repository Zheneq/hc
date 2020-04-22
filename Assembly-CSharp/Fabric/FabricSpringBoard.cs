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
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				GameObject gameObject = Resources.Load(_fabricManagerPrefabPath, typeof(GameObject)) as GameObject;
				if ((bool)gameObject)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
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
					while (true)
					{
						switch (1)
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
