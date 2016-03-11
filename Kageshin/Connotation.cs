using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kageshin {
	class Connotation {
		//members
		private short[] intensities;
		//constructors
		internal Connotation(string lineText) {
			intensities = new short[Constants.FEELING_NUMBER];
			string[] feelSegs = lineText.Split('/');
			foreach(string s in feelSegs) {
				int par1 = s.IndexOf('(');
                string tag = s.Substring(0, par1);
				Console.WriteLine(s.Substring(par1 + 1, s.Length - par1 - 2));
				short value = short.Parse(s.Substring(par1 + 1, s.Length - par1 - 2));
				string[] names = Enum.GetNames(typeof(Feeling));
				bool found = false;
				for (int i = 0; i < names.Length && !found; i++) {
					if (names[i].Equals(tag)) {
						intensities[i] = value;
						found = true;
					}
				}
			}
		}
		//methods
		internal short FeelingIntensity(Feeling f) {
			return intensities[(int)f];
		}
		public override string ToString() {
			string[] names = Enum.GetNames(typeof(Feeling));
			string s = "";
			for (int i = 0; i < names.Length; i++) {
				s += names[i] + "(" + intensities[i] + ") ";
			}
			return s;
		}
	}
	enum Feeling{
		POS_NEG, CONTRAST, RUDE, NEW
	}
}
