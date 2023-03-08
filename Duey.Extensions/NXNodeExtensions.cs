﻿using System.Globalization;
using System.Text.RegularExpressions;
using DotNext.Collections.Generic;
using Duey.Layout;

namespace Duey.Extensions;

public static class NXNodeExtensions
{
    public static IEnumerable<INXNode> FileImageByName(this INXNode node, string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Image name cannot be null or empty.", nameof(name));

        return node.Children.Where(child => child.Name.Equals(name, StringComparison.Ordinal));
    }

    public static IEnumerable<INXNode> ChildrenByName(this INXNode node, string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Child name cannot be null or empty.", nameof(name));

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
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Parent name cannot be null or empty.", nameof(name));

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
            .Select(childNode => new ReferenceNXNode(
                node,
                childNode,
                childNode.ResolveReferencedNpcNodeId(),
                childNode.ResolveOrDefault<string>()));
    }

    public static IReadOnlyList<ReferenceNXNode> AllReferencesToNpcNodesInFile(this INXNode node)
    {
        if (node is not NXFile nxFileType)
            throw new ArgumentException("Node is not of type NXFile cannot continue.", nameof(nxFileType));

        var referenceNxNodes = new List<ReferenceNXNode>();

        foreach (var imgChildNode in nxFileType.Children)
            referenceNxNodes.AddRange(nxFileType.ReferencesToNpcNodesInImage(imgChildNode.Name));

        nxFileType.Dispose();
        return referenceNxNodes
            .ToList()
            .AsReadOnly();
    }

    public static IEnumerable<ReferenceNXNode> ReferencesToNpcNodesInImage(this INXNode node, string imgName)
    {
        if (string.IsNullOrEmpty(imgName))
            throw new ArgumentException("Image name cannot be null or empty.", nameof(imgName));

        return node.FileImageByName(imgName)
            .SelectMany(imgNode => imgNode.Children)
            .SelectMany(entryNode => entryNode.ReferencesToNpcNodes());
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
            .Select(childNode => new ReferenceNXNode(
                node,
                childNode,
                childNode.ResolveReferencedLocationNodeId(),
                childNode.ResolveOrDefault<string>()));
    }

    public static IEnumerable<ReferenceNXNode> ReferencesToLocationNodesInImage(this INXNode node, string imgName)
    {
        if (string.IsNullOrEmpty(imgName))
            throw new ArgumentException("Image name cannot be null or empty.", nameof(imgName));

        return node.FileImageByName(imgName)
            .SelectMany(imgNode => imgNode.Children)
            .SelectMany(entryNode => entryNode.ReferencesToLocationNodes());
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
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        return node.Parent.Where(parent => parent.Name.Equals(name, StringComparison.Ordinal));
    }

    public static IEnumerable<INXNode> AllChildrenOfType(this INXNode node, NXNodeType type,
        IEnumerable<INXNode>? collection = null)
    {
        var result = new HashSet<INXNode>();

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

        return result.DistinctBy(n => n.ResolveOrDefault<string>());
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
            if (resType != typeof(int) && resType != typeof(long) &&
                resType != typeof(float) && resType != typeof(double))
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