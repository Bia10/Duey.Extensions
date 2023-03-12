namespace Duey.Extensions.Extensions;

public static class NXNodeExtensionsIterations
{
    public static IEnumerable<INXNode> AllNodes(this INXNode root, string searchType, string collectionType)
    {
        return searchType switch
        {
            "breadth" when string.Equals(collectionType, "list", StringComparison.OrdinalIgnoreCase) => root
                .AllNodesListBreadthFirst(),
            "breadth" when string.Equals(collectionType, "linkedList", StringComparison.OrdinalIgnoreCase) => root
                .AllNodesLinkedListBreathFirst(),
            "depth" when string.Equals(collectionType, "list", StringComparison.OrdinalIgnoreCase) => root
                .AllNodesListDepthFirst(collection: null),
            "depth" when string.Equals(collectionType, "linkedList", StringComparison.OrdinalIgnoreCase) => root
                .AllNodesLinkedListDepthFirst(collection: null),
            _ => throw new ArgumentOutOfRangeException(nameof(searchType),
                $"Unknown args: {nameof(searchType)}, {nameof(collectionType)}")
        };
    }

    public static IEnumerable<INXNode> AllNodesListBreadthFirst(this INXNode rootNode)
    {
        ArgumentNullException.ThrowIfNull(rootNode);

        var result = new List<INXNode>();
        var queue = new Queue<INXNode>();
        queue.Enqueue(rootNode);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            result.Add(node);

            foreach (var child in node.Children)
                queue.Enqueue(child);
        }

        return result;
    }

    public static IEnumerable<INXNode> AllNodesLinkedListBreathFirst(this INXNode rootNode)
    {
        ArgumentNullException.ThrowIfNull(rootNode);

        var result = new LinkedList<INXNode>();
        var stack = new Stack<INXNode>();
        stack.Push(rootNode);

        while (stack.Count > 0)
        {
            var node = stack.Pop();
            result.AddFirst(node);

            foreach (var child in node.Children)
                stack.Push(child);
        }

        return result;
    }

    public static IEnumerable<INXNode> AllNodesListDepthFirst(this INXNode rootNode, IEnumerable<INXNode>? collection)
    {
        collection ??= new List<INXNode>(rootNode.Count());
        collection = collection.ToList();

        foreach (var child in rootNode.Children)
            child.AllNodesListDepthFirst(collection);

        return collection;
    }

    public static IEnumerable<INXNode> AllNodesLinkedListDepthFirst(this INXNode rootNode,
        LinkedList<INXNode>? collection)
    {
        ArgumentNullException.ThrowIfNull(rootNode);

        collection ??= new LinkedList<INXNode>();
        collection.AddFirst(rootNode);

        foreach (var child in rootNode.Children)
            child.AllNodesLinkedListDepthFirst(collection);

        return collection;
    }

    public static int CountSumRecursion(this INXNode node)
    {
        return 1 + node.Children.Sum(CountSumRecursion);
    }

    public static int CountStackIteration(this INXNode node)
    {
        var count = 0;
        var stack = new Stack<INXNode>();
        stack.Push(node);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            count++;
            foreach (var child in current.Children)
                stack.Push(child);
        }

        return count;
    }

    public static int CountNodesRecursive(this INXNode node)
    {
        var count = 1;
        foreach (var child in node.Children)
            count += child.CountNodesRecursive();
        return count;
    }

    public static int CountBreadthFirst(this INXNode rootNode)
    {
        var count = 0;
        var queue = new Queue<INXNode>();
        queue.Enqueue(rootNode);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            count++;

            foreach (var child in node.Children)
                queue.Enqueue(child);
        }

        return count;
    }
}