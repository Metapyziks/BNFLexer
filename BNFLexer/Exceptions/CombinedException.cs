using System;
using System.Collections.Generic;
using System.Linq;

namespace BNFLexer.Exceptions
{
    public class CombinedException : ParserException
    {
        public IEnumerable<ParserException> Exceptions { get; private set; }

        public override string MessageNoLocation
        {
            get
            {
                if (Exceptions.All(x => x is SymbolExpectedException)) {
                    var es = Exceptions.Select(x => ((SymbolExpectedException) x).Symbol).Distinct();
                    return String.Join(", ", es.Where(x => x != es.Last()))
                        + (es.Count() > 1 ? " or " : "") + (es.Count() > 0 ? es.Last() : "something") + " expected";
                }
                return String.Join(" or ", Exceptions.Select(x => x is SymbolExpectedException
                    ? ((SymbolExpectedException) x).Symbol : x.MessageNoLocation).Distinct());
            }
        }

        public CombinedException(params ParserException[] exceptions)
            : base(null, exceptions.First().SourceIndex, 0)
        {
            Exceptions = exceptions.SelectMany(x => x is CombinedException
                ? ((CombinedException) x).Exceptions : new ParserException[] { x });

            Utility = Exceptions.Max(x => x.Utility);
        }
    }
}
