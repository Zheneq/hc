using System;

public class Neko_AdditionalFxDiscPoweredUp : AdditionalVfxContainerBase
{
	private Neko_SyncComponent m_syncComp;

	private BoardSquare m_targetSquare;

	private Sequence m_parentSequence;

	public override void Initialize(Sequence parentSequence)
	{
		base.Initialize(parentSequence);
		if (parentSequence != null && parentSequence.Caster != null)
		{
			this.m_syncComp = parentSequence.Caster.GetComponent<Neko_SyncComponent>();
			this.m_targetSquare = Board.\u000E().\u000E(parentSequence.TargetPos);
			this.m_parentSequence = parentSequence;
		}
	}

	public override bool CanBeVisible(bool parentSeqVisible)
	{
		if (this.m_parentSequence != null && this.m_parentSequence.AgeInTurns > 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_AdditionalFxDiscPoweredUp.CanBeVisible(bool)).MethodHandle;
			}
			if (parentSeqVisible)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_syncComp != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_targetSquare != null)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (GameFlowData.Get() != null)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							bool result;
							if (this.m_syncComp.m_clientLastDiscBuffTurn == GameFlowData.Get().CurrentTurn)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								result = (this.m_syncComp.m_clientDiscBuffTargetSquare == this.m_targetSquare);
							}
							else
							{
								result = false;
							}
							return result;
						}
					}
				}
			}
		}
		return false;
	}
}
