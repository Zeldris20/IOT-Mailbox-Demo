using System;
using System.Threading;
using System.Threading.Tasks;

class MailboxSensor
{
    public event EventHandler<string> MailboxStatusChanged;

    public void SimulateMailbox()
    {
        Random random = new Random();
        int unreadCount = 0;

        while (true)
        {
            // Simulate the mailbox status (open or closed)
            bool isMailboxOpen = random.Next(2) == 0;

            // Raise the event to notify the status change
            OnMailboxStatusChanged(isMailboxOpen);

            if (isMailboxOpen)
            {
                // Simulate the arrival of a new email
                Console.WriteLine($"New email received! Subject: Unread Email #{++unreadCount}");

                // Wait for 5 seconds before continuing
                Task.Delay(5000).Wait();

                if (unreadCount > 0 && Console.KeyAvailable)
                {
                    Console.WriteLine($"Email content: {GenerateRandomText()}");
                    unreadCount = 0; // Reset unread count after reading the email
                    Console.ReadKey(intercept: true); // Consume the Enter key
                }
            }

            // Simulate delay between updates
            Thread.Sleep(5000); // Update every 5 seconds
        }
    }

    private string GenerateRandomText()
    {
        string[] possibleTexts = {
            "Hello, how are you?",
            "Meeting at 2 PM tomorrow.",
            "Don't forget to buy groceries!",
            "Interesting article: [link]",
            "Your package has been delivered."
        };

        Random random = new Random();
        int index = random.Next(possibleTexts.Length);
        return possibleTexts[index];
    }

    protected virtual void OnMailboxStatusChanged(bool isMailboxOpen)
    {
        // Notify subscribers (e.g., the main program)
        MailboxStatusChanged?.Invoke(this, isMailboxOpen ? "Mailbox is open" : "Mailbox is closed");
    }
}

class Program
{
    static void Main()
    {
        // Create an instance of the MailboxSensor
        MailboxSensor mailboxSensor = new MailboxSensor();

        // Subscribe to the MailboxStatusChanged event
        mailboxSensor.MailboxStatusChanged += MailboxStatusChangedHandler;

        // Start simulating the mailbox
        mailboxSensor.SimulateMailbox();
    }

    private static void MailboxStatusChangedHandler(object sender, string e)
    {
        // Handle the mailbox status change (e.g., display in console)
        Console.WriteLine($"[{DateTime.Now}] {e}");
    }
}
