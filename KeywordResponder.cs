using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    public class KeywordResponder
    {
        // Dictionary stores keywords and LISTS of responses
        private Dictionary<string, List<string>> responses;

        // Random object
        private Random random;

        public KeywordResponder()
        {
            random = new Random();

            responses = new Dictionary<string, List<string>>();

            // PASSWORD RESPONSES
            responses.Add("password", new List<string>
            {
                "Use strong passwords with uppercase, lowercase, numbers, and symbols.",
                "Avoid using your name or birthdate in passwords.",
                "Change your passwords regularly for better security."
            });

            // PHISHING RESPONSES
            responses.Add("phishing", new List<string>
            {
                "Never click suspicious links in emails or messages.",
                "Phishing scams often pretend to be trusted companies.",
                "Always verify email senders before sharing information."
            });

            // PRIVACY RESPONSES
            responses.Add("privacy", new List<string>
            {
                "Always protect your personal information online.",
                "Avoid sharing sensitive details on public websites.",
                "Check privacy settings on your social media accounts."
            });

            // SCAM RESPONSES
            responses.Add("scam", new List<string>
            {
                "Be careful of fake messages asking for money.",
                "Scammers often create urgency to trick victims.",
                "Never share banking details with unknown people."
            });

            // MALWARE RESPONSES
            responses.Add("malware", new List<string>
            {
                "Install antivirus software to stay protected.",
                "Avoid downloading files from unknown websites.",
                "Keep your software updated to prevent malware attacks."
            });
        }

        // Returns a RANDOM response
        public string GetResponse(string input)
        {
            input = input.ToLower();

            foreach (var item in responses)
            {
                if (input.Contains(item.Key))
                {
                    // Pick random response
                    int index = random.Next(item.Value.Count);

                    return item.Value[index];
                }
            }

            // Default response
            return "I did not understand that. Please ask a cybersecurity question.";
        }
    }
}