using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DynamicRuling
{
    /// <summary>
    /// serializer
    /// </summary>
    [Serializable]
    public class Serializer
    {
        #region Public Methods

        /// <summary>
        /// serialize the 'convertor'-class (convertor)
        /// </summary>
        /// <param name="file">path to the outputfile</param>
        public static void Serialize(string file)
        {
            using (var stream = File.Open(file, FileMode.Create))
            {
                new BinaryFormatter().Serialize(stream, Converter.GetInstance());
            }
        }

        /// <summary>
        /// serialize a serializable object into a file which has the given path
        /// </summary>
        /// <param name="file">path to the outputfile</param>
        /// <param name="serializable">serializable object</param>
        public static void Serialize(string file, ISerializable serializable)
        {
            using (var stream = File.Open(file, FileMode.Create))
            {
                new BinaryFormatter().Serialize(stream, serializable);
            }
        }

        /// <summary>
        /// deserialize from a file with a given path to a 'convertor'-class
        /// resets the temporary values
        /// </summary>
        /// <param name="file">path to the osl-file to be deserialized</param>
        public static void Deserialize(string file)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            {
                var convertor = (Converter)new BinaryFormatter().Deserialize(stream);
                convertor.TempValues = new Dictionary<String, String>();
                Converter.SetInstance(convertor);
            }         
        }

        /// <summary>
        /// deserialize from a file with a given path to a serializable object (ICheckable in most cases)
        /// </summary>
        /// <param name="file">path to the osl-file to be deserialized</param>
        /// <returns>serializable object which is deserialized from the file</returns>
        public static ISerializable DeserializeTo(string file)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            {
                return (ISerializable)new BinaryFormatter().Deserialize(stream);
            }
        }

        #endregion
    }
}