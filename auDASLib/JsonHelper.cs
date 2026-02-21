#region define
using Newtonsoft.Json;
#endregion


/*************************************************
 * Author: Jinwang DU
 * Description: Used to Serialize and DeSerialize
 * between string and Json object
 * ************************************************/
namespace auDASLib
{
    public class JsonHelper
    {
        /// <summary>
        /// Serialize from Object to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObject"></param>
        /// <returns></returns>
        public static string Serialize<T>(T messageObject)
        {
            string output = JsonConvert.SerializeObject(messageObject);

            return output;
        }

        /// <summary>
        /// DeSerialize from string to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectstring"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(string objectstring)
        {
            return JsonConvert.DeserializeObject<T>(objectstring);
        }
    }
}
