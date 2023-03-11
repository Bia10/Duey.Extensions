namespace Duey.Extensions.Nodes;

internal interface IReferenceNXNode
{
    public INXNode LoadReferencedNodeByName(string name);
}