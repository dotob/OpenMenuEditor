using System.Xml.Linq;

namespace OpenMenuEditor.OpenMenu {
  public abstract class MenuBase {
    public string UID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public abstract void FromXML(XElement xml);
    public abstract XElement ToXML();
  }
}