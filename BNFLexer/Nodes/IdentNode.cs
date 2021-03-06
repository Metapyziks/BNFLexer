﻿using System;

namespace BNFLexer.Nodes
{
    /// <summary>
    /// Substitution node for identifiers.
    /// </summary>
    [SubstituteToken("ident")]
    public class IdentNode : SubstituteNode
    {
        private String _string;

        public override string String
        {
            get { return _string; }
        }

        /// <summary>
        /// Constructor to create a new identifier substitution
        /// node.
        /// </summary>
        /// <param name="original">The original parse node to be substituted</param>
        public IdentNode(Node original)
            : base(original, true)
        {
            _string = base.String.Trim();
        }
    }
}
