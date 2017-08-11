﻿using UnityEngine; 
using System.IO; 
using System.Xml; 
using System.Xml.Serialization; 
using System.Collections; 
using System; 
 
public class XML : MonoBehaviour
{
    static public void Serialize(object charac, string filename)
    {
        if (File.Exists("caract.xml") == false)
        {
            File.Create("caract.xml").Dispose();
        }
        XmlSerializer serializer = new XmlSerializer(charac.GetType());
        StreamWriter writer = new StreamWriter(filename);
        serializer.Serialize(writer, charac);
        writer.Close();
    }

    static public T Deserialize<T>(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        if (File.Exists(filename) == false)
        {
            File.Create(filename);
        }
        TextReader reader = new StreamReader(filename);
        T deserialized = (T)serializer.Deserialize(reader);
        reader.Close();

        return deserialized;
    }
}
