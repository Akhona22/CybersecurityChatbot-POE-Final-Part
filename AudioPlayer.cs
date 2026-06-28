using System.Media;

namespace CybersecurityChatbot
{
    public class AudioPlayer
    {
        // Plays greeting audio
        public void PlayGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("greeting.wav");

                player.PlaySync();
            }
            catch
            {
                // Ignore audio errors
            }
        }
    }
}