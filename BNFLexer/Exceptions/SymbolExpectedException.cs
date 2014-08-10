using System;

namespace BNFLexer.Exceptions
{
    public class SymbolExpectedException : ParserException
    {
        public String Symbol { get; private set; }

        public SymbolExpectedException(String symbol, int index, int utility = 0)
            : base(String.Format("{0} expected", symbol), index)
        {
            Symbol = symbol;
            Utility = utility;
        }
    }
}
