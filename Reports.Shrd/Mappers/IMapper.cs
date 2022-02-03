namespace Reports.Shrd.Mappers
{
    public interface IMapper
    {
        T Map<T>(object o) where T : new();
    }
}