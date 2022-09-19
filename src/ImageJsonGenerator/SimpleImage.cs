using System;

namespace ImageJsonGenerator
{
    [Serializable]
    internal class SimpleImage
    {
        public string FilePath { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public float WidthRes { get; set; }
        public float HeightRes { get; set; }
        public string Topic { get; internal set; }
    }
}