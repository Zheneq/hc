using System;
using UnityEngine;

public class PKFxSceneMeshBuilder : MonoBehaviour
{
	[Tooltip("Output path for the scene mesh, relative to the PackFX directory")]
	public string m_OutputPkmmPath = "Meshes/UnityScene.pkmm";

	[Tooltip("List of the GameObjects to be searched for potential meshes.")]
	public GameObject[] m_GameObjectsToSearch;

	[HideInInspector]
	public GameObject[] m_MeshGameObjects;
}
