using System.Globalization;
using System.Text.RegularExpressions;
using DotNext.Collections.Generic;
using Duey.Extensions.Nodes;
using Duey.Extensions.Regexes;
using Duey.Layout;

namespace Duey.Extensions.Extensions;

public static class NXNodeExtensions
{
    public static IEnumerable<INXNode> ChildrenByName(this INXNode node, string name)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrEmpty(name);

        return node.Children.Where(child => child.Name.Equals(name, StringComparison.Ordinal));
    }

    public static IEnumerable<INXNode> ChildrenByType(this INXNode node, NXNodeType type)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(type);

        return node.Children.Where(child => child.Type.Equals(type));
    }

    public static IEnumerable<INXNode> ChildrenParents(this INXNode node)
    {
        ArgumentNullException.ThrowIfNull(node);

        return node.Children.Select(static child => child.Parent);
    }

    public static IEnumerable<INXNode> ChildrenParentsByName(this INXNode node, string name)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrEmpty(name);

        return node.Children.Where(child => child.Parent.Name.Equals(name, StringComparison.Ordinal));
    }

    public static IEnumerable<INXNode> ChildrenParentsByType(this INXNode node, NXNodeType type)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(type);

        return node.Children.Where(child => child.Parent.Type.Equals(type));
    }

    public static IEnumerable<INXNode> ParentByType(this INXNode node, NXNodeType type)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(type);

        return node.Parent.Where(parent => parent.Type.Equals(type));
    }

    public static IEnumerable<INXNode> ParentByName(this INXNode node, string name)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrEmpty(name);

        return node.Parent.Where(parent => parent.Name.Equals(name, StringComparison.Ordinal));
    }

    public static string ResolveReferencedNodeId(this INXNode node)
    {
        ArgumentNullException.ThrowIfNull(node);

        var data = node.ResolveOrDefault<string>();
        var match = RegexPatterns.AnyNpcName().Match(data);

        return match.Value[2..];
    }

    public static IEnumerable<ReferenceNXNode> ReferencesToReferencedType(this INXNode node,
        ReferencedType referencedType)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(referencedType);

        return node.ChildrenOfReferenceType(referencedType)
            .Select(childNode => new ReferenceNXNode(node, childNode, childNode.ResolveReferencedNodeId(),
                childNode.ResolveOrDefault<string>(), referencedType));
    }

    public static IEnumerable<ReferenceNXNode> ReferencesToReferencedTypeInImage(this INXNode node, string imgName,
        ReferencedType referencedType)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrEmpty(imgName);
        ArgumentNullException.ThrowIfNull(referencedType);

        return node.FileImageByName(imgName)
            .SelectMany(static imgNode => imgNode.Children)
            .SelectMany(entryNode => entryNode.ReferencesToReferencedType(referencedType));
    }

    public static IEnumerable<INXNode> ChildrenOfReferenceType(this INXNode node, ReferencedType referencedType)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(referencedType);

        var patternToSearch = referencedType switch
        {
            ReferencedType.Map => RegexPatterns.AnyMapName(),
            ReferencedType.Mob => RegexPatterns.AnyMobName(),
            ReferencedType.Npc => RegexPatterns.AnyNpcName(),
            ReferencedType.Item => RegexPatterns.AnyItemName(),
            ReferencedType.Skill => RegexPatterns.AnySkillName(),
            ReferencedType.Character => RegexPatterns.AnyPlayerName(),
            _ => throw new ArgumentOutOfRangeException(nameof(referencedType), referencedType, null)
        };

        return node.AnyChildrenTextMatchesPattern(patternToSearch ?? throw new InvalidOperationException());
    }

    public static IEnumerable<INXNode> AllChildrenOfType(this INXNode node, NXNodeType type,
        IEnumerable<INXNode>? collection = null)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(type);

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
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(pattern);

        foreach (var child in node.AllChildrenOfType(NXNodeType.String))
            if (child.TryResolveOrDefault(out string? resolvedStr) && !string.IsNullOrWhiteSpace(resolvedStr) &&
                pattern.IsMatch(resolvedStr))
                yield return child;
    }

    public static bool TryResolveOrDefault<T>(this INXNode node, out T? result, string? path = null) where T : class
    {
        ArgumentNullException.ThrowIfNull(node);

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