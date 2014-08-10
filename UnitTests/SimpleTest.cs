using System;
using System.IO;
using BNFLexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class SimpleTest
    {
        [TestMethod]
        public void SimpleGrammarTest1()
        {
            var src = File.ReadAllText("Resources/grammars/simple.ebnf");
            var ruleset = Ruleset.FromString(src);

            return;
        }
    }
}
