using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenMenuEditor.OpenMenu {
  public class Menu : MenuBase {
    public string Currency { get; set; }
    public List<MenuGroup> Groups { get; set; }
    public override void FromXML(XElement xml) {
      throw new NotImplementedException();
    }

    public override XElement ToXML() {
      var ret = new XElement("menu");
      ret.Add(new XAttribute("uid", this.UID));
      ret.Add(new XAttribute("name", this.Name));
      ret.Add(new XElement("menu_description", this.Description));
      var items = new XElement("menu_groups");
      foreach (var item in this.Groups) {
        items.Add(item.ToXML());
      }
      ret.Add(items);
      return ret;
    }
  }
}