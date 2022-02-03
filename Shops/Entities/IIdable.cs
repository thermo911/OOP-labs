namespace Shops.Entities
{
    /// <summary>
    /// Classes that implements IIdable must have Id - unsigned integer identifier
    /// </summary>
    public interface IIdable
    {
        uint Id { get; }
    }
}