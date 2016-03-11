using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kageshin {
	enum POS { NONE, VERB, NOUN, ADJ, ART, PREP }
	class Word : BaseWord{
		//members
		private Word parent;
		private POS pos;
		private Connotation conno;
		private string[] related;
		//constructors
		internal Word(string text) : base(text){
			//give basic declarations
			parent = null;
			pos = POS.NONE;
			conno = null;
			related = null;
			string fileName = Constants.WORDS_DIR + text + ".txt";
			using (StreamReader sr = new StreamReader(fileName)) {
				while (!sr.EndOfStream) {
					string line = sr.ReadLine();
					if (line.StartsWith("parent: ")) {
						parent = new Word(line.Substring(8));
					}
					else if (line.StartsWith("related: ")) {
						related = line.Substring(9).Split(',');
					}
					else if(line.StartsWith("POS: ")) {
						switch (line.Substring(5)) {
							case "VERB": pos = POS.VERB; break;
							case "NOUN": pos = POS.NOUN; break;
							case "ADJ": pos = POS.ADJ; break;
							case "ART": pos = POS.ART; break;
							case "PREP": pos = POS.PREP; break;
						}
					}
					else if (line.StartsWith("conno: ")) {
						conno = new Connotation(line.Substring(7));
						Console.WriteLine(conno);
					}
				}
			}
		}
		//methods
		public override string ToString() {
			return text + "("+pos+")";
		}
		internal override string GetText() {
			return parent != null ? parent.GetText() : text;
		}
		internal POS GetPOS() {
			return pos.Equals(POS.NONE) && parent != null ? parent.GetPOS(): pos;
		}
	}
	class BaseWord : Readable{
		//members
		protected string text;
		//constructors
		internal BaseWord(string text) {
			this.text = text;
		}
		//methods
		public override string ToString() {
			return "?/" + text + "/?";
		}
		internal override string GetText() {
			return text;
		}
	}
	abstract class Readable {
		//members
		//constructors
		//methods
		internal abstract string GetText();
	}
}
