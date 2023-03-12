using Duey.Extensions.Nodes;
using Duey.Layout;

namespace Duey.Extensions.Extensions;

public static class NXFileExtensions
{
    private const string NxFileNamePattern = "*.nx";
    private static readonly string UserProfileDirPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    private static readonly string DirectoryPath = Path.Join(UserProfileDirPath, "Downloads", "Data (2)");

    public static IEnumerable<NXFile> LoadNxFiles(string directoryPath)
    {
        if (string.IsNullOrEmpty(directoryPath))
            directoryPath = DirectoryPath;

        var files = Directory
            .GetFiles(directoryPath, NxFileNamePattern)
            .ToArray();

        return files.Select(static file => new NXFile(file));
    }

    public static IEnumerable<INXNode> FileImageByName(this INXNode node, string name)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentException.ThrowIfNullOrEmpty(name);

        return node.Children.Where(child => child.Name.Equals(name, StringComparison.Ordinal));
    }

    public static IReadOnlyList<ReferenceNXNode> AllReferencesToNpcNodesInFile(this INXNode node)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (node is not NXFile nxFileType)
            throw new ArgumentException("Node is not of type NXFile cannot continue.", nameof(node));

        List<ReferenceNXNode> referenceNxNodes = new(node.Count());

        foreach (var imgChildNode in nxFileType.Children)
            referenceNxNodes.AddRange(nxFileType.ReferencesToReferencedTypeInImage(imgChildNode.Name, ReferencedType.Npc));

        return referenceNxNodes.AsReadOnly();
    }

    public static IEnumerable<INXNode> GetAllReferencesToNodeTypeInFile(this INXNode rootNode, NXNodeType targetType)
    {
        ArgumentNullException.ThrowIfNull(rootNode);
        ArgumentNullException.ThrowIfNull(targetType);

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
}