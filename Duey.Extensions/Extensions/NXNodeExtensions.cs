using System.Globalization;
using System.Text.RegularExpressions;
using DotNext.Collections.Generic;
using Duey.Extensions.Nodes;
using Duey.Layout;

namespace Duey.Extensions.Extensions;

public static class NXNodeExtensions
{
    public static IEnumerable<INXNode> FileImageByName(this INXNode node, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return node.Children.Where(child => child.Name.Equals(name, StringComparison.Ordinal));
    }

    public static IEnumerable<INXNode> ChildrenByName(this INXNode node, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return node.Children.Where(child => child.Name.Equals(name, StringComparison.Ordinal));
    }

    public static IEnumerable<INXNode> ChildrenByType(this INXNode node, NXNodeType type)
    {
        return node.Children.Where(child => child.Type.Equals(type));
    }

    public static IEnumerable<INXNode> ChildrenParents(this INXNode node)
    {
        return node.Children.Select(static child => child.Parent);
    }

    public static IEnumerable<INXNode> ChildrenParentsByName(this INXNode node, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return node.Children.Where(child => child.Parent.Name.Equals(name, StringComparison.Ordinal));
    }

    public static IEnumerable<INXNode> ChildrenParentsByType(this INXNode node, NXNodeType type)
    {
        return node.Children.Where(child => child.Parent.Type.Equals(type));
    }

    public static IEnumerable<INXNode> ChildrenReferencingNpcNode(this INXNode node)
    {
        return node.AnyChildrenTextMatchesPattern(RegexPatterns.AnyNpcName());
    }

    public static string ResolveReferencedNpcNodeId(this INXNode node)
    {
        var data = node.ResolveOrDefault<string>();
        var match = RegexPatterns.AnyNpcName().Match(data);

        return match.Value
            .Replace("#", string.Empty, StringComparison.Ordinal)
            .Replace("p", string.Empty, StringComparison.Ordinal);
    }

    public static IEnumerable<ReferenceNXNode> ReferencesToNpcNodes(this INXNode node)
    {
        return node.ChildrenReferencingNpcNode()
            .Select(childNode => new ReferenceNXNode(node,
                childNode,
                childNode.ResolveReferencedNpcNodeId(),
                childNode.ResolveOrDefault<string>(),
                ReferenceNXNode.ReferenceNodeType.Npc));
    }

    public static IEnumerable<INXNode> GetAllReferencesToNodeTypeInFile(this INXNode rootNode, NXNodeType targetType)
    {
        HashSet<INXNode> visitedNodes = new();
        Queue<INXNode> nodesToVisit = new();
        nodesToVisit.Enqueue(rootNode);

        while (nodesToVisit.Count > 0)
        {
            var currentNode = nodesToVisit.Dequeue();

            if (visitedNodes.Contains(currentNode))
                continue;

            visitedNodes.Add(currentNode);

            if (currentNode.Type == targetType)
            {
                var parentNode = currentNode.Parent;
                if (parentNode != null)
                    visitedNodes.Add(parentNode);
            }

            foreach (var childNode in currentNode.Children)
                nodesToVisit.Enqueue(childNode);
        }

        return visitedNodes;
    }

    public static IEnumerable<INXNode> AllNodes(this INXNode root, string searchType, string collectionType)
    {
        return searchType switch
        {
            "breadth" when string.Equals(collectionType, "list", StringComparison.OrdinalIgnoreCase) =>
                root.AllNodesListBreadthFirst(),
            "breadth" when string.Equals(collectionType, "linkedList", StringComparison.OrdinalIgnoreCase) =>
                root.AllNodesLinkedListBreathFirst(),
            "depth" when string.Equals(collectionType, "list", StringComparison.OrdinalIgnoreCase) =>
                root.AllNodesListDepthFirst(null),
            "depth" when string.Equals(collectionType, "linkedList", StringComparison.OrdinalIgnoreCase) =>
                root.AllNodesLinkedListDepthFirst(null),
            _ => throw new ArgumentOutOfRangeException(nameof(searchType),
                $"Unknown args: {nameof(searchType)}, {nameof(collectionType)}")
        };
    }

