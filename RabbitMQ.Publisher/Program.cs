using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("	amqps://szxgbist:ivBmeATJumRHZuj5Jsxfg1QuXLLUUmgY@sparrow.rmq.cloudamqp.com/szxgbist");
            using var connection =factory.CreateConnection();

            var channel=connection.CreateModel();
            //durable:true dedim cünkü restart atıldıgında silinsin istemiyorum
            //exculasive:false diyorum true dersem sadece buradaki channeldan baglantı kabul eder
            //autoDelete: false dedim son subscriber
            channel.QueueDeclare("hello Queue",true, false,false);
            Enumerable.Range(1, 50).ToList().ForEach(x =>
             {
                 //rabbitMQ ya dosyalarını bytes dizisine cevirerek gönderebilirsin
                 string message = $"Message {x}";
                 var messageBody = Encoding.UTF8.GetBytes(message);
                 // default exchange olacak
                 channel.BasicPublish(string.Empty, "hello Queue", null, messageBody);

                 Console.WriteLine($"mesajınız gönderilmistir: {message} ");

             });

           
            Console.ReadLine();


        }
    }
}
