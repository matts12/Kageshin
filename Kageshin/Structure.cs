using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kageshin {
	abstract class Structure : Readable {
		//members
		protected Readable[] words;
		//properties
		internal int Length { get { return words.Length; } }
		//constructors
		internal Structure(Readable[] words) {
			this.words = words;
		}
		//methods
		internal override string GetText() {
			string t = "";
			foreach (Word w in words) {
				t += w.GetText() + " ";
			}
			return t;
		}
		public override string ToString() {
			return GetText();
		}
	}
	class Expression : Structure {
		//members
		//constructors
		internal Expression(Readable[] words) : base(words) {

		}
		//methods
		public override string ToString() {
			return "EXP( " + base.ToString() + ")";
		}
	}
	class NounPhrase : Structure {
		//members
		private short headNoun;
		//constructors
		internal NounPhrase(Readable[] words, short headNoun) : base(words) {
			this.headNoun = headNoun;
		}
		//methods
		public override string ToString() {
			return "NOUN_PHRASE( " + base.ToString() + " " + words[headNoun] + ")";
		}
	}
	class ComplexNounPhrase : Structure {
		//members
		private short headNoun1, headNoun2;
		//constructors
		internal ComplexNounPhrase(Readable[] words, short headNoun1, short headNoun2) : base(words) {
			this.headNoun1 = headNoun1;
			this.headNoun2 = headNoun2;
		}
		//methods
		public override string ToString() {
			return "COMPLEX_NOUN_PHRASE( " + base.ToString() + " " + words[headNoun1] + " " + words[headNoun2] + ")";
		}
	}
	class PrepPhrase : Structure {
		//members
		private short prep;
		//constructors
		internal PrepPhrase(Readable[] words, short prep) : base(words) {
			this.prep = prep;
		}
		//methods
		public override string ToString() {
			return "PREP_PHRASE( " + base.ToString() + " " + words[prep] + ")";
		}
	}
	class CompoundVerb : Structure {
		//members
		private short headVerb;
		//constructors
		internal CompoundVerb(Readable[] words, short headVerb) : base(words) {
			this.headVerb = headVerb;
		}
		//methods
		public override string ToString() {
			return "COMPOUND_VERB( " + base.ToString() + " " + words[headVerb] + ")";
		}
	}
}
