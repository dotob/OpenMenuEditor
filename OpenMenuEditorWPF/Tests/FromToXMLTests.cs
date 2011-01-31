using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;
using OpenMenuEditor.OpenMenu;

namespace OpenMenuEditorWPF.Tests {
  [TestFixture]
  public class FromToXMLTests {
    [Test]
    public void MenuGroup_FromXML_ValidObject() {
      var mi = new MenuGroup();
      string xmlIn = "<menu_group uid=\"1\" name=\"myname\">\r\n  <menu_group_description>desc</menu_group_description>\r\n  <menu_items>\r\n    <menu_item uid=\"1\">\r\n      <menu_item_name>item1</menu_item_name>\r\n      <menu_item_description>desc1</menu_item_description>\r\n      <menu_item_price>0</menu_item_price>\r\n    </menu_item>\r\n    <menu_item uid=\"2\">\r\n      <menu_item_name>item2</menu_item_name>\r\n      <menu_item_description>desc2</menu_item_description>\r\n      <menu_item_price>0</menu_item_price>\r\n    </menu_item>\r\n  </menu_items>\r\n</menu_group>";
      XElement xmlElem = XElement.Load(new StringReader(xmlIn));
      mi.FromXML(xmlElem);
      Assert.AreEqual("myname", mi.Name);
      Assert.AreEqual("desc", mi.Description);
      Assert.AreEqual("1", mi.UID);
      Assert.AreEqual(2, mi.Items.Count);
    }

    [Test]
    public void MenuGroup_ToXML_ValidXML() {
      var mi1 = new MenuItem {UID = "1", Description = "desc1", Name = "item1"};
      var mi2 = new MenuItem {UID = "2", Description = "desc2", Name = "item2"};

      var mg = new MenuGroup {UID = "1", Description = "desc", Name = "myname", Items = new ObservableCollection<MenuItem> {mi1, mi2}};

      string xml = mg.ToXML().ToString();
      Assert.AreEqual("<menu_group uid=\"1\" name=\"myname\">\r\n  <menu_group_description>desc</menu_group_description>\r\n  <menu_items>\r\n    <menu_item uid=\"1\">\r\n      <menu_item_name>item1</menu_item_name>\r\n      <menu_item_description>desc1</menu_item_description>\r\n      <menu_item_price>0</menu_item_price>\r\n    </menu_item>\r\n    <menu_item uid=\"2\">\r\n      <menu_item_name>item2</menu_item_name>\r\n      <menu_item_description>desc2</menu_item_description>\r\n      <menu_item_price>0</menu_item_price>\r\n    </menu_item>\r\n  </menu_items>\r\n</menu_group>", xml);
    }

    [Test]
    public void MenuItem_FromXML_ValidObject() {
      var mi = new MenuItem();
      string xmlIn = "<menu_item uid=\"1\" disabled=\"disabled\" special=\"special\">\r\n  <menu_item_name>item1</menu_item_name>\r\n  <menu_item_description>desc1</menu_item_description>\r\n  <menu_item_price>0</menu_item_price>\r\n</menu_item>";
      XElement xmlElem = XElement.Load(new StringReader(xmlIn));
      mi.FromXML(xmlElem);
      Assert.AreEqual("item1", mi.Name);
      Assert.AreEqual("desc1", mi.Description);
      Assert.AreEqual(0d, mi.Price);
      Assert.IsTrue(mi.Disabled);
      Assert.IsTrue(mi.Special);
      Assert.IsFalse(mi.Vegetarian);
    }

    [Test]
    public void MenuItem_ToXML_ValidXML() {
      var mi1 = new MenuItem {UID = "1", Description = "desc1", Name = "item1"};
      string xml = mi1.ToXML().ToString();
      Assert.AreEqual("<menu_item uid=\"1\">\r\n  <menu_item_name>item1</menu_item_name>\r\n  <menu_item_description>desc1</menu_item_description>\r\n  <menu_item_price>0</menu_item_price>\r\n</menu_item>", xml);
    }

