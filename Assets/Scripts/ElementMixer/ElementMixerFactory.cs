namespace ElementMixer
{
    public static class ElementMixerFactory
    {
        public static IElementMixer Mixer { get; } = new LocalElementMixer();
    }
}