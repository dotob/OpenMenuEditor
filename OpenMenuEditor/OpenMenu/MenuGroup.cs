using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenMenuEditor.OpenMenu {
  public class MenuGroup : MenuBase {
    public List<MenuItem> Items { get; set; }

    public override void FromXML(XElement xml) {
      throw new NotImplementedException();
    }

    public override XElement ToXML() {
      var ret = new XElement("menu_group");
      ret.Add(new XAttribute("uid", this.UID));
      ret.Add(new XAttribute("name", this.Name));
      ret.Add(new XElement("menu_group_description", this.Description));
      var items = new XElement("menu_items");
      foreach (var item in this.Items) {
        items.Add(item.ToXML());
      }
      ret.Add(items);
      return ret;
    }
  }
}