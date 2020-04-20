using System;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	public class JSONData : JSONNode
	{
		private string symbol_0012;

		public JSONData(string symbol_001D)
		{
			this.symbol_0012 = symbol_001D;
		}

		public JSONData(float symbol_001D)
		{
			this.symbol_0003 = symbol_001D;
		}

		public JSONData(double symbol_001D)
		{
			this.symbol_000F = symbol_001D;
		}

		public JSONData(bool symbol_001D)
		{
			this.symbol_0017 = symbol_001D;
		}

		public JSONData(int symbol_001D)
		{
			this.symbol_000B = symbol_001D;
		}

		public override string symbol_0009
		{
			get
			{
				return this.symbol_0012;
			}
			set
			{
				this.symbol_0012 = value;
			}
		}

		public override string ToString()
		{
			return "\"" + JSONNode.escapeString(this.symbol_0012) + "\"";
		}

		public override string symbol_0004(string symbol_001D)
		{
			return "\"" + JSONNode.escapeString(this.symbol_0012) + "\"";
		}

		public override void symbol_0002(BinaryWriter symbol_001D)
		{
			JSONData jsondata = new JSONData(string.Empty);
			jsondata.symbol_000B = this.symbol_000B;
			if (jsondata.symbol_0012 == this.symbol_0012)
			{
				symbol_001D.Write(4);
				symbol_001D.Write(this.symbol_000B);
				return;
			}
			jsondata.symbol_0003 = this.symbol_0003;
			if (jsondata.symbol_0012 == this.symbol_0012)
			{
				symbol_001D.Write(7);
				symbol_001D.Write(this.symbol_0003);
				return;
			}
			jsondata.symbol_000F = this.symbol_000F;
			if (jsondata.symbol_0012 == this.symbol_0012)
			{
				symbol_001D.Write(5);
				symbol_001D.Write(this.symbol_000F);
				return;
			}
			jsondata.symbol_0017 = this.symbol_0017;
			if (jsondata.symbol_0012 == this.symbol_0012)
			{
				symbol_001D.Write(6);
				symbol_001D.Write(this.symbol_0017);
				return;
			}
			symbol_001D.Write(3);
			symbol_001D.Write(this.symbol_0012);
		}
	}
}
