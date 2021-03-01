using System.Collections.Generic;
using Alexa.NET.Request;

namespace MyMusic.Extensions
{
    public static class SessionExtensions
    {
        public static T GetSessionValue<T>(this Session session, string key) where T : class
        {
            if (session.Attributes == null)
                return null;

            if (!session.Attributes.ContainsKey(key))
                return null;

            return session.Attributes[key] as T;
        }

        public static void SetSessionValue(this Session session, string key, object value)
        {
            if (session.Attributes == null)
                session.Attributes = new Dictionary<string, object>();

            session.Attributes[key] = value;
        }
    }
}