using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabitMQ.subcriber
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory();
            factory.Uri = new Uri("	amqps://szxgbist:ivBmeATJumRHZuj5Jsxfg1QuXLLUUmgY@sparrow.rmq.cloudamqp.com/szxgbist");
            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            //subscriber bu kuyruk olusmadan ayaga kalkarsa hata alırız
            //gecmis oldugumuz parametlerin aynı olduguna dikkat edin
            //channel.QueueDeclare("hello Queue", true, false, false);

            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume("hello Queue",false,consumer);



            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message=Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Gelen mesaj:" + message);
                
                //rabbitmq haberdar ediyoruz.
                channel.BasicAck(e.DeliveryTag, false);

            };




            Console.ReadLine();

        }

      
    }
}
