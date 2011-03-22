using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace OpenMenuEditor.OpenMenu {
  public class OpenMenuFormat : NotifyPropertyBase, IMenuXMLSerializable {
    public OpenMenuFormat() {
      this.UUID = Guid.NewGuid().ToString();
    }

    public string Version { get; set; }
    public ObservableCollection<Menu> Menus { get; set; }
    public string UUID { get; set; }

    #region IMenuXMLSerializable Members

    public void FromXML(XElement xml) {
      this.UUID = (string) xml.Attribute("uuid");
      XElement itemsElement = xml.Element("menus");
      if (itemsElement != null) {
        this.Menus = new ObservableCollection<Menu>();
        foreach (XElement itemElement in itemsElement.Elements()) {
          var mi = new Menu();
          mi.FromXML(itemElement);
          this.Menus.Add(mi);
        }
      }
    }

    public XElement ToXML() {
      var ret = new XElement("omf");
      if (this.UUID != null) {
        ret.Add(new XAttribute("uuid", this.UUID));
      }
      var items = new XElement("menus");
      foreach (Menu item in this.Menus) {
        items.Add(item.ToXML());
      }
      ret.Add(items);
      return ret;
    }

    #endregion
  }
}