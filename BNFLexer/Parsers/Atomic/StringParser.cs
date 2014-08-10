using System.Collections.Generic;
using System.Linq;

using BNFLexer.Exceptions;
using BNFLexer.Nodes;

namespace BNFLexer.Parsers.Atomic
{
    /// <summary>
    /// Atomic parser that parses a string literal surrounded by either
    /// single or double quotes.
    /// </summary>
    public class StringParser : Parser
    {
        /// <summary>
        /// Constructor to create a new string literal parser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        public StringParser(Ruleset ruleset) : base(ruleset) { }

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            int init = i;
            if (whitespace) SkipWhitespace(str, ref i);
            if (i < str.Length && (str[i] == '"' || str[i] == '\'')) {
                char startChar = str[i];
                while (++i < str.Length) {
                    if (str[i] == '\\') {
                        ++i;
                    }
                    if (str[i] == startChar) {
                        ++i;
                        return true;
                    }
                }
            }
            i = init; return false;
        }

        // TODO: Add proper support for escape characters
        public override IEnumerable<Node> Parse(string str, int i, bool whitespace, out ParserException exception)
        {
            int j = i;
            if (IsMatch(str, ref i, whitespace)) {
                exception = null;
                try {
                    return new Node[] { Ruleset.GetSubstitution(new LeafNode(j, i - j, str.Substring(j, i - j), "string")) };
                } catch (ParserException e) {
                    exception = e;
                    return Enumerable.Empty<Node>();
                }
            } else {
                exception = new SymbolExpectedException("StringLiteral", i, 1);
                return Enumerable.Empty<Node>();
            }
        }

        public override string ToString()
        {
            return "string";
        }
    }
}
