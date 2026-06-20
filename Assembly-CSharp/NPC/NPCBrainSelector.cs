// ROGUES
// SERVER
#if SERVER
using System;
using System.Collections.Generic;
using System.Linq;
using Malee;
using UnityEngine;

// added in rogues
public class NPCBrainSelector : MonoBehaviour
{
	private ActorData m_actorData;

	[HideInInspector]
	public NPCBrain_Adaptive m_npcBrain;
	[Separator("Default Brain Parameters")]
	public NPCBrainParameters m_defaultBrainParameters;
	public OtherOptions m_defaultOtherOptions;
	[Separator("Brain Entries")]
	[Reorderable("descriptor")]
	public NPCBrainEntryArray m_brainEntries;

	private void OnValidate()
	{
		if (!m_brainEntries.IsNullOrEmpty())
		{
			for (int i = 0; i < m_brainEntries.Length; i++)
			{
				NPCBrainEntry npcbrainEntry = m_brainEntries[i];
				for (int j = 0; j < npcbrainEntry.conditions.Length; j++)
				{
					Condition condition = npcbrainEntry.conditions[j];
					condition.descriptor = string.Concat(condition.valueType.ToString(), " ", condition.comparisonType.ToString(), " ", condition.checkValue.ToString());
				}
			}
		}
	}

	public virtual NPCBrainSelector Create(BotController bot)
	{
		NPCBrainSelector npcbrainSelector = bot.gameObject.AddComponent<NPCBrainSelector>();
		npcbrainSelector.m_brainEntries = m_brainEntries;
		npcbrainSelector.m_defaultBrainParameters = m_defaultBrainParameters;
		npcbrainSelector.m_defaultOtherOptions = m_defaultOtherOptions;
		npcbrainSelector.m_actorData = bot.GetComponent<ActorData>();
		
		// custom
		npcbrainSelector.m_npcBrain = NPCBrain_Adaptive.Create(bot, null, BotDifficulty.Medium, false) as NPCBrain_Adaptive;
		// rogues
		// npcbrainSelector.m_npcBrain = (NPCBrain_Adaptive.CreateDefault(bot, null) as NPCBrain_Adaptive);
		
		npcbrainSelector.SetupBrainParameters(m_defaultBrainParameters);
		npcbrainSelector.SetupOtherOptions(m_defaultOtherOptions);
		return npcbrainSelector;
	}

	private float GetClosestEnemyDistance()
	{
		BoardSquare currentBoardSquare = m_actorData.CurrentBoardSquare;
		List<ActorData> list = m_actorData.GetOtherTeams().SelectMany(otherTeam => GameFlowData.Get().GetAllTeamMembers(otherTeam)).ToList();
		float num = 10000f;
		foreach (ActorData actorData in list)
		{
			BoardSquare currentBoardSquare2 = actorData.GetCurrentBoardSquare();
			float num2 = currentBoardSquare.HorizontalDistanceOnBoardTo(currentBoardSquare2);
			if (num2 <= num)
			{
				num = num2;
			}
		}
		return num;
	}

	private float GetTurnsSinceAlerted()
	{
		int num = -1;
		BotController component = m_actorData.GetComponent<BotController>();
		if (component != null && component.m_alertedTurn != -1)
		{
			num = GameFlowData.Get().CurrentTurn - component.m_alertedTurn;
		}
		return num;
	}

	private bool EvaluateCondition(Condition condition)
	{
		bool result = false;
		float num = 0f;
		switch (condition.valueType)
		{
		case ValueType.HealthFraction:
			num = m_actorData.GetHitPointPercent();
			break;
		case ValueType.TurnsAlive:
			num = GameFlowData.Get().CurrentTurn - m_actorData.LastSpawnTurn;
			break;
		case ValueType.ClosestEnemyDistance:
			num = GetClosestEnemyDistance();
			break;
		case ValueType.TurnsSinceAlert:
			num = GetTurnsSinceAlerted();
			break;
		}
		switch (condition.comparisonType)
		{
		case ComparisonType.Less:
			result = num < condition.checkValue;
			break;
		case ComparisonType.Equal:
			result = num == condition.checkValue;
			break;
		case ComparisonType.Greater:
			result = num > condition.checkValue;
			break;
		}
		return result;
	}

	private void SetupBrainParameters(NPCBrainParameters npcBrainParams)
	{
		m_npcBrain.m_optimalRange = npcBrainParams.m_optimalRange;
		m_npcBrain.m_movementType = npcBrainParams.m_movementType;
		m_npcBrain.m_decisionPriority = npcBrainParams.m_decisionPriority;
		m_npcBrain.m_allowedAbilities = npcBrainParams.m_allowedAbilities;
	}

	private void SetupOtherOptions(OtherOptions otherOptions)
	{
		// rogues
		// base.GetComponent<ActorTurnSM>().NumAbilityActionsPerTurn = (uint)otherOptions.AbilityActionsPerTurn;
	}

	public void ChooseBrainParameters()
	{
		bool flag = false;
		foreach (NPCBrainEntry brain in m_brainEntries)
		{
			bool isValid = true;
			foreach (Condition condition in brain.conditions)
			{
				if (!EvaluateCondition(condition))
				{
					isValid = false;
					break;
				}
			}
			if (isValid)
			{
				SetupBrainParameters(brain.brainParameters);
				SetupOtherOptions(brain.otherOptions);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			SetupBrainParameters(m_defaultBrainParameters);
			SetupOtherOptions(m_defaultOtherOptions);
		}
	}

	public enum ValueType
	{
		HealthFraction,
		TurnsAlive,
		ClosestEnemyDistance,
		TurnsSinceAlert
	}

	public enum ComparisonType
	{
		Less,
		Equal,
		Greater
	}

	[Serializable]
	public class NPCBrainParameters
	{
		public float m_optimalRange;

		public NPCBrain_Adaptive.MovementType m_movementType;

		public int m_decisionPriority = 1;

		[AbilityIndexArray]
		public int[] m_allowedAbilities;
	}

	[Serializable]
	public class OtherOptions
	{
		public int AbilityActionsPerTurn = 1;
	}

	[Serializable]
	public class Condition
	{
		[HideInInspector]
		public string descriptor;

		public ValueType valueType;

		public ComparisonType comparisonType;

		public float checkValue;
	}

	[Serializable]
	public class ConditionArray : ReorderableArray<Condition>
	{
	}

	[Serializable]
	public class NPCBrainEntry
	{
		public string descriptor = "Empty";

		[Reorderable("descriptor")]
		public ConditionArray conditions;

		public NPCBrainParameters brainParameters;

		public OtherOptions otherOptions;
	}

	[Serializable]
	public class NPCBrainEntryArray : ReorderableArray<NPCBrainEntry>
	{
	}
}
#endif