    [Test]
    public void Menu_FromXML_ValidObject() {
      var mi = new Menu();
      string xmlIn = "<menu uid=\"1\" name=\"alacarte\" currency_symbol=\"EUR\">\r\n  <menu_description>desc</menu_description>\r\n  <menu_groups>\r\n    <menu_group uid=\"1\" name=\"myname\">\r\n      <menu_group_description>desc</menu_group_description>\r\n      <menu_items>\r\n        <menu_item uid=\"1\">\r\n          <menu_item_name>item1</menu_item_name>\r\n          <menu_item_description>desc1</menu_item_description>\r\n          <menu_item_price>0</menu_item_price>\r\n        </menu_item>\r\n        <menu_item uid=\"2\">\r\n          <menu_item_name>item2</menu_item_name>\r\n          <menu_item_description>desc2</menu_item_description>\r\n          <menu_item_price>0</menu_item_price>\r\n        </menu_item>\r\n      </menu_items>\r\n    </menu_group>\r\n    <menu_group uid=\"2\" name=\"myname\">\r\n      <menu_group_description>desc</menu_group_description>\r\n      <menu_items>\r\n        <menu_item uid=\"1\">\r\n          <menu_item_name>item1</menu_item_name>\r\n          <menu_item_description>desc1</menu_item_description>\r\n          <menu_item_price>0</menu_item_price>\r\n        </menu_item>\r\n        <menu_item uid=\"2\">\r\n          <menu_item_name>item2</menu_item_name>\r\n          <menu_item_description>desc2</menu_item_description>\r\n          <menu_item_price>0</menu_item_price>\r\n        </menu_item>\r\n      </menu_items>\r\n    </menu_group>\r\n  </menu_groups>\r\n</menu>";
      XElement xmlElem = XElement.Load(new StringReader(xmlIn));
      mi.FromXML(xmlElem);
      Assert.AreEqual("alacarte", mi.Name);
      Assert.AreEqual("desc", mi.Description);
      Assert.AreEqual("EUR", mi.Currency);
      Assert.AreEqual("1", mi.UID);
      Assert.AreEqual(2, mi.Groups.Count);
      foreach (MenuGroup menuGroup in mi.Groups) {
        Assert.AreEqual(2, menuGroup.Items.Count);
      }
    }

    [Test]
    public void Menu_ToXML_ValidXML() {
      var mi1 = new MenuItem {UID = "1", Description = "desc1", Name = "item1"};
      var mi2 = new MenuItem {UID = "2", Description = "desc2", Name = "item2"};

      var mg1 = new MenuGroup {UID = "1", Description = "desc", Name = "myname", Items = new ObservableCollection<MenuItem> {mi1, mi2}};
      var mg2 = new MenuGroup {UID = "2", Description = "desc", Name = "myname", Items = new ObservableCollection<MenuItem> {mi1, mi2}};

      var m = new Menu {UID = "1", Name = "alacarte", Description = "desc", Currency = "EUR", Groups = new ObservableCollection<MenuGroup> {mg1, mg2}};
      string xml = m.ToXML().ToString();
      Assert.AreEqual("<menu uid=\"1\" name=\"alacarte\" currency_symbol=\"EUR\">\r\n  <menu_description>desc</menu_description>\r\n  <menu_groups>\r\n    <menu_group uid=\"1\" name=\"myname\">\r\n      <menu_group_description>desc</menu_group_description>\r\n      <menu_items>\r\n        <menu_item uid=\"1\">\r\n          <menu_item_name>item1</menu_item_name>\r\n          <menu_item_description>desc1</menu_item_description>\r\n          <menu_item_price>0</menu_item_price>\r\n        </menu_item>\r\n        <menu_item uid=\"2\">\r\n          <menu_item_name>item2</menu_item_name>\r\n          <menu_item_description>desc2</menu_item_description>\r\n          <menu_item_price>0</menu_item_price>\r\n        </menu_item>\r\n      </menu_items>\r\n    </menu_group>\r\n    <menu_group uid=\"2\" name=\"myname\">\r\n      <menu_group_description>desc</menu_group_description>\r\n      <menu_items>\r\n        <menu_item uid=\"1\">\r\n          <menu_item_name>item1</menu_item_name>\r\n          <menu_item_description>desc1</menu_item_description>\r\n          <menu_item_price>0</menu_item_price>\r\n        </menu_item>\r\n        <menu_item uid=\"2\">\r\n          <menu_item_name>item2</menu_item_name>\r\n          <menu_item_description>desc2</menu_item_description>\r\n          <menu_item_price>0</menu_item_price>\r\n        </menu_item>\r\n      </menu_items>\r\n    </menu_group>\r\n  </menu_groups>\r\n</menu>", xml);
    }

