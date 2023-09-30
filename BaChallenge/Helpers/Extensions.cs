using System.IO;

namespace BaChallenge.Helpers
{
    public static class Extensions
    {
        public static void WriteLine(this TextWriter[] streams) => WriteLine(streams, string.Empty);

        public static void WriteLine(this TextWriter[] streams, string value)
        {
            foreach (var writer in streams)
            {
                writer.WriteLine(value);
            }
        }
    }
}