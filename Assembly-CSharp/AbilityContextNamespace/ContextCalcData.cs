using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	public class ContextCalcData
	{
		public ContextVars _001D = new ContextVars();

		public Dictionary<ActorData, ActorHitContext> _000E = new Dictionary<ActorData, ActorHitContext>();

		public void _0012()
		{
			_000E.Clear();
			_001D.Clear();
		}

		public void _0012(ActorData _001D, Vector3 _000E, bool _0012 = false)
		{
			if (_001D == null)
			{
				Log.Error("Trying to add null actor");
			}
			if (!this._000E.ContainsKey(_001D))
			{
				this._000E.Add(_001D, new ActorHitContext());
				this._000E[_001D]._001D = _000E;
				this._000E[_001D]._000E = _0012;
			}
			else
			{
				if (!Application.isEditor)
				{
					return;
				}
				while (true)
				{
					Log.Warning("TargetSelect context: trying to add actor more than once");
					return;
				}
			}
		}

		public void _0012(ActorData _001D, int _000E, int _0012)
		{
			if (this._000E.ContainsKey(_001D))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						this._000E[_001D]._0015.SetInt(_000E, _0012);
						return;
					}
				}
			}
			if (Application.isEditor)
			{
				Log.Warning("Setting context for actor we didn't track");
			}
		}

		public void _0012(ActorData _001D, int _000E, float _0012)
		{
			if (this._000E.ContainsKey(_001D))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						this._000E[_001D]._0015.SetFloat(_000E, _0012);
						return;
					}
				}
			}
			if (!Application.isEditor)
			{
				return;
			}
			while (true)
			{
				Log.Warning("Setting context for actor we didn't track");
				return;
			}
		}

		public void _0012(ActorData _001D, int _000E, Vector3 _0012)
		{
			if (this._000E.ContainsKey(_001D))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						this._000E[_001D]._0015.SetVector(_000E, _0012);
						return;
					}
				}
			}
			if (!Application.isEditor)
			{
				return;
			}
			while (true)
			{
				Log.Warning("Setting context for actor we didn't track");
				return;
			}
		}
	}
}
