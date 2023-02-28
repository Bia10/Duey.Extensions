namespace Duey.Extensions.TestConsole;

internal static class Program
{
    private static void Main()
    {
        //var name = node.ResolveOrDefault<string>("name");
        //var image = node.ResolveOrDefault<NXBitmap>("image");
        //var soundFx = node.ResolveOrDefault<NXAudio>("soundFx");

        var file = new NXFile(@"C:\Users\Bia\Downloads\Data (2)\Quest.nx");
        var npcRefs = file.ReferencesToNpcNodesInImage("Say");

        foreach (var npcRef in npcRefs)
        {
            Console.WriteLine($"Quest node of id: '{npcRef.ParentNode.Name}' contains one child node of name: '{npcRef.ReferencingNode.Name}' " +
                              $"\n making reference to npc node id: '{npcRef.ReferencedNodeName}' !" +
                              $"\n referencing data: {npcRef.ReferencingNodeData}");
        }
    }
}