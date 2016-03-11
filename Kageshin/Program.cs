using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kageshin {
	class Program {
		//members
		//methods
		static void Main(string[] args) {
			//TODO greet
			Lexicon lex = new Lexicon();
			while (true) {
				string input = Console.ReadLine();
				Console.WriteLine(lex.GetResponse(input));
			}
		}
	}
}
