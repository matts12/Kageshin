using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kageshin {
	class Lexicon {
		//members
		private readonly static string[] EXPRESSIONS = ConvertToNames(Constants.EXP_DIR);
		private readonly static string[] WORDS = ConvertToNames(Constants.WORDS_DIR);
		private readonly static string[] STRUCTURES = ConvertToNames(Constants.STR_DIR);
		internal delegate int MatchesWord(string[] list, Word w);
		private static MatchesWord EXPRESSION_PATTERN = (string[] dirFiles, Word w) => {
			int ifDir = Array.IndexOf(dirFiles, w.GetText());
			return ifDir == -1 ? Array.IndexOf(dirFiles, w.GetText() + ".txt") : ifDir;
		};
		private static MatchesWord STRUCTURE_PATTERN = (string[] dirFiles, Word w) => {
			int ifDir = Array.IndexOf(dirFiles, Enum.GetName(typeof(POS), w.GetPOS())); //TODO performance drain?
			ifDir = ifDir == -1 ? Array.IndexOf(dirFiles, Enum.GetName(typeof(POS), w.GetPOS()) + ".txt") : ifDir;
			return ifDir == -1 ? Array.IndexOf(dirFiles, w.GetText()) : ifDir;
		};
		//constructors
		internal Lexicon() {
			//foreach(string s in WORDS) {
			//	Console.WriteLine(s);
			//}
		}
		//methods
		internal static string[] ConvertToNames(string path, bool keepExtension = false) {
			string[] arr = Directory.GetFileSystemEntries(path);
			//remove file path info
			for (int i = 0; i < arr.Length; i++) {
				arr[i] = keepExtension ? Path.GetFileName(arr[i]) : Path.GetFileNameWithoutExtension(arr[i]);
			}
			return arr;
		}
		internal List<string> ParseWords(string input) {
			List<string> strWords = new List<string>();
			string currWord = "";
			//parse words from sentence
			for (int i = 0; i < input.Length; i++) {
				string ch = input[i].ToString().ToLower();
				if (input[i] == ' ') {
					strWords.Add(currWord);
					currWord = "";
				}
				else if (input[i] == ',' || input[i] == '?') {
					strWords.Add(currWord);
					currWord = ch;
				}
				else {
					currWord += ch;
				}
			}
			if (currWord.Length != 0) {
				strWords.Add(currWord);
			}
			return strWords;
		}
		internal List<Readable> ConvertToWords(List<string> strWords) {
			//transcribe to word objects
			List<Readable> words = new List<Readable>();
			foreach (string s in strWords) {
				Console.WriteLine(s);
				if (Array.IndexOf(WORDS, s) >= 0) {
					words.Add(new Word(s));
				}
				else {
					words.Add(new BaseWord(s));
				}
			}
			return words;
		}
		internal void ReplaceWords(List<Readable> words, string[] dictionary, string startingDir, MatchesWord pattern, bool isExpression) {
			for (int i = 0; i < words.Count; i++) {
				//word has structure
				Structure exp = FindStructure(words, i, dictionary, startingDir, new List<Word>(), pattern, isExpression);
				if (exp != null) {
					//remove words that make up the structure
					for (int j = 0; j < exp.Length; j++) {
						words.RemoveAt(i);
					}
					//insert the structure
					words.Insert(i, exp);
				}
			}
		}
		internal string GetResponse(string input) {
			List<Readable> words = ConvertToWords(ParseWords(input));
			//replace words with expressions
			ReplaceWords(words, EXPRESSIONS, Constants.EXP_DIR, EXPRESSION_PATTERN, true);
			//replace remaining words with structures
			ReplaceWords(words, STRUCTURES, Constants.STR_DIR, STRUCTURE_PATTERN, false);
			Clause phr = new Clause(words.ToArray());
			Console.WriteLine(phr);
			return "...";
		}
		private Structure FindStructure(List<Readable> words, int i, string[] dirFiles, string currPath, List<Word> expWords, MatchesWord pattern, bool isExpression) {
			if (i >= words.Count) return null;
			Word w = words[i] as Word;
			if(w != null) {
				int fileIndex = pattern(dirFiles, w);
				Console.WriteLine(fileIndex);
				//next word in expression found
				if (fileIndex >= 0) {
					//add the word
					expWords.Add(w);
					//end of the line
					if (dirFiles[fileIndex].EndsWith(".txt")) {
						string data = "";
						using (StreamReader sr = new StreamReader(currPath + dirFiles[fileIndex])) {
							data = sr.ReadToEnd();
						}
						if (isExpression) {
							//TODO include data
							return new Expression(expWords.ToArray());
						}
						else {
							string[] args = data.Split(' ');
							Console.WriteLine(args[0]);
							//TODO sort by relative amount to optimise searching
							if (args[0].Equals("NounPhrase")) {
								return new NounPhrase(expWords.ToArray(), short.Parse(args[1]));
							}
							else if (args[0].Equals("ComplexNounPhrase")) {
								return new ComplexNounPhrase(expWords.ToArray(), short.Parse(args[1]), short.Parse(args[2]));
							}
							else if (args[0].Equals("CompoundVerb")) {
								return new CompoundVerb(expWords.ToArray(), short.Parse(args[1]));
							}
							else {
								return new PrepPhrase(expWords.ToArray(), short.Parse(args[1]));
							}
						}
					}
					else {
						return FindStructure(words, i + 1, ConvertToNames(currPath + dirFiles[fileIndex] + @"\", true), currPath + dirFiles[fileIndex] + @"\", expWords, pattern, isExpression);
					}
					//TODO here
				}
				else {
					//structure doesn't have an end
					return null;
				}
			}
			else {
				return null; //TODO
			}
		}
	}
	class Clause {
		//members
		private Readable[] reads;
		//constructors
		internal Clause(Readable[] reads) {
			this.reads = reads;
		}
		//methods
		public override string ToString() {
			string s = "";
			foreach(Readable r in reads) {
				s += r + " ";
			}
			return s;
		}
		private void FindFeeling() {
			//TODO add up all words
		}
	}
}
