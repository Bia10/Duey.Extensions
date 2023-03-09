namespace Duey.Extensions;

public sealed class ReferenceNXNode : IReferenceNXNode
{
    public enum ReferenceNodeType : short
    {
        Unknown = 0,
        Map,
        Mob,
        Npc,
        Item,
        Skill,
        Quest,
        Reactor,
        String,
        Etc,
        UI,
        Base,
        Character,
        List,
        Sound,
        TamingMob,
        Morph,
        Effect
    }

    public readonly INXNode ParentNode;
    public readonly string ReferencedNodeName;
    public readonly ReferenceNodeType ReferenceType;
    public readonly INXNode ReferencingNode;
    public readonly string ReferencingNodeData;

    internal ReferenceNXNode(
        INXNode parentNode,
        INXNode referencingNode,
        string referencedNodeName,
        string referencingNodeData,
        ReferenceNodeType referenceType)
    {
        ParentNode = parentNode;
        ReferencingNode = referencingNode;
        ReferencedNodeName = referencedNodeName;
        ReferencingNodeData = referencingNodeData;
        ReferenceType = referenceType;
    }

    public INXNode LoadReferencedNodeByName(string name)
    {
        var dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        var filePath = ReferenceType switch
        {
            ReferenceNodeType.Map => Path.Combine(dataPath, "Map.nx"),
            ReferenceNodeType.Mob => Path.Combine(dataPath, "Mob.nx"),
            ReferenceNodeType.Npc => Path.Combine(dataPath, "Npc.nx"),
            ReferenceNodeType.Item => Path.Combine(dataPath, "Item.nx"),
            ReferenceNodeType.Skill => Path.Combine(dataPath, "Skill.nx"),
            ReferenceNodeType.Quest => Path.Combine(dataPath, "Quest.nx"),
            ReferenceNodeType.Reactor => Path.Combine(dataPath, "Reactor.nx"),
            ReferenceNodeType.String => Path.Combine(dataPath, "String.nx"),
            ReferenceNodeType.Etc => Path.Combine(dataPath, "Etc.nx"),
            ReferenceNodeType.UI => Path.Combine(dataPath, "UI.nx"),
            ReferenceNodeType.Base => Path.Combine(dataPath, "Base.nx"),
            ReferenceNodeType.Character => Path.Combine(dataPath, "Character.nx"),
            ReferenceNodeType.List => Path.Combine(dataPath, "List.nx"),
            ReferenceNodeType.Sound => Path.Combine(dataPath, "Sound.nx"),
            ReferenceNodeType.TamingMob => Path.Combine(dataPath, "TamingMob.nx"),
            ReferenceNodeType.Morph => Path.Combine(dataPath, "Morph.nx"),
            ReferenceNodeType.Effect => Path.Combine(dataPath, "Effect.nx"),
            ReferenceNodeType.Unknown => throw new NotImplementedException(
                $"Unsupported ReferenceNXNodeType: {ReferenceType}"),
            _ => throw new NotImplementedException($"Unsupported ReferenceNXNodeType: {ReferenceType}")
        };

        using var nxFile = new NXFile(filePath);
        return nxFile.Root.ResolvePath(name);
    }
}