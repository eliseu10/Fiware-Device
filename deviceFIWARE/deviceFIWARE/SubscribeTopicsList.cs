using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace device_robot
{
    public class SubscribeTopicsList
    {
        private string topicsPath;
        private string deviceIP;
        private string[] topics;
        private MqttClient client;

        public SubscribeTopicsList(string topicsPath, string deviceIP, string[] topics)
        {
            this.topicsPath = topicsPath;
            this.deviceIP = deviceIP;
            this.topics = topics;
        }

        public void getList(){
            connect(deviceIP, "deviceGetTopics");
            subscribe(topics);
        }

        private void connect(String url, string id)
        {
            client = new MqttClient(url);
            client.Connect(id);
        }

        private void subscribe(string[] topicString)
        {
            client.Subscribe(topicString, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Message);
            Console.WriteLine("Received = " + message + " on topic " + e.Topic);
            saveTopicList(message);
        }

        private void saveTopicList(string listString){
            System.IO.File.WriteAllText(topicsPath, listString);
        }

        public void disconnect()
        {
            client.Disconnect();
        }
    }
}
