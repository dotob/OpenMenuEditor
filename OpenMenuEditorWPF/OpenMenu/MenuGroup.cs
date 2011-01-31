using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace OpenMenuEditor.OpenMenu {
  public class MenuGroup : MenuBase {
    public ObservableCollection<MenuItem> Items { get; set; }
    public Menu Menu { get; set; }


    public MenuGroup() {
      Items = new ObservableCollection<MenuItem>();
    }

    public override void FromXML(XElement xml) {
      this.Name = (string) xml.Attribute("name");
      this.Description = (string)xml.Element("menu_group_description");
      this.UID = (string)xml.Attribute("uid");

      XElement itemsElement = xml.Element("menu_items");
      if (itemsElement != null) {
        this.Items = new ObservableCollection<MenuItem>();
        foreach (XElement itemElement in itemsElement.Elements()) {
          var mi = new MenuItem();
          mi.Group = this;
          mi.FromXML(itemElement);
          this.Items.Add(mi);
        }
      }
    }

    public override XElement ToXML() {
      var ret = new XElement("menu_group");
      ret.Add(new XAttribute("uid", this.UID));
      ret.Add(new XAttribute("name", this.Name));
      ret.Add(new XElement("menu_group_description", this.Description));
      var items = new XElement("menu_items");
      foreach (MenuItem item in this.Items) {
        items.Add(item.ToXML());
      }
      ret.Add(items);
      return ret;
    }
  }
}