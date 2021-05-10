namespace Core.Interface
{
    public interface IDeepClone<out T> : IDeepClone
    {
        public new T DeepClone();
    }

    public interface IDeepClone
    {
        public object DeepClone();
    }
}
