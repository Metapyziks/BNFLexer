using System.Collections.Generic;
using System.Linq;

using BNFLexer.Exceptions;
using BNFLexer.Nodes;

namespace BNFLexer.Parsers.Atomic
{
    /// <summary>
    /// Atomic parser that parses a single letter.
    /// </summary>
    public class LetterParser : Parser
    {
        /// <summary>
        /// Constructor to create a new letter parser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        public LetterParser(Ruleset ruleset)
            : base(ruleset) { }

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            int init = i;
            if (whitespace) SkipWhitespace(str, ref i);
            if (i < str.Length && char.IsLetter(str[i])) {
                ++i; return true;
            }
            i = init; return false;
        }

        public override IEnumerable<Node> Parse(string str, int i, bool whitespace, out ParserException exception)
        {
            int j = i;
            if (IsMatch(str, ref j, whitespace)) {
                exception = null;
                return new Node[] { new LeafNode(i, 1, str[i].ToString(), "letter") };
            } else {
                exception = new SymbolExpectedException("Letter", i, 1);
                return Enumerable.Empty<Node>();
            }
        }

        public override string ToString()
        {
            return "letter";
        }
    }
}
