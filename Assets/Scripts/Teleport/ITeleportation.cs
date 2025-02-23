// Check if ITeleportable object can teleport or not
// Teleport object
public interface ITeleportation
{
    public ITeleportable teleportableObj { get; }
    public void Teleport();
}
