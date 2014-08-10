using System;

namespace BNFLexer.Nodes
{
    public abstract class Node
    {
         /// <summary>
        /// String identifying the type of this node.
        /// </summary>
        public String Token { get; set; }

        /// <summary>
        /// The parsed contents of this node.
        /// </summary>
        public abstract String String { get; }

        /// <summary>
        /// Start index of this node in the original source string.
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// Length of this node in the original source string.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// End index of this node in the original source string.
        /// </summary>
        public int EndIndex { get { return StartIndex + Length; } }

        /// <summary>
        /// Gets a value representing if this null is useless.
        /// </summary>
        public abstract bool IsNull { get; }

        /// <summary>
        /// Abstract constructor for a new Node.
        /// </summary>
        /// <param name="index">Start index of this node in the original source string</param>
        /// <param name="length">Length of this node in the original source string</param>
        /// <param name="token">String identifying the type of this node</param>
        public Node(int index, int length, String token = null)
        {
            StartIndex = index;
            Length = length;

            Token = token;
        }

        public override String ToString()
        {
            return SerializeXML();
        }

        public abstract String SerializeXML();
    }
}
