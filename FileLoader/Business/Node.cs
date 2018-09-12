namespace FileLoader.Business
{
    public class Node
    {
        public Node()
        {
            Children = new NodeCollection();
        }

        public NodeCollection Children { get; }
    }
}
