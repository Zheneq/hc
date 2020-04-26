using UnityEngine;

public class SetGameObjectActiveOnSave : MonoBehaviour
{
	public enum GameObjectActiveState
	{
		None,
		Active,
		Inactive
	}

	public GameObjectActiveState m_StateToSetGameObject;
}
