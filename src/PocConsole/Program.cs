using Limilabs.Client.IMAP;
using Limilabs.Client.POP3;
using Limilabs.Mail;
using Limilabs.Mail.MIME;
using OnboardingStatus.Services;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pop3();
            // Imap();
            // Pop3Client();
            TestClientService();
        }

        private static void GemBox()
        {
            
        }

        private static void Pop3()
        {
            using (Pop3 pop3 = new Pop3())
            {
                //pop3.Connect("outlook.office365.com/owa/phatrasec.com/");
                pop3.Connect("outlook.office365.com");
                pop3.UseBestLogin("suktatha@phatrasec.com", "Thailand_33");
                foreach (string uid in pop3.GetAll())
                {
                    IMail email = new MailBuilder().CreateFromEml(pop3.GetMessageByUID(uid));
                    Console.WriteLine(email.Subject);
                    // save all attachments to disk  
                    email.Attachments.ForEach(mime => 
                    {
                        Console.WriteLine($"attached file:'{mime.SafeFileName}'");
                        //mime.Save(mime.SafeFileName);
                    });

                }
                pop3.Close();
            }
        }

        private static void Imap()
        {
            using (Imap imap = new Imap())
            {
                imap.Connect("outlook.office365.com"); // or ConnectSSL for SSL
                imap.UseBestLogin("suktatha@phatrasec.com", "Thailand_33");

                imap.SelectInbox();

                SimpleImapQuery query = new SimpleImapQuery();
                //query.Subject = "subject to search";
                //query.New = true;
                query.Recent = true;
               // query.Subject = "CRM";

                List<long> uids = imap.Search(Flag.Recent);

                foreach (long uid in uids)
                {
                    IMail email = new MailBuilder()
                        .CreateFromEml(imap.GetMessageByUID(uid));

                    Console.WriteLine(email.Subject);

                    // save all attachments to disk
                    foreach (MimeData mime in email.Attachments)
                    {
                        Console.WriteLine($"attached file:'{mime.SafeFileName}'");
                        //mime.Save(mime.SafeFileName);
                    }
                }
                imap.Close();
            }
        }

        private static void Pop3Client()
        {
            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(hostname: "Outlook.office365.com", port: 995, useSsl:false);

                // Authenticate ourselves towards the server
                client.Authenticate("suktatha@phatrasec.com", "Thailand_33");

                // Get the number of messages in the inbox
                int messageCount = client.GetMessageCount();

                // We want to download all messages
                List<Message> allMessages = new List<Message>(messageCount);

                // Messages are numbered in the interval: [1, messageCount]
                // Ergo: message numbers are 1-based.
                // Most servers give the latest message the highest number
                for (int i = messageCount; i > 0; i--)
                {
                    allMessages.Add(client.GetMessage(i));
                }

                //foreach (var email in allMessages)
                //{
                //    Console.WriteLine(email.);
                //}
            }
        }

        private static void TestClientService()
        {
            var a = new ClientService();
            var result = a.GetClientProcess("eeeeeee");
            Console.Read();

        }
    }
}
