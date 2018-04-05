using System;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace device_robot
{
    public class PublishData
    {
        private string topicsPath;
        private string deviceIP;
        private string deviceURL;
        private string deviceID;

        public PublishData(string topicsPath, string deviceIP, string deviceID)
        {
            this.topicsPath = topicsPath;
            this.deviceIP = deviceIP;
            this.deviceURL = "tcp://" + deviceIP + ":1883";
            this.deviceID = deviceID;
        }

        public void createPublishDataThreads(){
            string jsonString = System.IO.File.ReadAllText(topicsPath);
            JObject topicsJSON = JObject.Parse(jsonString);
            JArray vector = (JArray) topicsJSON.GetValue("topics");

            //JObject iterator = (JObject) vector.First;
            //JToken token;
            Console.WriteLine(vector.Count);

            MQTTclient pubData = new MQTTclient();
            pubData.connect(deviceIP, deviceID + "DataPublisher");
            int i = 1;
            foreach (var item in vector.Children()){
                JToken t = item.SelectToken("url");
                string url = t.ToString();

                if (url == deviceURL)
                {
                    JToken token = item.SelectToken("topic");
                    Console.WriteLine(token.ToString());
                    Thread thread = new Thread(() => attributePublish(token.ToString(), pubData, i * 2000));
                    thread.Start();
                }
                i++;
            }
        }

        public void attributePublish(string topic, MQTTclient pubData, int sleep){
            Random rd = new Random();

            while(true){
                int value = rd.Next(100);
                Console.WriteLine("Topic: " + topic);
                pubData.publish(topic, value.ToString());

                System.Threading.Thread.Sleep(sleep);
            }

        }
    }
}
