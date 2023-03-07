using System.Text.RegularExpressions;
using Duey.Layout;

namespace Duey.Extensions;

public static class NXNodeExtensions
{
    public static IEnumerable<INXNode> FileImageByName(this INXNode node, string name)
    {
        return node.Children.Where(child => child.Name.Equals(name + ".img", StringComparison.Ordinal));
    }

    public static IEnumerable<INXNode> ChildrenByName(this INXNode node, string name)
    {
        return node.Children.Where(child => child.Name.Equals(name, StringComparison.Ordinal));
    }

    public static IEnumerable<INXNode> ChildrenByType(this INXNode node, NXNodeType type)
    {
        return node.Children.Where(child => child.Type.Equals(type));
    }

    public static IEnumerable<INXNode> ChildrenParents(this INXNode node)
    {
        return node.Children.Select(child => child.Parent);
    }

    public static IEnumerable<INXNode> ChildrenParentsByName(this INXNode node, string name)
    {
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
            .Replace("#", string.Empty)
            .Replace("p", string.Empty);
    }

    public static IEnumerable<ReferenceNXNode> ReferencesToNpcNodes(this INXNode node)
    {
        var result = new List<ReferenceNXNode>();

        foreach (var childNode in node.ChildrenReferencingNpcNode())
        {
            var reference = new ReferenceNXNode(
                node,
                childNode,
                childNode.ResolveReferencedNpcNodeId(),
                childNode.ResolveOrDefault<string>());

            result.Add(reference);
        }

        return result;
    }

    public static IEnumerable<ReferenceNXNode> ReferencesToNpcNodesInImage(this INXNode node, string imgName)
    {
        var result = new List<ReferenceNXNode>();

        foreach (var imgNode in node.FileImageByName(imgName))
        foreach (var entryNode in imgNode.Children)
            result.AddRange(entryNode.ReferencesToNpcNodes());

        return result;
    }

    public static IEnumerable<INXNode> ChildrenReferencingImageLocationNode(this INXNode node)
    {
        return node.AnyChildrenTextMatchesPattern(RegexPatterns.ImageLocation2());
    }

    public static string ResolveReferencedLocationNodeId(this INXNode node)
    {
        var data = node.ResolveOrDefault<string>();
        var match = RegexPatterns.ImageLocation2().Match(data);

        return match.Value
            .Replace("#", string.Empty)
            .Replace("f", string.Empty);
    }


    public static IEnumerable<ReferenceNXNode> ReferencesToLocationNodes(this INXNode node)
    {
        var result = new List<ReferenceNXNode>();

        foreach (var childNode in node.ChildrenReferencingImageLocationNode())
        {
            var reference = new ReferenceNXNode(
                node,
                childNode,
                childNode.ResolveReferencedLocationNodeId(),
                childNode.ResolveOrDefault<string>());

            result.Add(reference);
        }

        return result;
    }

    public static IEnumerable<ReferenceNXNode> ReferencesToLocationNodesInImage(this INXNode node, string imgName)
    {
        var result = new List<ReferenceNXNode>();

        foreach (var imgNode in node.FileImageByName(imgName))
        foreach (var entryNode in imgNode.Children)
            result.AddRange(entryNode.ReferencesToLocationNodes());

        return result;
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
        return node.Parent.Where(parent => parent.Name.Equals(name, StringComparison.Ordinal));
    }

    public static IEnumerable<INXNode> AllChildrenOfType(this INXNode node, NXNodeType type,
        List<INXNode>? collection = null)
    {
        var result = collection ?? new List<INXNode>();

        var matching = node.ChildrenByType(type);
        result.AddRange(matching.ToArray());

        foreach (var child in node.Children)
        {
            var matchingType = child.ChildrenByType(type);
            result.AddRange(matchingType.ToArray());

            if (child.Children.Any())
                child.AllChildrenOfType(type, result);
        }

        return result.DistinctBy(n => n.ResolveOrDefault<string>());
    }

    public static IEnumerable<INXNode> AnyChildrenTextMatchesPattern(this INXNode node, Regex pattern)
    {
        var matchingChildren = new List<INXNode>();

        foreach (var child in node.AllChildrenOfType(NXNodeType.String))
        {
            var resolved = child.TryResolveOrDefault(out string? resolvedStr);
            if (resolved && !string.IsNullOrEmpty(resolvedStr) && pattern.IsMatch(resolvedStr))
                matchingChildren.Add(child);
        }

        return matchingChildren;
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
            if (resType != typeof(int) && resType != typeof(long) &&
                resType != typeof(float) && resType != typeof(double))
                return false;

            result = (T)Convert.ChangeType(res, typeof(T));
            return true;
        }

        if (resType != typeof(T) || typeof(T) != typeof(string))
            return false;

        result = (T)res;
        return true;
    }
}