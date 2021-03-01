using System;

namespace MyMusic.Domain
{
    [Serializable]
    public class SpotifyDevice
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }

        public SpotifyDevice()
        {

        }

        public SpotifyDevice(string id, string name, int index)
        {
            Id = id;
            Name = name;
            Index = index;
        }
    }
}