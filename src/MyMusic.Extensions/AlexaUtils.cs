using Alexa.NET.Response;

namespace MyMusic.Extensions
{
    public static class AlexaUtils
    {
        public static SkillResponse ReturnEmptySkillResponse(bool endSession = false)
        {
            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new ResponseBody
                {
                    ShouldEndSession = endSession
                }
            };
            return skillResponse;
        }
        public static SkillResponse TellWithoutEnding(string message)
        {
            var skillResponse = new SkillResponse
            {
                Version = "1.0",
                Response = new ResponseBody
                {
                    ShouldEndSession = false,
                    OutputSpeech = new PlainTextOutputSpeech { Text = message}
                }
            };
            return skillResponse;
        }
    }
}