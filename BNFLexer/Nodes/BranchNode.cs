using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNFLexer.Nodes
{
    public class BranchNode : Node, IEnumerable<Node>
    {
        /// <summary>
        /// Utility function to get the source index of a list of sibling nodes.
        /// </summary>
        /// <param name="children">List of siblings</param>
        /// <returns>Source index of the list</returns>
        static int GetBranchIndex(IEnumerable<Node> children)
        {
            return children.First().StartIndex;
        }

        /// <summary>
        /// Utility function to get the source length of a list of sibling nodes.
        /// </summary>
        /// <param name="children">List of siblings</param>
        /// <returns>Source length of the list</returns>
        static int GetBranchLength(IEnumerable<Node> children)
        {
            return children.Last().StartIndex - children.First().StartIndex + children.Last().Length;
        }

        /// <summary>
        /// Collection of child nodes within this node.
        /// </summary>
        public IEnumerable<Node> Children { get; protected set; }

        public override bool IsNull
        {
            get { return Token == null && Children.Count() == 0; }
        }

        public override String String
        {
            get
            {
                var builder = new StringBuilder();
                foreach (var node in Children) {
                    builder.Append(node.String);
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Constructor for a new Branch.
        /// </summary>
        /// <param name="children">Collection of child nodes within this node</param>
        /// <param name="token">String identifying the type of this node</param>
        public BranchNode(IEnumerable<Node> children, String token = null)
            : base(GetBranchIndex(children), GetBranchLength(children), token)
        {
            Children = children.SelectMany(x => x is BranchNode && x.Token == null ? ((BranchNode) x).Children : new Node[] { x });
        }

        /// <summary>
        /// Constructor for an empty BranchNode.
        /// </summary>
        /// <param name="index">Start index of this node in the original source string</param>
        /// <param name="token">String identifying the type of this node</param>
        public BranchNode(int index, String token = null)
            : base(index, 0, token)
        {
            Children = Enumerable.Empty<Node>();
        }

        public BranchNode(int index, int length, String token = null)
            : base(index, length, token)
        {
            Children = Enumerable.Empty<Node>();
        }

        public override String SerializeXML()
        {
            var nl = Environment.NewLine;
            return String.Format("<{0} index=\"{1}\" length=\"{2}\">", Token, StartIndex, Length)
                + (Children.Count() > 0 ? nl + String.Join(nl, Children.Select(x => x.SerializeXML())) : "")
                    .Replace("\n", "\n  ") + nl + String.Format("</{0}>", Token);
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
