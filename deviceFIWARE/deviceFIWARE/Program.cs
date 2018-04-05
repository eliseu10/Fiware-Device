using System;
using Newtonsoft.Json.Linq;

namespace device_robot
{
    class MainClass
    {
        private static string TOPIC_REGIST = "/registdevice";
        private static string TOPIC_GETTOPICS = "/gettopics";
        private static String REGIST_PATH = "/home/eliseu/Dropbox/c-sharp-projects/regist.json";
        private static string TOPICS_PATH = "/home/eliseu/Dropbox/c-sharp-projects/topics.json";

        public static void Main(string[] args)
        {
            DiscoverySniffer sniffer = new DiscoverySniffer();
            sniffer.discoverIP();
            String localIP = sniffer.GetLocalIPAddress();

            string jsonString = System.IO.File.ReadAllText(REGIST_PATH);
            JObject registJSON = JObject.Parse(jsonString);
            JToken deviceID = registJSON.GetValue("device_id");

            //publish the topic for the regist
            MQTTclient mqtt = new MQTTclient();
            mqtt.connect(localIP, deviceID.ToString());
            mqtt.publish(TOPIC_REGIST, registJSON.ToString());
            //mqtt.disconnect();

            //subscribe the list topic
            string[] topicStrings = { TOPIC_GETTOPICS };
            SubscribeTopicsList topSub = new SubscribeTopicsList(TOPICS_PATH, localIP, topicStrings);
            topSub.getList();

            System.Threading.Thread.Sleep(10000);
            topSub.disconnect();

            PublishData pubData = new PublishData(TOPICS_PATH, localIP, deviceID.ToString());
            pubData.createPublishDataThreads();
        }
    }
}
