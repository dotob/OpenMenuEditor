using System;
using System.Xml.Linq;

namespace OpenMenuEditor.OpenMenu {
  public class MenuItem : MenuBase {
    public double Price { get; set; }
    public override void FromXML(XElement xml) {
      throw new NotImplementedException();
    }

    public override XElement ToXML() {
      throw new NotImplementedException();
    }
  }
}