namespace Duey.Extensions;

public interface IReferenceNXNode
{
    public INXNode LoadReferencedNodeByName(string name);
}