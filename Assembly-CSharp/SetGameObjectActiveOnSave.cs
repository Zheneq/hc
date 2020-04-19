using System;
using UnityEngine;

public class SetGameObjectActiveOnSave : MonoBehaviour
{
	public SetGameObjectActiveOnSave.GameObjectActiveState m_StateToSetGameObject;

	public enum GameObjectActiveState
	{
		None,
		Active,
		Inactive
	}
}