    [Test]
    public void OMF_FromXML_ValidObject() {
      var mi = new OpenMenuFormat();
      string xmlIn = "<omf uuid=\"74c2b466-e360-42f0-9060-9cc7765e196a\">\r\n  <menus>\r\n    <menu uid=\"1\" name=\"alacarte\" currency_symbol=\"EUR\">\r\n      <menu_description>desc</menu_description>\r\n      <menu_groups>\r\n        <menu_group uid=\"1\" name=\"myname\">\r\n          <menu_group_description>desc</menu_group_description>\r\n          <menu_items>\r\n            <menu_item uid=\"1\">\r\n              <menu_item_name>item1</menu_item_name>\r\n              <menu_item_description>desc1</menu_item_description>\r\n              <menu_item_price>0</menu_item_price>\r\n            </menu_item>\r\n            <menu_item uid=\"2\">\r\n              <menu_item_name>item2</menu_item_name>\r\n              <menu_item_description>desc2</menu_item_description>\r\n              <menu_item_price>0</menu_item_price>\r\n            </menu_item>\r\n          </menu_items>\r\n        </menu_group>\r\n        <menu_group uid=\"2\" name=\"myname\">\r\n          <menu_group_description>desc</menu_group_description>\r\n          <menu_items>\r\n            <menu_item uid=\"1\">\r\n              <menu_item_name>item1</menu_item_name>\r\n              <menu_item_description>desc1</menu_item_description>\r\n              <menu_item_price>0</menu_item_price>\r\n            </menu_item>\r\n            <menu_item uid=\"2\">\r\n              <menu_item_name>item2</menu_item_name>\r\n              <menu_item_description>desc2</menu_item_description>\r\n              <menu_item_price>0</menu_item_price>\r\n            </menu_item>\r\n          </menu_items>\r\n        </menu_group>\r\n      </menu_groups>\r\n    </menu>\r\n  </menus>\r\n</omf>";
      XElement xmlElem = XElement.Load(new StringReader(xmlIn));
      mi.FromXML(xmlElem);
      Assert.AreEqual("74c2b466-e360-42f0-9060-9cc7765e196a", mi.UUID);
      Assert.AreEqual(1, mi.Menus.Count);
      foreach (Menu menu in mi.Menus) {
        Assert.AreEqual(2, menu.Groups.Count);
        foreach (MenuGroup menuGroup in menu.Groups) {
          Assert.AreEqual(2, menuGroup.Items.Count);
        }
      }
    }

    [Test]
    public void OMF_FromRealXML_ValidObject() {
      var mi = new OpenMenuFormat();
      string xmlIn = Properties.Resources.alacarte_menu;
      XElement xmlElem = XElement.Load(new StringReader(xmlIn));
      mi.FromXML(xmlElem);
      Assert.AreEqual("df8cb20e-297c-11e0-91d7-0018512e6b26", mi.UUID);
      Assert.AreEqual(1, mi.Menus.Count);
      foreach (Menu menu in mi.Menus) {
        Assert.AreEqual(3, menu.Groups.Count);
        foreach (MenuGroup menuGroup in menu.Groups) {
          Assert.AreEqual(1, menuGroup.Items.Count);
        }
      }
    }

    [Test]
    public void OMF_ToXML_ValidXML() {
      var mi1 = new MenuItem {UID = "1", Description = "desc1", Name = "item1"};
      var mi2 = new MenuItem {UID = "2", Description = "desc2", Name = "item2"};

      var mg1 = new MenuGroup {UID = "1", Description = "desc", Name = "myname", Items = new ObservableCollection<MenuItem> {mi1, mi2}};
      var mg2 = new MenuGroup {UID = "2", Description = "desc", Name = "myname", Items = new ObservableCollection<MenuItem> {mi1, mi2}};

      var m = new Menu {UID = "1", Name = "alacarte", Description = "desc", Currency = "EUR", Groups = new ObservableCollection<MenuGroup> {mg1, mg2}};

      var omf = new OpenMenuFormat {UUID = "74c2b466-e360-42f0-9060-9cc7765e196a", Menus = new ObservableCollection<Menu> {m}};
      string xml = omf.ToXML().ToString();
      Assert.AreEqual("<omf uuid=\"74c2b466-e360-42f0-9060-9cc7765e196a\">\r\n  <menus>\r\n    <menu uid=\"1\" name=\"alacarte\" currency_symbol=\"EUR\">\r\n      <menu_description>desc</menu_description>\r\n      <menu_groups>\r\n        <menu_group uid=\"1\" name=\"myname\">\r\n          <menu_group_description>desc</menu_group_description>\r\n          <menu_items>\r\n            <menu_item uid=\"1\">\r\n              <menu_item_name>item1</menu_item_name>\r\n              <menu_item_description>desc1</menu_item_description>\r\n              <menu_item_price>0</menu_item_price>\r\n            </menu_item>\r\n            <menu_item uid=\"2\">\r\n              <menu_item_name>item2</menu_item_name>\r\n              <menu_item_description>desc2</menu_item_description>\r\n              <menu_item_price>0</menu_item_price>\r\n            </menu_item>\r\n          </menu_items>\r\n        </menu_group>\r\n        <menu_group uid=\"2\" name=\"myname\">\r\n          <menu_group_description>desc</menu_group_description>\r\n          <menu_items>\r\n            <menu_item uid=\"1\">\r\n              <menu_item_name>item1</menu_item_name>\r\n              <menu_item_description>desc1</menu_item_description>\r\n              <menu_item_price>0</menu_item_price>\r\n            </menu_item>\r\n            <menu_item uid=\"2\">\r\n              <menu_item_name>item2</menu_item_name>\r\n              <menu_item_description>desc2</menu_item_description>\r\n              <menu_item_price>0</menu_item_price>\r\n            </menu_item>\r\n          </menu_items>\r\n        </menu_group>\r\n      </menu_groups>\r\n    </menu>\r\n  </menus>\r\n</omf>", xml);
    }
  }
}