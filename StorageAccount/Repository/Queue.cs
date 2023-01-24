using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace StorageAccount.Repository
{
    public class Queue
    {
        static string connectionString="DefaultEndpointsProtocol=https;AccountName=storageaccount768;AccountKey=fUfsMfDc2nqA3EWiU2bnpxWwcwcqqqUwL11ao5xNHiMd/bpN7tlS2SlRITMsF9SNLKEVMW7HtZqO+AStaE9Edg==;EndpointSuffix=core.windows.net";
        public static async Task<bool>CreateQueue(string queueName){
            if(string.IsNullOrEmpty(queueName)){
                throw new ArgumentNullException("enter queue name");
            }
            try{
                QueueClient container=new QueueClient(connectionString,queueName);
                await container.CreateIfNotExistsAsync();
                if(container.Exists()){
                    Console.WriteLine("Queue Created:"+container.Name);
                    return true;
                }
                else{
                    Console.WriteLine("Check Connection and try again");
                    return false;
                }
            }
            catch(Exception ex){
                throw ex;
            }
        }
        public static async Task InsertMessage(string queueName,string msg){
            if(string.IsNullOrEmpty(queueName)){
                throw new ArgumentNullException("enter queue name");
            }
            QueueClient container=new QueueClient(connectionString,queueName);
            await container.CreateIfNotExistsAsync();
            if(container.Exists()){
                var data=container.SendMessage(msg);
                Console.WriteLine("Message sent Successfully");
            }
            else{
                Console.WriteLine("Queue message not sent");
            }
        }
        public static async Task<PeekedMessage[]>PeekMessage(string queueName){
            QueueClient container=new QueueClient(connectionString,queueName);
            PeekedMessage[] msg=null;
            if(container.Exists()){
                msg=container.PeekMessages(2);
            }
            return msg;
        }
        public static async Task UpdateMessage(string queueName,string data){
            QueueClient container=new QueueClient(connectionString,queueName);
            if(container.Exists()){
                QueueMessage[] msg=container.ReceiveMessages();
                container.UpdateMessage(msg[0].MessageId,msg[0].PopReceipt,data,TimeSpan.FromSeconds(180));
            }
        }
        public static async Task DequeueMessage(string queueName){
            QueueClient container=new QueueClient(connectionString,queueName);
            if(container.Exists()){
                QueueMessage[] msg=container.ReceiveMessages();
                System.Console.WriteLine("Dequeue message"+msg[0].Body);
                container.DeleteMessage(msg[0].MessageId,msg[0].PopReceipt);
            }
        }
        public static async Task DeleteQueue(string queueName){
            QueueClient container=new QueueClient(connectionString,queueName);
            if(container.Exists()){
                await container.DeleteAsync();
            }
        }
    }
}
        
    