    public static IEnumerable<INXNode> AllNodesListBreadthFirst(this INXNode rootNode)
    {
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

    public static IEnumerable<INXNode> AllNodesListDepthFirst(this INXNode rootNode, List<INXNode>? collection)
    {
        collection ??= new List<INXNode>(rootNode.Count());
        collection.Add(rootNode);

        foreach (var child in rootNode.Children)
            child.AllNodesListDepthFirst(collection);

        return collection;
    }

    public static IEnumerable<INXNode> AllNodesLinkedListDepthFirst(this INXNode rootNode,
        LinkedList<INXNode>? collection)
    {
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

    public static IReadOnlyList<ReferenceNXNode> AllReferencesToNpcNodesInFile(this INXNode node)
    {
        if (node is not NXFile nxFileType)
            throw new ArgumentException("Node is not of type NXFile cannot continue.", nameof(node));

        List<ReferenceNXNode> referenceNxNodes = new(node.Count());

        foreach (var imgChildNode in nxFileType.Children)
            referenceNxNodes.AddRange(nxFileType.ReferencesToNpcNodesInImage(imgChildNode.Name));

        return referenceNxNodes.AsReadOnly();
    }

    public static IEnumerable<ReferenceNXNode> ReferencesToNpcNodesInImage(this INXNode node, string imgName)
    {
        ArgumentException.ThrowIfNullOrEmpty(imgName);

        return node.FileImageByName(imgName)
            .SelectMany(static imgNode => imgNode.Children)
            .SelectMany(static entryNode => entryNode.ReferencesToNpcNodes());
    }

    public static IEnumerable<INXNode> ChildrenReferencingImageLocationNode(this INXNode node)
    {
        return node.AnyChildrenTextMatchesPattern(RegexPatterns.AnyImageLocation2());
    }

    public static string ResolveReferencedLocationNodeId(this INXNode node)
    {
        var data = node.ResolveOrDefault<string>();
        var match = RegexPatterns.AnyImageLocation2().Match(data);

        return match.Value
            .Replace("#", string.Empty, StringComparison.Ordinal)
            .Replace("f", string.Empty, StringComparison.Ordinal);
    }

    public static IEnumerable<ReferenceNXNode> ReferencesToLocationNodes(this INXNode node)
    {
        return node.ChildrenReferencingImageLocationNode()
            .Select(childNode => new ReferenceNXNode(node,
                childNode,
                childNode.ResolveReferencedLocationNodeId(),
                childNode.ResolveOrDefault<string>(),
                ReferenceNXNode.ReferenceNodeType.Map));
    }

    public static IEnumerable<ReferenceNXNode> ReferencesToLocationNodesInImage(this INXNode node, string imgName)
    {
        ArgumentException.ThrowIfNullOrEmpty(imgName);

        return node.FileImageByName(imgName)
            .SelectMany(static imgNode => imgNode.Children)
            .SelectMany(static entryNode => entryNode.ReferencesToLocationNodes());
    }

    public static IEnumerable<INXNode> ChildrenReferencingMapNode(this INXNode node)
    {
        return node.AnyChildrenTextMatchesPattern(RegexPatterns.AnyMapName());
    }

    public static IEnumerable<INXNode> ChildrenReferencingItemNode(this INXNode node)
    {
        return node.AnyChildrenTextMatchesPattern(RegexPatterns.AnyItemName());
    }

    public static IEnumerable<INXNode> ChildrenReferencingMobNode(this INXNode node)
    {
        return node.AnyChildrenTextMatchesPattern(RegexPatterns.AnyMobName());
    }

    public static IEnumerable<INXNode> ParentByType(this INXNode node, NXNodeType type)
    {
        return node.Parent.Where(parent => parent.Type.Equals(type));
    }

    public static IEnumerable<INXNode> ParentByName(this INXNode node, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return node.Parent.Where(parent => parent.Name.Equals(name, StringComparison.Ordinal));
    }

    public static IEnumerable<INXNode> AllChildrenOfType(this INXNode node, NXNodeType type,
        IEnumerable<INXNode>? collection = null)
    {
        HashSet<INXNode> result = new();

        if (collection is not null)
            result.AddAll(collection.ToArray());

        var matching = node.ChildrenByType(type);
        result.AddAll(matching.ToArray());

        foreach (var child in node.Children)
        {
            var matchingType = child.ChildrenByType(type);
            result.AddAll(matchingType.ToArray());

            if (child.Children.Any())
                child.AllChildrenOfType(type, result);
        }

        return result.DistinctBy(static nxNode => nxNode.ResolveOrDefault<string>());
    }

    public static IEnumerable<INXNode> AnyChildrenTextMatchesPattern(this INXNode node, Regex pattern)
    {
        foreach (var child in node.AllChildrenOfType(NXNodeType.String))
            if (child.TryResolveOrDefault(out string? resolvedStr) && !string.IsNullOrWhiteSpace(resolvedStr) &&
                pattern.IsMatch(resolvedStr))
                yield return child;
    }

    public static bool TryResolveOrDefault<T>(this INXNode node, out T? result, string? path = null) where T : class
    {
        result = null;

        var res = node.ResolvePath(path)?.Resolve();
        if (res is null)
            return false;

        var resType = res.GetType();

        if (resType != typeof(T) && typeof(T) == typeof(string))
        {
            if (resType != typeof(int) && resType != typeof(long) && resType != typeof(float) &&
                resType != typeof(double))
                return false;

            result = (T)Convert.ChangeType(res, typeof(T), CultureInfo.InvariantCulture);
            return true;
        }

        if (resType != typeof(T) || typeof(T) != typeof(string))
            return false;

        result = (T)res;
        return true;
    }
}