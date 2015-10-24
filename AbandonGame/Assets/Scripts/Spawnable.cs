using System.Xml;
using System.Xml.Serialization;

public class Spawnable{

    [XmlAttribute("name")]
	public string name;
	
	[XmlAttribute("initialSpawnProb")]
	public float initialSpawnProb;

	public int failedSpawnCount = 0;
	
}
