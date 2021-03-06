using NStore.Core.Streams;

namespace NStore.Core.Processing
{
    public static class StreamProcessorExtensions
    {
        public static StreamProcessor Fold(this IReadOnlyStream stream)
        {
            var processor = new StreamProcessor(stream);
            return processor;
        }
    }
}