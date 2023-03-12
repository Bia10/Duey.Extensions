namespace Duey.Extensions.Nodes;

public sealed class ReferenceNXNode : IReferenceNXNode
{
    public readonly INXNode ParentNode;
    public readonly string ReferencedNodeName;
    public readonly ReferencedType ReferencedNodeType;
    public readonly INXNode ReferencingNode;
    public readonly string ReferencingNodeData;

    internal ReferenceNXNode(INXNode parentNode, INXNode referencingNode, string referencedNodeName,
        string referencingNodeData, ReferencedType referencedNodeType)
    {
        ParentNode = parentNode;
        ReferencingNode = referencingNode;
        ReferencedNodeName = referencedNodeName;
        ReferencingNodeData = referencingNodeData;
        ReferencedNodeType = referencedNodeType;
    }

    public INXNode LoadReferencedNodeByName(string name)
    {
        var dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        var filePath = ReferencedNodeType switch
        {
            ReferencedType.Map => Path.Combine(dataPath, "Map.nx"),
            ReferencedType.Mob => Path.Combine(dataPath, "Mob.nx"),
            ReferencedType.Npc => Path.Combine(dataPath, "Npc.nx"),
            ReferencedType.Item => Path.Combine(dataPath, "Item.nx"),
            ReferencedType.Skill => Path.Combine(dataPath, "Skill.nx"),
            ReferencedType.Quest => Path.Combine(dataPath, "Quest.nx"),
            ReferencedType.Reactor => Path.Combine(dataPath, "Reactor.nx"),
            ReferencedType.String => Path.Combine(dataPath, "String.nx"),
            ReferencedType.Etc => Path.Combine(dataPath, "Etc.nx"),
            ReferencedType.UI => Path.Combine(dataPath, "UI.nx"),
            ReferencedType.Base => Path.Combine(dataPath, "Base.nx"),
            ReferencedType.Character => Path.Combine(dataPath, "Character.nx"),
            ReferencedType.List => Path.Combine(dataPath, "List.nx"),
            ReferencedType.Sound => Path.Combine(dataPath, "Sound.nx"),
            ReferencedType.TamingMob => Path.Combine(dataPath, "TamingMob.nx"),
            ReferencedType.Morph => Path.Combine(dataPath, "Morph.nx"),
            ReferencedType.Effect => Path.Combine(dataPath, "Effect.nx"),
            _ => throw new ArgumentOutOfRangeException($"Unsupported ReferencedNodeType: {ReferencedNodeType}")
        };

        using NXFile nxFile = new(filePath);
        return nxFile.Root.ResolvePath(name);
    }
}