using System.Collections.Generic;
using System.Linq;

using BNFLexer.Exceptions;
using BNFLexer.Nodes;

namespace BNFLexer.Parsers.Binary
{
    /// <summary>
    /// Binary parser that matches the left parser, and optionally matches
    /// the right parser as many times as possible.
    /// </summary>
    public class OptionalRepeatParser : BinaryParser
    {
        /// <summary>
        /// Constructor to create a new OptionalRepeatParser.
        /// </summary>
        /// <param name="ruleset">The ruleset that will contain this parser</param>
        /// <param name="left">Parser to be matched first</param>
        /// <param name="right">Parser to optionally attempt second</param>
        public OptionalRepeatParser(Ruleset ruleset, Parser left, Parser right)
            : base(ruleset, left, right) { }

        public override bool IsMatch(string str, ref int i, bool whitespace)
        {
            int init = i;
            if (Left == null || Left.IsMatch(str, ref i, whitespace)) {
                init = i;
                while (Right.IsMatch(str, ref i, whitespace)) init = i;

                // Reset index to before the last match was attempted
                i = init; return true;
            }
            i = init; return false;
        }

        public override IEnumerable<Node> Parse(string str, int i, bool whitespace, out ParserException exception)
        {
            exception = null;
            var nodes = new SortedSet<Node>(NodeComparer);
            List<Node> fresh;
            if (Left != null) {
                fresh = Left.Parse(str, i, whitespace, out exception).ToList();
            } else {
                fresh = new List<Node> { new BranchNode(i) };
            }
            var stale = new List<Node>();

            foreach (var left in fresh) nodes.Add(left);

            while (fresh.Count > 0) {
                stale = fresh;
                fresh = new List<Node>();

                foreach (var left in stale) {
                    ParserException innerError;
                    foreach (var right in Right.Parse(str, left.EndIndex, whitespace, out innerError)) {
                        Node next = null;
                        if (right.IsNull) {
                            continue;
                        } else if (left.IsNull) {
                            // If left side is null, don't bother concatenating
                            next = right;
                        } else if (left is BranchNode && left.Token == null) {
                            // If the parsed left hand side is a branch with no assigned token,
                            // append the parsed right hand side
                            next = new BranchNode(((BranchNode) left).Children.Concat(new Node[] { right }));
                        } else if (right is BranchNode && right.Token == null) {
                            // If the parsed right hand side is a branch with no assigned token,
                            // prepend the parsed left hand side
                            next = new BranchNode(new Node[] { left }.Concat(((BranchNode) right).Children));
                        } else {
                            // Otherwise, create a new branch with only the two parsed results
                            next = new BranchNode(new Node[] { left, right });
                        }
                        fresh.Add(next);
                        nodes.Add(next);
                    }
                    exception = ChooseParserException(exception, innerError);
                }
            }

            return nodes;
        }

        public override string ToString()
        {
            return (Left != null ? Left.ToString() + " " : "") + "{" + Right.ToString() + "}";
        }
    }
}
