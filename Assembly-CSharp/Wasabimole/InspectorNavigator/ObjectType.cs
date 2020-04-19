using System;

namespace Wasabimole.InspectorNavigator
{
	[Serializable]
	public enum ObjectType
	{
		None,
		Asset,
		Instance,
		Folder,
		Scene,
		ProjectSettings,
		TextAssets,
		InspectorBreadcrumbs,
		AssetStoreAssetInspector
	}
}
