using System.Xml.Linq;

namespace OpenMenuEditor.OpenMenu {
  public interface IMenuXMLSerializable {
    void FromXML(XElement xml);
    XElement ToXML();
  }

  public abstract class MenuBase : IMenuXMLSerializable {
    public string UID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public abstract void FromXML(XElement xml);
    public abstract XElement ToXML();
  }
}