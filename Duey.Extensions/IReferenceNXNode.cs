namespace Duey.Extensions;

internal interface IReferenceNXNode
{
    public INXNode LoadReferencedNodeByName(string name);
}