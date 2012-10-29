using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml.Linq;
using Tamir.SharpSsh.java.lang;

namespace OpenMenuEditor.OpenMenu {
  public class OpenMenuFormat : NotifyPropertyBase, IMenuXMLSerializable
  {
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

    public string ToHTML() {
      var sb = new StringBuilder();
      sb.Append("<!DOCTYPE html>\n<html dir=\"ltr\" lang=\"de-DE\">\n<head>\n<meta charset=\"UTF-8\" />\n");
      sb.Append("<link rel='stylesheet' id='OpenMenu-Template-Default-css'  href='http://fringshaus.com/wp-content/plugins/open-menu/templates/default/styles/style.css?ver=3.0.3' type='text/css' media='all' /> ");
      sb.Append("</head>");
      sb.Append("<body>");
      foreach (var menu in Menus) {
        menu.ToHTML(sb);
      }
      sb.Append("</body>");
      sb.Append("</html>");
      return sb.ToString();
    }
  }
}