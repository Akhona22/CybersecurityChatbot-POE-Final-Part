using System;
using System.Windows;

namespace CybersecurityChatbot
{
    public partial class MainWindow : Window
    {
        // Create ChatBot object
        private ChatBot chatbot;

        // Create AudioPlayer object
        private AudioPlayer audio;

        // Create TaskRepository object
        private TaskRepository taskRepository;

        public MainWindow()
        {
            InitializeComponent();

            // Create chatbot object
            chatbot = new ChatBot();

            // Create audio object
            audio = new AudioPlayer();

            // Create TaskRepository object
            taskRepository = new TaskRepository();

            // Play greeting audio
            audio.PlayGreeting();

            // Connect button click events
            SendButton.Click += SendButton_Click;
            ShowTasksButton.Click += ShowTasksButton_Click;
            CompleteTaskButton.Click += CompleteTaskButton_Click;
            DeleteTaskButton.Click += DeleteTaskButton_Click;
            QuizButton.Click += QuizButton_Click;
        }

        // SEND BUTTON
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = UserInput.Text;

            string botResponse = chatbot.ProcessInput(userMessage);

            ChatDisplay.Text += "You: " + userMessage + "\n";
            ChatDisplay.Text += "Bot: " + botResponse + "\n\n";

            UserInput.Clear();
        }

        // SHOW TASKS BUTTON
        private void ShowTasksButton_Click(object sender, RoutedEventArgs e)
        {
            var tasks = taskRepository.GetAllTasks();

            if (tasks.Count == 0)
            {
                ChatDisplay.Text += "Bot: There are no tasks.\n\n";
                return;
            }

            ChatDisplay.Text += "========== TASK LIST ==========\n";

            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "Completed" : "Pending";

                ChatDisplay.Text +=
                    $"{task.TaskID}. {task.Title} [{status}]\n";
            }

            ChatDisplay.Text += "\n";
        }

        // COMPLETE TASK BUTTON
        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                taskRepository.MarkTaskCompleted(1);

                ChatDisplay.Text += "Bot: Task 1 marked as completed.\n\n";
            }
            catch (Exception ex)
            {
                ChatDisplay.Text += "Error: " + ex.Message + "\n\n";
            }
        }

        // DELETE TASK BUTTON
        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                taskRepository.DeleteTask(1);

                ChatDisplay.Text += "Bot: Task 1 deleted successfully.\n\n";
            }
            catch (Exception ex)
            {
                ChatDisplay.Text += "Error: " + ex.Message + "\n\n";
            }
        }

        // QUIZ BUTTON
        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            ChatDisplay.Text +=
                "Bot: Cybersecurity Quiz\n";
            ChatDisplay.Text +=
                "Question 1:\n";
            ChatDisplay.Text +=
                "What should you do if you receive a suspicious email?\n";
            ChatDisplay.Text +=
                "A) Click the link\n";
            ChatDisplay.Text +=
                "B) Ignore or report it\n";
            ChatDisplay.Text +=
                "C) Reply with your password\n\n";
        }
    }
}