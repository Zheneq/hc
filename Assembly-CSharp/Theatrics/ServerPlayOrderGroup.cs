// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Theatrics
{
#if SERVER
	internal class ServerPlayOrderGroup : IComparable<ServerPlayOrderGroup>
	{
		private Turn m_turn;
		private int m_phaseIndex;
		private List<Caster> m_casters = new List<Caster>();

		private const float SORT_POSITION_TOLERANCE = 0.02f;

		private bool m_casterDied;
		private bool m_targetDied;

		internal Vector3 SortPosition { get; private set; }

		internal ServerPlayOrderGroup(Turn turn, int phaseIndex)
		{
			m_turn = turn;
			m_phaseIndex = phaseIndex;
		}

		internal void Add(ActorAnimation actorAnimation)
		{
			foreach (Caster caster in m_casters)
			{
				if (caster.CastingActor == actorAnimation.Caster)
				{
					if (!caster.ActorAnimations.Contains(actorAnimation))
					{
						caster.ActorAnimations.Add(actorAnimation);
					}
					return;
				}
			}
			m_casters.Add(new Caster(actorAnimation));
		}

		internal void InitSortData()
		{
			foreach (Caster caster in m_casters)
			{
				caster.InitSortData();
			}
			SortPosition = FindSortPosition();
			m_casterDied = FindIfCasterDied();
			m_targetDied = FindIfTargetDied();
		}

		internal sbyte AssignPlayOrderIndexes(sbyte startIndex)
		{
			sbyte b = startIndex;
			m_casters.Sort();
			foreach (Caster caster in m_casters)
			{
				caster.ActorAnimations.Sort();
			}
			foreach (Caster caster in m_casters)
			{
				for (int k = 0; k < caster.ActorAnimations.Count; k++)
				{
					ActorAnimation actorAnimation = caster.ActorAnimations[k];
					if (actorAnimation.m_playOrderIndex < 0)
					{
						actorAnimation.m_playOrderIndex = b++;
						actorAnimation.m_playOrderGroupIndex = startIndex;
					}
				}
			}
			return b;
		}

		internal List<ActorAnimation> GetActorAnimationsInGroup()
		{
			List<ActorAnimation> list = new List<ActorAnimation>();
			foreach (Caster caster in m_casters)
			{
				foreach (ActorAnimation anim in caster.ActorAnimations)
				{
					list.Add(anim);
				}
			}
			return list;
		}

		internal bool ShouldSortByPositionOnly()
		{
			return !m_casterDied && !m_targetDied;
		}

		public int CompareTo(ServerPlayOrderGroup rhs)
		{
			if (rhs == null)
			{
				return 1;
			}
			if (m_casterDied != rhs.m_casterDied)
			{
				return rhs.m_casterDied.CompareTo(m_casterDied);
			}
			if (m_targetDied != rhs.m_targetDied)
			{
				return m_targetDied.CompareTo(rhs.m_targetDied);
			}
			if (Mathf.Abs(SortPosition.x - rhs.SortPosition.x) >= 0.02f)
			{
				return SortPosition.x.CompareTo(rhs.SortPosition.x);
			}
			return rhs.SortPosition.y.CompareTo(SortPosition.y);
		}

		private bool FindIfCasterDied()
		{
			for (int i = 0; i < m_casters.Count; i++)
			{
				if (m_turn.DeadAtEndOfTimelineIndex(m_casters[i].CastingActor, m_phaseIndex))
				{
					return true;
				}
			}
			return false;
		}

		private bool FindIfTargetDied()
		{
			foreach (Caster caster in m_casters)
			{
				foreach (ActorAnimation actorAnimation in caster.ActorAnimations)
				{
					if (actorAnimation.HitActorsToDeltaHP != null)
					{
						foreach (ActorData actor in actorAnimation.HitActorsToDeltaHP.Keys)
						{
							if (m_turn.DeadAtEndOfTimelineIndex(actor, m_phaseIndex))
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		private Vector3 FindSortPosition()
		{
			Vector3 vector = new Vector3(float.MinValue, float.MaxValue, 0f);
			foreach (Caster caster in m_casters)
			{
				if (Mathf.Abs(caster.SortPosition.x - vector.x) >= 0.02f)
				{
					if (caster.SortPosition.x > vector.x)
					{
						vector = caster.SortPosition;
					}
				}
				else if (caster.SortPosition.y < vector.y)
				{
					vector = caster.SortPosition;
				}
			}
			return vector;
		}

		internal bool CasterDied()
		{
			return m_casterDied;
		}

		internal bool TargetDied()
		{
			return m_targetDied;
		}

		private class Caster : IComparable<Caster>
		{
			internal Caster(ActorAnimation actorAnimation)
			{
				CastingActor = actorAnimation.Caster;
				ActorAnimations = new List<ActorAnimation>
				{
					actorAnimation
				};
			}

			internal void InitSortData()
			{
				SortPosition = Camera.main.WorldToViewportPoint(CastingActor.GetFreePos());
			}

			public int CompareTo(Caster rhs)
			{
				if (rhs == null)
				{
					return 1;
				}
				if (Mathf.Abs(SortPosition.x - rhs.SortPosition.x) >= 0.02f)
				{
					return SortPosition.x.CompareTo(rhs.SortPosition.x);
				}
				return rhs.SortPosition.y.CompareTo(SortPosition.y);
			}

			internal ActorData CastingActor { get; private set; }
			internal List<ActorAnimation> ActorAnimations { get; private set; }
			internal Vector3 SortPosition { get; private set; }
		}
	}
#endif
}
