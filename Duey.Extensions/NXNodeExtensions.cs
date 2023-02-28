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
        return node.ChildrenTextMatchesPattern(RegexPatterns.AnyNpc);
    }

    public static IEnumerable<INXNode> ChildrenReferencingMapNode(this INXNode node)
    {
        return node.ChildrenTextMatchesPattern(RegexPatterns.AnyMap);
    }

    public static IEnumerable<INXNode> ChildrenReferencingItemNode(this INXNode node)
    {
        return node.ChildrenTextMatchesPattern(RegexPatterns.AnyItem);
    }

    public static IEnumerable<INXNode> ChildrenReferencingMobNode(this INXNode node)
    {
        return node.ChildrenTextMatchesPattern(RegexPatterns.AnyMob);
    }

    public static IEnumerable<INXNode> ParentByType(this INXNode node, NXNodeType type)
    {
        return node.Parent.Where(parent => parent.Type.Equals(type));
    }

    public static IEnumerable<INXNode> ParentByName(this INXNode node, string name)
    {
        return node.Parent.Where(parent => parent.Name.Equals(name, StringComparison.Ordinal));
    }

    public static IEnumerable<INXNode> AllChildrenOfType(this INXNode node, NXNodeType type)
    {
        var result = new List<INXNode>();

        var matching = node.ChildrenByType(type);
        result.AddRange(matching);

        foreach (var child in node.Children)
        {
            var matchingType = child.ChildrenByType(type);
            result.AddRange(matchingType);

            if (child.Children.Any())
                child.AllChildrenOfType(type);
        }

        return result;
    }

    public static IEnumerable<INXNode> ChildrenTextMatchesPattern(this INXNode node, Regex pattern)
    {
        var matchingChildren = new List<INXNode>();

        foreach (var child in node.ChildrenByType(NXNodeType.String))
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