using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("SpawnableCollection")]
public class SpawnableCollection
{
	[XmlArray("Spawnables"),XmlArrayItem("Spawnable")]
	public Spawnable[] spawnables;
	
	public void Save(string path)
	{
		var serializer = new XmlSerializer(typeof(SpawnableCollection));
		using(var stream = new FileStream(path, FileMode.Create))
		{
			serializer.Serialize(stream, this);
		}
	}
	
	public SpawnableCollection Load(string path)
	{
		path = Path.Combine (Application.dataPath, path);
		var serializer = new XmlSerializer(typeof(SpawnableCollection));
		var stream = new FileStream(path, FileMode.Open);
		var container = serializer.Deserialize(stream) as SpawnableCollection;
		stream.Close();
		return container;
	}
	
	//Loads the xml directly from the given string. Useful in combination with www.text.
	public SpawnableCollection LoadFromText(string text) 
	{
		var serializer = new XmlSerializer(typeof(SpawnableCollection));
		return serializer.Deserialize(new StringReader(text)) as SpawnableCollection;
	}
}