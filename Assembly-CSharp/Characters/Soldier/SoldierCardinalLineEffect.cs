// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class SoldierCardinalLineEffect : Effect
{
	private int m_numTurns;
	private float m_widthInSquares;
	private bool m_ignoreLos;
	private Vector3 m_shapeCenter;
	private List<Vector3> m_directions;
	private int m_damage;
	private StandardEffectInfo m_enemyHitEffect;
	private GameObject m_hitSequencePrefab;

	public SoldierCardinalLineEffect(
		EffectSource parent,
		ActorData caster,
		int numTurns,
		float width,
		bool ignoreLos,
		Vector3 shapeCenter,
		List<Vector3> directions,
		int damage,
		StandardEffectInfo enemyHitEffect,
		GameObject hitSequencePrefab)
		: base(parent, null, null, caster)
	{
		m_numTurns = numTurns;
		m_widthInSquares = width;
		m_ignoreLos = ignoreLos;
		m_shapeCenter = shapeCenter;
		m_directions = directions;
		m_damage = damage;
		m_enemyHitEffect = enemyHitEffect;
		m_hitSequencePrefab = hitSequencePrefab;
		m_time.duration = m_numTurns + 1;
		HitPhase = AbilityPriority.Combat_Damage;
	}

	private bool ShouldHitThisTurn()
	{
		return m_time.age > 0;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (ShouldHitThisTurn())
		{
			GetActorToDamage(out _, out List<List<ActorData>> actorsInDirs, out List<Vector3> dirStartPosList, out List<Vector3> dirEndPosList, null);
			SequenceSource shallowCopy = SequenceSource.GetShallowCopy();
			if (AddActorAnimEntryIfHasHits(HitPhase))
			{
				shallowCopy.SetWaitForClientEnable(true);
			}
			for (int i = 0; i < actorsInDirs.Count; i++)
			{
				list.Add(
					new ServerClientUtils.SequenceStartData(
						m_hitSequencePrefab,
						dirEndPosList[i],
						actorsInDirs[i].ToArray(),
						Caster,
						shallowCopy,
						new SoldierProjectilesInLineSequence.HitAreaExtraParams
						{
							fromPos = dirStartPosList[i],
							toPos = dirEndPosList[i],
							areaWidthInSquares = m_widthInSquares,
							ignoreStartEvent = true
						}.ToArray()));
			}
		}
		return list;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (ShouldHitThisTurn())
		{
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			Dictionary<ActorData, int> actorToDamage = GetActorToDamage(
				out Dictionary<ActorData, Vector3> actorToHitOrigin, out _, out _, out List<Vector3> dirEndPosList, nonActorTargetInfo);
			foreach (ActorData actorData in actorToDamage.Keys)
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorToHitOrigin[actorData]));
				actorHitResults.SetBaseDamage(actorToDamage[actorData]);
				actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
				effectResults.StoreActorHit(actorHitResults);
			}
			PositionHitResults hitResults = new PositionHitResults(new PositionHitParameters(dirEndPosList[0]));
			effectResults.StorePositionHit(hitResults);
		}
	}

	private Dictionary<ActorData, int> GetActorToDamage(
		out Dictionary<ActorData, Vector3> actorToHitOrigin,
		out List<List<ActorData>> actorsInDirs,
		out List<Vector3> dirStartPosList,
		out List<Vector3> dirEndPosList,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		actorToHitOrigin = new Dictionary<ActorData, Vector3>();
		actorsInDirs = new List<List<ActorData>>();
		dirStartPosList = new List<Vector3>();
		dirEndPosList = new List<Vector3>();
		return SoldierCardinalLine.GetActorToDamageStatic(
			m_shapeCenter,
			m_directions,
			Caster,
			out actorToHitOrigin,
			out actorsInDirs,
			out dirStartPosList,
			out dirEndPosList,
			nonActorTargetInfo,
			m_widthInSquares,
			m_ignoreLos,
			m_damage,
			0f,
			0,
			AbilityAreaShape.SingleSquare,
			0);
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		ActorData actorAnimationActor = GetActorAnimationActor();
		return actorAnimationActor != null && !actorAnimationActor.IsDead();
	}

	public override ActorData GetActorAnimationActor()
	{
		if (Caster.IsDead())
		{
			ActorData[] hitActors = m_effectResults.HitActorsArray();
			if (hitActors.Length != 0)
			{
				return hitActors[0];
			}
		}
		return Caster;
	}

	public override List<Vector3> CalcPointsOfInterestForCamera()
	{
		if (ShouldHitThisTurn())
		{
			List<Vector3> list = new List<Vector3>();
			int maxX = Board.Get().GetMaxX();
			int maxY = Board.Get().GetMaxY();
			float squareSize = Board.Get().squareSize;
			float num = 10f * squareSize;
			foreach (Vector3 dir in m_directions)
			{
				Vector3 vector2 = 0.5f * num * dir;
				Vector3 vector3 = m_shapeCenter - vector2;
				vector3.x = Mathf.Clamp(vector3.x, 0f, maxX * squareSize);
				vector3.z = Mathf.Clamp(vector3.z, 0f, maxY * squareSize);
				Vector3 vector4 = m_shapeCenter + vector2;
				vector4.x = Mathf.Clamp(vector4.x, -1f, maxX * squareSize + 1f);
				vector4.z = Mathf.Clamp(vector4.z, -1f, maxY * squareSize + 1f);
				list.Add(vector3);
				list.Add(vector4);
			}
			return list;
		}
		return base.CalcPointsOfInterestForCamera();
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() == Caster.GetTeam())
		{
			return;
		}
		
		int maxX = Board.Get().GetMaxX();
		int maxY = Board.Get().GetMaxY();
		float num = Mathf.Max(maxX, maxY) + 10f;
		float squareSize = Board.Get().squareSize;
		float num2 = num * squareSize;
		foreach (Vector3 dir in m_directions)
		{
			Vector3 vector2 = 0.5f * num2 * dir;
			List<BoardSquare> squaresInBox = AreaEffectUtils.GetSquaresInBox(
				m_shapeCenter - vector2, m_shapeCenter + vector2, 0.5f * m_widthInSquares, m_ignoreLos, Caster);
			squaresToAvoid.UnionWith(squaresInBox);
		}
	}
}
#endif
