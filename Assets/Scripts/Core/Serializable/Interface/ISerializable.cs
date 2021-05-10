namespace Core.Interface
{
    public interface ISerializable
    {
        public string Guid { get; }

        public bool Equals(ISerializable item);
    }

}
