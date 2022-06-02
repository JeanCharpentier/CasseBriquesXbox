using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CasseBriques
{
    [DataContract]
    public class JsonManager:IJson
    {
        [DataMember]
        public string map;

        public JsonManager()
        {
            ServicesLocator.AddService<IJson>(this);
        }

        public JsonManager ReadJson(string pFile)
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(pFile)));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(JsonManager));
            JsonManager myJSON = (JsonManager)ser.ReadObject(stream);

            return myJSON;
        }
    }
}

