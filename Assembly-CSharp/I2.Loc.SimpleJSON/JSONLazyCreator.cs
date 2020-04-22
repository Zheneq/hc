using System.Reflection;

namespace I2.Loc.SimpleJSON
{
	[DefaultMember("Item")]
	internal class JSONLazyCreator : JSONNode
	{
		private JSONNode _0015;

		private string _0016;

		public override JSONNode _0018
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				JSONArray jSONArray = new JSONArray();
				jSONArray._0013(value);
				_000A(jSONArray);
			}
		}

		public override JSONNode symbol_0018_2
		{
			get
			{
				return new JSONLazyCreator(this, _001D);
			}
			set
			{
				JSONClass jSONClass = new JSONClass();
				jSONClass._0013(_001D, value);
				_000A(jSONClass);
			}
		}

		public override int _000B
		{
			get
			{
				JSONData jSONData = new JSONData(0);
				_000A(jSONData);
				return 0;
			}
			set
			{
				JSONData jSONData = new JSONData(value);
				_000A(jSONData);
			}
		}

		public override float _0003
		{
			get
			{
				JSONData jSONData = new JSONData(0f);
				_000A(jSONData);
				return 0f;
			}
			set
			{
				JSONData jSONData = new JSONData(value);
				_000A(jSONData);
			}
		}

		public override double _000F
		{
			get
			{
				JSONData jSONData = new JSONData(0.0);
				_000A(jSONData);
				return 0.0;
			}
			set
			{
				JSONData jSONData = new JSONData(value);
				_000A(jSONData);
			}
		}

		public override bool _0017
		{
			get
			{
				JSONData jSONData = new JSONData(false);
				_000A(jSONData);
				return false;
			}
			set
			{
				JSONData jSONData = new JSONData(value);
				_000A(jSONData);
			}
		}

		public override JSONArray _000D
		{
			get
			{
				JSONArray jSONArray = new JSONArray();
				_000A(jSONArray);
				return jSONArray;
			}
		}

		public override JSONClass _0008
		{
			get
			{
				JSONClass jSONClass = new JSONClass();
				_000A(jSONClass);
				return jSONClass;
			}
		}

		public JSONLazyCreator(JSONNode _001D)
		{
			_0015 = _001D;
			_0016 = null;
		}

		public JSONLazyCreator(JSONNode _001D, string _000E)
		{
			_0015 = _001D;
			_0016 = _000E;
		}

		private void _000A(JSONNode _001D)
		{
			if (_0016 == null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				_0015._0013(_001D);
			}
			else
			{
				_0015._0013(_0016, _001D);
			}
			_0015 = null;
		}

		public override void _0013(JSONNode _001D)
		{
			JSONArray jSONArray = new JSONArray();
			jSONArray._0013(_001D);
			_000A(jSONArray);
		}

		public override void _0013(string _001D, JSONNode _000E)
		{
			JSONClass jSONClass = new JSONClass();
			jSONClass._0013(_001D, _000E);
			_000A(jSONClass);
		}

		public static bool _000A(JSONLazyCreator _001D, object _000E)
		{
			if (_000E == null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return true;
					}
				}
			}
			return object.ReferenceEquals(_001D, _000E);
		}

		public static bool _0006(JSONLazyCreator _001D, object _000E)
		{
			return !_000A(_001D, _000E);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return true;
					}
				}
			}
			return object.ReferenceEquals(this, obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return string.Empty;
		}

		public override string _0004(string _001D)
		{
			return string.Empty;
		}
	}
}
