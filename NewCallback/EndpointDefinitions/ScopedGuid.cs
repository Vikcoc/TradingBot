namespace NewCallback.EndpointDefinitions
{
    public class ScopedGuid
    {
        public ScopedGuid()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; }
    }
}
