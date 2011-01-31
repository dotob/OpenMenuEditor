using System;
using System.IO;
using System.Xml.Linq;
using OpenMenuEditor.OpenMenu;

namespace OpenMenuEditorWPF {
  public class MainViewModel {
    private string fileToStoreLocal;
    private string menuFileName;

    public MainViewModel() {
      this.OpenMenu = new OpenMenuFormat();
      // create dir for data
      string dirToStore = "FringshausOpenMenu";
      string dirToStoreCombined = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), dirToStore);
      if(!Directory.Exists(dirToStoreCombined)) {
        Directory.CreateDirectory(dirToStoreCombined);
      }
      this.menuFileName = "alacarte.xml";
      this.fileToStoreLocal = Path.Combine(dirToStoreCombined,this.menuFileName);
      FTPTools.Download("dotob.de", 21, this.menuFileName, this.fileToStoreLocal, "fringshaus", "fringshaus", false);

      XElement xmlElem = XElement.Load(this.fileToStoreLocal);
      this.OpenMenu.FromXML(xmlElem);

    }
    public OpenMenuFormat OpenMenu { get; set; }

    public MenuBase SelectedItem { get; set; }
    public Menu SelectedMenu { get; set; }

    public void NewMenu() {
      this.OpenMenu.Menus.Add(new Menu { Name = "New Menu"});
    }

    public void NewMenuGroup() {
      this.SelectedMenu.Groups.Add(new MenuGroup { Menu = this.SelectedMenu});
    }

    public void NewMenuItem() {
      MenuGroup mg = this.SelectedItem as MenuGroup;
      if(mg!=null) {
        mg.Items.Add(new MenuItem { Group = mg});
      }
    }

    public void DeleteSelectedElement() {
     if(this.SelectedItem is MenuGroup) {
       MenuGroup mg = (MenuGroup) this.SelectedItem;
       mg.Menu.Groups.Remove(mg);
     } else if (this.SelectedItem is MenuItem) {
       MenuItem mi = (MenuItem)this.SelectedItem;
       mi.Group.Items.Remove(mi);
     }
    }

    public void MoveSelectedElementUp() {
      

    }

    public void MoveSelectedElementDown() {
      

    }

    public void Save() {
      XElement xElement = this.OpenMenu.ToXML();
      xElement.Save(this.fileToStoreLocal);
      FTPTools.Upload("dotob.de", 21, this.fileToStoreLocal, this.menuFileName, "fringshaus", "fringshaus", false);
    }
  }
}