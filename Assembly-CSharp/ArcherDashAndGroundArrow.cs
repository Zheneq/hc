using UnityEngine;

public class ArcherDashAndGroundArrow : Ability
{
	[Header("-- Targeting")]
	public float m_groundArrowMaxRange = 6f;
	public float m_groundArrowMinRange = 1f;
	public bool m_groundArrowPenetratesLoS;
	public float m_laserWidth = 1f;
	public float m_laserRange = 5.5f;
	[Header("-- Oil Slick")]
	public StandardGroundEffectInfo m_groundEffect;
	public StandardBarrierData m_stopMovementBarrierData;
	[Header("-- Sequences")]
	public GameObject m_dashSequencePrefab;
	public GameObject m_arrowProjectileSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ArcherDashAndGroundArrow";
		}
		Setup();
	}

	private void Setup()
	{
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Flight;
	}
}
