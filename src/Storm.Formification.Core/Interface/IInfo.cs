namespace Storm.Formification.Core
{
    public static partial class Forms
    {
        public interface IInfo
        {
            string Slug { get; }
            string Name { get; }
            string Id { get; }
            int Version { get; }
        }
    }
}
