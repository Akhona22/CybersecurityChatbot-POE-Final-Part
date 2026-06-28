using System;
using System.Collections.Generic;
using System.Text;

namespace CybersecurityChatbot
{
    public class ChatBot
    {
        // Helper classes
        private KeywordResponder responder;
        private SentimentDetector sentiment;
        private MemoryStore memory;
        private TaskRepository taskRepository;

        // Activity Log
        private List<string> activityLog;

        // Conversation memory
        private string lastTopic;

        // Quiz variables
        private bool quizStarted;
        private int currentQuestion;
        private int score;

        private string[] questions =
        {
            "Q1. What is phishing?\nA) A scam email\nB) Antivirus\nC) Firewall",
            "Q2. Which password is strongest?\nA) 123456\nB) Password\nC) P@ssw0rd!92",
            "Q3. What does MFA stand for?\nA) Multi-Factor Authentication\nB) My File Access\nC) Main Firewall Access",
            "Q4. What should you do with suspicious links?\nA) Click them\nB) Ignore or report them\nC) Share them",
            "Q5. Malware is...\nA) Harmful software\nB) Antivirus\nC) Email",
            "Q6. A firewall is used to...\nA) Protect networks\nB) Charge phones\nC) Store passwords",
            "Q7. Personal information should...\nA) Be shared publicly\nB) Be protected\nC) Be posted online",
            "Q8. What helps secure an account?\nA) MFA\nB) Weak passwords\nC) Sharing passwords",
            "Q9. Which email is safest?\nA) Unknown sender\nB) Verified sender\nC) Random link",
            "Q10. What should you do after creating a password?\nA) Share it\nB) Keep it private\nC) Email it"
        };

        private char[] answers =
        {
            'A','C','A','B','A','A','B','A','B','B'
        };

        // Constructor
        public ChatBot()
        {
            responder = new KeywordResponder();
            sentiment = new SentimentDetector();
            memory = new MemoryStore();
            taskRepository = new TaskRepository();

            activityLog = new List<string>();

            quizStarted = false;
            currentQuestion = 0;
            score = 0;
        }

        // Main chatbot method
        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please enter a message.";

            string originalInput = input;
            input = input.ToLower();
            // ==========================
            // START QUIZ
            // ==========================
            if (input == "start quiz")
            {
                quizStarted = true;
                currentQuestion = 0;
                score = 0;

                activityLog.Add("Started Quiz");

                return "Cybersecurity Quiz Started!\n\n" +
                       questions[currentQuestion] +
                       "\n\nType A, B or C.";
            }

            // ==========================
            // QUIZ ANSWERS
            // ==========================
            if (quizStarted)
            {
                char answer = char.ToUpper(originalInput.Trim()[0]);

                if (answer == answers[currentQuestion])
                {
                    score++;
                }

                currentQuestion++;

                if (currentQuestion >= questions.Length)
                {
                    quizStarted = false;

                    activityLog.Add("Finished Quiz");

                    return "Quiz Finished!\n" +
                           "Your Score: " + score +
                           " / " + questions.Length;
                }

                return questions[currentQuestion] +
                       "\n\nType A, B or C.";
            }

            // ==========================
            // ADD TASK
            // ==========================
            if (input.StartsWith("add task "))
            {
                string title = originalInput.Substring(9);

                TaskItem task = new TaskItem
                {
                    Title = title,
                    Description = "",
                    ReminderDate = null,
                    IsCompleted = false
                };

                taskRepository.AddTask(task);

                activityLog.Add("Added Task: " + title);

                return "Task saved successfully.";
            }

            // ==========================
            // SHOW TASKS
            // ==========================
            if (input == "show tasks")
            {
                List<TaskItem> tasks = taskRepository.GetAllTasks();

                if (tasks.Count == 0)
                    return "There are no tasks.";

                StringBuilder builder = new StringBuilder();

                builder.AppendLine("========== TASK LIST ==========");

                foreach (TaskItem task in tasks)
                {
                    builder.AppendLine(
                        task.TaskID + ". " +
                        task.Title +
                        (task.IsCompleted ? " [Completed]" : " [Pending]")
                    );
                }

                activityLog.Add("Viewed Tasks");

                return builder.ToString();
            }
            // ==========================
            // COMPLETE TASK
            // ==========================
            if (input.StartsWith("complete task "))
            {
                try
                {
                    int id = Convert.ToInt32(originalInput.Substring(14));

                    taskRepository.MarkTaskCompleted(id);

                    activityLog.Add("Completed Task " + id);

                    return "Task " + id + " marked as completed.";
                }
                catch
                {
                    return "Invalid task number.";
                }
            }

            // ==========================
            // DELETE TASK
            // ==========================
            if (input.StartsWith("delete task "))
            {
                try
                {
                    int id = Convert.ToInt32(originalInput.Substring(12));

                    taskRepository.DeleteTask(id);

                    activityLog.Add("Deleted Task " + id);

                    return "Task " + id + " deleted successfully.";
                }
                catch
                {
                    return "Invalid task number.";
                }
            }

            // ==========================
            // ACTIVITY LOG
            // ==========================
            if (input == "activity log")
            {
                if (activityLog.Count == 0)
                    return "No activity recorded.";

                StringBuilder builder = new StringBuilder();

                builder.AppendLine("===== ACTIVITY LOG =====");

                foreach (string activity in activityLog)
                {
                    builder.AppendLine(activity);
                }

                return builder.ToString();
            }

            // Save user name
            if (input.StartsWith("my name is "))
            {
                memory.UserName = originalInput.Replace("my name is ", "");
                return "Nice to meet you, " + memory.UserName + "!";
            }

            // Cybersecurity topics
            if (input.Contains("phishing"))
            {
                memory.FavouriteTopic = "Phishing";
                lastTopic = "phishing";
            }

            if (input.Contains("password"))
            {
                memory.FavouriteTopic = "Password Safety";
                lastTopic = "password";
            }

            if (input.Contains("privacy"))
                lastTopic = "privacy";

            if (input.Contains("malware"))
                lastTopic = "malware";

            // Tell me more
            if (input.Contains("tell me more"))
            {
                if (lastTopic == "phishing")
                    return "Phishing attacks impersonate trusted organisations to steal your information.";

                if (lastTopic == "password")
                    return "Use strong passwords and enable Multi-Factor Authentication.";

                if (lastTopic == "privacy")
                    return "Never share personal information on untrusted websites.";

                if (lastTopic == "malware")
                    return "Keep your antivirus software updated to reduce malware infections.";

                return "Please mention a cybersecurity topic first.";
            }

            // Detect sentiment
            string mood = sentiment.DetectSentiment(input);

            if (mood == "worried")
            {
                return "It is okay to feel worried. Cybersecurity awareness helps keep you safe online.";
            }

            if (mood == "angry")
            {
                return "I understand your frustration. Online scams and threats can be stressful.";
            }

            if (mood == "curious")
            {
                return "Curiosity is great! Learning about cybersecurity helps protect your digital life.";
            }

            // Memory recall
            if (input.Contains("what do you remember"))
            {
                return "Your name is " + memory.UserName +
                       " and your favourite topic is " +
                       memory.FavouriteTopic + ".";
            }

            // Default chatbot response
            return responder.GetResponse(input);
        }
    }
}