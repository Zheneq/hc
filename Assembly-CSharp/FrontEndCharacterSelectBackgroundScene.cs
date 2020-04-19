using System;
using UnityEngine;

public class FrontEndCharacterSelectBackgroundScene : UIScene
{
	public GameObject m_lootMatrixModelStage;

	public Transform m_chestContainer;

	public GameObject m_vfxSpawnIn;

	public GameObject m_vfxOpenBase;

	public GameObject m_vfxOpenCommon;

	public GameObject m_vfxOpenUncommon;

	public GameObject m_vfxOpenRare;

	public GameObject m_vfxOpenEpic;

	public GameObject m_vfxOpenLegendary;

	public GameObject m_frontendEnvironmentCameraParent;

	private static FrontEndCharacterSelectBackgroundScene s_instance;

	public static FrontEndCharacterSelectBackgroundScene Get()
	{
		return FrontEndCharacterSelectBackgroundScene.s_instance;
	}

	public override void Awake()
	{
		FrontEndCharacterSelectBackgroundScene.s_instance = this;
		base.Awake();
	}

	public override SceneType GetSceneType()
	{
		return SceneType.CharacterSelectBackground;
	}
}
