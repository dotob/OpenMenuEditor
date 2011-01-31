using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace OpenMenuEditor.OpenMenu {
  public class Menu : MenuBase {
    public string Currency { get; set; }
    public ObservableCollection<MenuGroup> Groups { get; set; }

    public Menu() {
      Groups = new ObservableCollection<MenuGroup>();
    }

    public override void FromXML(XElement xml) {
      this.Name = (string)xml.Attribute("name");
      this.Description = (string)xml.Element("menu_description");
      this.UID = (string)xml.Attribute("uid");
      this.Currency = (string)xml.Attribute("currency_symbol");

      XElement itemsElement = xml.Element("menu_groups");
      if (itemsElement != null) {
        this.Groups = new ObservableCollection<MenuGroup>();
        foreach (XElement itemElement in itemsElement.Elements()) {
          var mi = new MenuGroup();
          mi.Menu = this;
          mi.FromXML(itemElement);
          this.Groups.Add(mi);
        }
      }
    }

    public override XElement ToXML() {
      var ret = new XElement("menu");
      ret.Add(new XAttribute("uid", this.UID));
      ret.Add(new XAttribute("name", this.Name));
      ret.Add(new XAttribute("currency_symbol", this.Currency));
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