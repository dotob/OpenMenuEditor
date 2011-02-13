using System;
using System.IO;
using System.Xml.Linq;
using OpenMenuEditor.OpenMenu;
using OpenMenuEditorWPF.Properties;

namespace OpenMenuEditorWPF {
  public class MainViewModel {
    private readonly string fileToStoreLocal;
    private readonly string menuFileName;
    private bool goOnline = true;

    public MainViewModel() {
      this.OpenMenu = new OpenMenuFormat();
      // create dir for data
      XElement xmlElem;
      string dirToStore = "FringshausOpenMenu";
      string dirToStoreCombined = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), dirToStore);
      if (!Directory.Exists(dirToStoreCombined)) {
        Directory.CreateDirectory(dirToStoreCombined);
      }

      // for testing no need to go online
      if (this.goOnline) {
        this.menuFileName = "alacarte.xml";
        this.fileToStoreLocal = Path.Combine(dirToStoreCombined, this.menuFileName);
        FTPTools.Download("dotob.de", 21, this.menuFileName, this.fileToStoreLocal, "fringshaus", "fringshaus", false);

        xmlElem = XElement.Load(this.fileToStoreLocal);
      }
      else {
        xmlElem = XElement.Load(new StringReader(Resources.alacarte_menu));
      }
      this.OpenMenu.FromXML(xmlElem);
    }

    public OpenMenuFormat OpenMenu { get; set; }

    public MenuBase SelectedItem { get; set; }
    public Menu SelectedMenu { get; set; }

    public void NewMenu() {
      this.OpenMenu.Menus.Add(new Menu {Name = "New Menu"});
    }

    public void NewMenuGroup() {
      this.SelectedMenu.Groups.Add(new MenuGroup {Menu = this.SelectedMenu});
    }

    public void NewMenuItem() {
      var mg = this.SelectedItem as MenuGroup;
      if (mg != null) {
        mg.Items.Add(new MenuItem {Group = mg});
      }
    }

    public void DeleteSelectedElement() {
      if (this.SelectedItem is MenuGroup) {
        var mg = (MenuGroup) this.SelectedItem;
        mg.Menu.Groups.Remove(mg);
      }
      else if (this.SelectedItem is MenuItem) {
        var mi = (MenuItem) this.SelectedItem;
        mi.Group.Items.Remove(mi);
      }
    }

    public void MoveSelectedElementUp() {}

    public void MoveSelectedElementDown() {}

    public void Save() {
      XElement xElement = this.OpenMenu.ToXML();
      if (this.goOnline) {
        xElement.Save(this.fileToStoreLocal);
        FTPTools.Upload("dotob.de", 21, this.fileToStoreLocal, this.menuFileName, "fringshaus", "fringshaus", false);
      }
      else {
        xElement.Save(this.fileToStoreLocal);
      }
    }
  }
}