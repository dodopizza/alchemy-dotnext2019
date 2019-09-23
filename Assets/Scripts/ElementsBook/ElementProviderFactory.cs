namespace ElementsBook
{
    public static class ElementProviderFactory
    {
        public static IElementProvider Provider { get; } = new LocalElementProvider();
    }
}
