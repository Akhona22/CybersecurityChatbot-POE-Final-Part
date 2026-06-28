namespace CybersecurityChatbot
{
    public class SentimentDetector
    {
        // Detects emotion in user input
        public string DetectSentiment(string input)
        {
            input = input.ToLower();

            // Worried emotion
            if (input.Contains("worried") ||
                input.Contains("scared") ||
                input.Contains("afraid"))
            {
                return "worried";
            }

            // Angry emotion
            if (input.Contains("angry") ||
                input.Contains("frustrated") ||
                input.Contains("mad"))
            {
                return "angry";
            }

            // Curious emotion
            if (input.Contains("curious") ||
                input.Contains("interested"))
            {
                return "curious";
            }

            // Default
            return "neutral";
        }
    }
}