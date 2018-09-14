namespace AxinomCommon.Business
{
    /*
     * This class represents the primordial node used in the Node Collection class
     */
    public class Node
    {
        public Node()
        {
            Children = new NodeCollection();
        }

        public NodeCollection Children { get; }
    }
}
