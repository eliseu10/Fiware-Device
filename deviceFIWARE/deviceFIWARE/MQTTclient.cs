using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace device_robot
{
    public class MQTTclient
    {
        private MqttClient client;

        public void connect(String url, string id){
            client = new MqttClient(url);
            client.Connect(id);
        }

        public void publish(string topic, String msgString){
            byte[] msg = Encoding.ASCII.GetBytes(msgString);
            System.Threading.Thread.Sleep(500);
            client.MqttMsgPublished += client_MqttMsgPublished;
            client.Publish(topic, msg,MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,false);
        }

        void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Console.WriteLine("MessageId = " + e.MessageId + " Published = " + e.IsPublished);
        }

        public void subscribe(string[] topicString){
            client.Subscribe(topicString, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);
        }

        public void disconnect()
        {
            client.Disconnect();
        }
    }
}
