using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BNFLexer.Exceptions;
using BNFLexer.Nodes;

namespace BNFLexer.Parsers.Atomic
{
    /// <summary>
    /// Atomic parser that parses an identifier.
    /// </summary>
    public class IdentParser : Parser
    {
        /// <summary>
        /// Constructor to create a new identifier parser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        public IdentParser(Ruleset ruleset)
            : base(ruleset) { }

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            int init = i;
            if (whitespace) SkipWhitespace(str, ref i);
            int j = 0;
            while (i < str.Length && (char.IsLetter(str[i]) || (j > 0 && char.IsDigit(str[i])))) {
                ++i; ++j;
            }
            if (j > 0 && !Ruleset.IsKeyword(str.Substring(init, j))) return true;
            i = init; return false;
        }

        public override IEnumerable<Node> Parse(string str, int i, bool whitespace, out ParserException exception)
        {
            int j = i;
            if (IsMatch(str, ref i, whitespace)) {
                exception = null;
                try {
                    return new Node[] { Ruleset.GetSubstitution(new LeafNode(j, i - j, str.Substring(j, i - j), "ident")) };
                } catch (ParserException e) {
                    exception = e;
                    return Enumerable.Empty<Node>();
                }
            } else {
                exception = new SymbolExpectedException("Identifier", i, 1);
                return Enumerable.Empty<Node>();
            }
        }

        public override string ToString()
        {
            return "ident";
        }
    }
}
