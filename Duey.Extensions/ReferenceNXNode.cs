namespace Duey.Extensions;

public class ReferenceNXNode : IReferenceNXNode
{
    public readonly INXNode ParentNode;
    public readonly string ReferencedNodeName;
    public readonly INXNode ReferencingNode;
    public readonly string ReferencingNodeData;

    public ReferenceNXNode(
        INXNode parentNode,
        INXNode referencingNode,
        string referencedNodeName,
        string referencingNodeData)
    {
        ParentNode = parentNode;
        ReferencingNode = referencingNode;
        ReferencedNodeName = referencedNodeName;
        ReferencingNodeData = referencingNodeData;
    }

    public INXNode LoadReferencedNodeByName(string name)
    {
        throw new NotImplementedException();
    }
}