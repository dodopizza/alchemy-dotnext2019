namespace Workbench
{
    public static class ElementMixerFactory
    {
        public static IElementMixer Mixer { get; } = new NetworkElementMixer();
    }
}