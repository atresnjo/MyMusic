using Alexa.NET.Request.Type;

namespace MyMusic.Extensions
{
    public static class IntentSlotExtensions
    {
        public static string GetSlotValue(this IntentRequest intentRequest, string slotName)
        {
            if (!intentRequest.Intent.Slots.ContainsKey(slotName))
                return null;

            var value = intentRequest.Intent.Slots[slotName];
            return value.Value;
        }
    }
}