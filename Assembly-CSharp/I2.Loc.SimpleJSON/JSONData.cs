using System.IO;

namespace I2.Loc.SimpleJSON
{
	public class JSONData : JSONNode
	{
		private string _0012;

		public override string _0009
		{
			get
			{
				return _0012;
			}
			set
			{
				_0012 = value;
			}
		}

		public JSONData(string _001D)
		{
			_0012 = _001D;
		}

		public JSONData(float _001D)
		{
			_0003 = _001D;
		}

		public JSONData(double _001D)
		{
			_000F = _001D;
		}

		public JSONData(bool _001D)
		{
			_0017 = _001D;
		}

		public JSONData(int _001D)
		{
			_000B = _001D;
		}

		public override string ToString()
		{
			return "\"" + JSONNode.escapeString(_0012) + "\"";
		}

		public override string _0004(string _001D)
		{
			return "\"" + JSONNode.escapeString(_0012) + "\"";
		}

		public override void _0002(BinaryWriter _001D)
		{
			JSONData jSONData = new JSONData(string.Empty);
			jSONData._000B = _000B;
			if (jSONData._0012 == _0012)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						_001D.Write((byte)4);
						_001D.Write(_000B);
						return;
					}
				}
			}
			jSONData._0003 = _0003;
			if (jSONData._0012 == _0012)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						_001D.Write((byte)7);
						_001D.Write(_0003);
						return;
					}
				}
			}
			jSONData._000F = _000F;
			if (jSONData._0012 == _0012)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						_001D.Write((byte)5);
						_001D.Write(_000F);
						return;
					}
				}
			}
			jSONData._0017 = _0017;
			if (jSONData._0012 == _0012)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						_001D.Write((byte)6);
						_001D.Write(_0017);
						return;
					}
				}
			}
			_001D.Write((byte)3);
			_001D.Write(_0012);
		}
	}
}
