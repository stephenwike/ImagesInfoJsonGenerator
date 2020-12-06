using System;

namespace ImageJsonGenerator
{
    [Serializable]
    internal class SimpleImage
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public float WidthRes { get; set; }
        public float HeightRes { get; set; }
    }
}