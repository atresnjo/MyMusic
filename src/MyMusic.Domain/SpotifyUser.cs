using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace MyMusic.Domain
{
    [DynamoDBTable("users")]
    public class SpotifyUser
    {
        [DynamoDBHashKey] public string UserIdentifier { get; set; }

        public List<SpotifyDevice> AvailableDevices { get; set; }
        public SpotifyDevice CurrentPlayingDevice { get; set; }

        public SpotifyUser(string userIdentifier) : this()
        {
            UserIdentifier = userIdentifier;
        }

        public SpotifyUser()
        {
            AvailableDevices = new List<SpotifyDevice>();
        }
    }
}