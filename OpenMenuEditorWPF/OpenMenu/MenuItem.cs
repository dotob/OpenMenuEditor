using System;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace OpenMenuEditor.OpenMenu {
  public class MenuItem : MenuBase {
    public double Price { get; set; }
    public bool Special { get; set; }
    public bool Disabled { get; set; }
    public bool Vegetarian { get; set; }
    public MenuGroup Group { get; set; }

    public override void FromXML(XElement xml) {
      this.UID = (string) xml.Attribute("uid");
      this.Special = xml.Attribute("special") != null;
      this.Vegetarian = xml.Attribute("vegetarian") != null;
      this.Disabled = xml.Attribute("disabled") != null;
      this.Name = (string) xml.Element("menu_item_name");
      this.Description = (string) xml.Element("menu_item_description");
      this.Price = (double) xml.Element("menu_item_price");
    }

    public override XElement ToXML() {
      var ret = new XElement("menu_item");
      ret.Add(new XAttribute("uid", this.UID));
      if (this.Disabled) {
        ret.Add(new XAttribute("disabled", "disabled"));
      }
      if (this.Special) {
        ret.Add(new XAttribute("special", "special"));
      }
      if (this.Vegetarian) {
        ret.Add(new XAttribute("vegetarian", "vegetarian"));
      }
      ret.Add(new XElement("menu_item_name", this.Name));
      ret.Add(new XElement("menu_item_description", this.Description));
      ret.Add(new XElement("menu_item_price", this.Price.ToString(CultureInfo.InvariantCulture)));
      return ret;
    }

    public override void ToHTML(StringBuilder sb) {
      if (!this.Disabled) {
        sb.AppendFormat("\t\t<dl>");
      } else {
        sb.AppendFormat("\t\t<dl class=\"item_tag disabled\">");
      }
      sb.Append("<dt class=\"pepper_\">");
      if (this.Special) {
        sb.Append("<span class=\"item_tag special\">Special</span>");
      }
      if (this.Vegetarian) {
        sb.Append("<span class=\"item_tag vegetarian\">Vegetarian</span>");
      }
      sb.Append(this.Name);
      sb.Append("</dt><dd class=\"price\">");
      sb.Append(this.Price);
      sb.Append("</dd><dd class=\"description\">");
      sb.Append(this.Description);
      sb.Append("</dd></dl>\n");
    }
  }
}