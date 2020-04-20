using System;
using System.Reflection;

namespace I2.Loc.SimpleJSON
{
	[DefaultMember("Item")]
	internal class JSONLazyCreator : JSONNode
	{
		private JSONNode symbol_0015;

		private string symbol_0016;

		public JSONLazyCreator(JSONNode symbol_001D)
		{
			this.symbol_0015 = symbol_001D;
			this.symbol_0016 = null;
		}

		public JSONLazyCreator(JSONNode symbol_001D, string symbol_000E)
		{
			this.symbol_0015 = symbol_001D;
			this.symbol_0016 = symbol_000E;
		}

		private void symbol_000A(JSONNode symbol_001D)
		{
			if (this.symbol_0016 == null)
			{
				this.symbol_0015.symbol_0013(symbol_001D);
			}
			else
			{
				this.symbol_0015.symbol_0013(this.symbol_0016, symbol_001D);
			}
			this.symbol_0015 = null;
		}

		public override JSONNode symbol_0018
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				JSONArray jsonarray = new JSONArray();
				jsonarray.symbol_0013(value);
				this.symbol_000A(jsonarray);
			}
		}

		public override JSONNode symbol_0018_2
		{
			get
			{
				return new JSONLazyCreator(this, symbol_001D);
			}
			set
			{
				JSONClass jsonclass = new JSONClass();
				jsonclass.symbol_0013(symbol_001D, value);
				this.symbol_000A(jsonclass);
			}
		}

		public override void symbol_0013(JSONNode symbol_001D)
		{
			JSONArray jsonarray = new JSONArray();
			jsonarray.symbol_0013(symbol_001D);
			this.symbol_000A(jsonarray);
		}

		public override void symbol_0013(string symbol_001D, JSONNode symbol_000E)
		{
			JSONClass jsonclass = new JSONClass();
			jsonclass.symbol_0013(symbol_001D, symbol_000E);
			this.symbol_000A(jsonclass);
		}

		public static bool symbol_000A(JSONLazyCreator symbol_001D, object symbol_000E)
		{
			if (symbol_000E == null)
			{
				return true;
			}
			return object.ReferenceEquals(symbol_001D, symbol_000E);
		}

		public static bool symbol_0006(JSONLazyCreator symbol_001D, object symbol_000E)
		{
			return !JSONLazyCreator.symbol_000A(symbol_001D, symbol_000E);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return true;
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

		public override string symbol_0004(string symbol_001D)
		{
			return string.Empty;
		}

		public override int symbol_000B
		{
			get
			{
				JSONData u001D = new JSONData(0);
				this.symbol_000A(u001D);
				return 0;
			}
			set
			{
				JSONData u001D = new JSONData(value);
				this.symbol_000A(u001D);
			}
		}

		public override float symbol_0003
		{
			get
			{
				JSONData u001D = new JSONData(0f);
				this.symbol_000A(u001D);
				return 0f;
			}
			set
			{
				JSONData u001D = new JSONData(value);
				this.symbol_000A(u001D);
			}
		}

		public override double symbol_000F
		{
			get
			{
				JSONData u001D = new JSONData(0.0);
				this.symbol_000A(u001D);
				return 0.0;
			}
			set
			{
				JSONData u001D = new JSONData(value);
				this.symbol_000A(u001D);
			}
		}

		public override bool symbol_0017
		{
			get
			{
				JSONData u001D = new JSONData(false);
				this.symbol_000A(u001D);
				return false;
			}
			set
			{
				JSONData u001D = new JSONData(value);
				this.symbol_000A(u001D);
			}
		}

		public override JSONArray symbol_000D
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.symbol_000A(jsonarray);
				return jsonarray;
			}
		}

		public override JSONClass symbol_0008
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.symbol_000A(jsonclass);
				return jsonclass;
			}
		}
	}
}
