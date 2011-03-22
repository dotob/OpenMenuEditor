using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Xml.Linq;
using NLog;
using OpenMenuEditor.OpenMenu;
using OpenMenuEditorWPF.Properties;

namespace OpenMenuEditorWPF {
  public class MainViewModel : INotifyPropertyChanged {
    private static readonly Logger nlogger = LogManager.GetCurrentClassLogger();
    private string fileToStoreLocal;
    private string menuFileName;
    private bool goOnline = true;

    public MainViewModel() {
      this.InitMenu();
    }

    private OpenMenuFormat openMenu = new OpenMenuFormat();

    public OpenMenuFormat OpenMenu {
      get { return this.openMenu; }
      set {
        if (this.openMenu == value) {
          return;
        }
        this.openMenu = value;
        PropertyChangedEventHandler tmp = this.PropertyChanged;
        if (tmp != null) {
          tmp(this, new PropertyChangedEventArgs("OpenMenu"));
        }
      }
    }

    public MenuBase SelectedItem { get; set; }
    public Menu SelectedMenu { get; set; }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    public void InitMenu() {
      this.OpenMenu = new OpenMenuFormat();
      // create dir for data
      XElement xmlElem;
      const string dirToStore = "FringshausOpenMenu";
      string dirToStoreCombined = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), dirToStore);
      if (!Directory.Exists(dirToStoreCombined)) {
        nlogger.Debug("create dir to store: {0}", dirToStoreCombined);
        Directory.CreateDirectory(dirToStoreCombined);
      } else {
        nlogger.Debug("use dir to store: {0}", dirToStoreCombined);
      }

      // for testing no need to go online
      if (this.goOnline) {
        nlogger.Debug("go online", dirToStoreCombined);

        this.menuFileName = "alacarte.xml";
        this.fileToStoreLocal = Path.Combine(dirToStoreCombined, this.menuFileName);
        //FTPTools.Download("dotob.de", 21, this.menuFileName, this.fileToStoreLocal, "fringshaus", "fringshaus", false);
        bool success = FTPTools.DownloadScp("dotob.de", this.menuFileName, this.fileToStoreLocal, "fringshaus", "fringshaus");

        if (success) {
          xmlElem = XElement.Load(this.fileToStoreLocal);
          this.OpenMenu.FromXML(xmlElem);
        } else {
          MessageBox.Show("Leider ist beim laden des Menüs ein Fehler aufgetreten. Siehe LogDatei...");
          Application.Current.Shutdown();
        }
      } else {
        xmlElem = XElement.Load(new StringReader(Resources.alacarte_menu));
        this.OpenMenu.FromXML(xmlElem);
      }
    }

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
      } else if (this.SelectedItem is MenuItem) {
        var mi = (MenuItem) this.SelectedItem;
        mi.Group.Items.Remove(mi);
      }
    }

    public void MoveSelectedElementUp() {
      if (this.SelectedItem is MenuGroup) {
        var mg = (MenuGroup) this.SelectedItem;
        int oldIndex = mg.Menu.Groups.IndexOf(mg);
        if (oldIndex > 0) {
          mg.Menu.Groups.Move(oldIndex, oldIndex - 1);
        }
      } else if (this.SelectedItem is MenuItem) {
        var mi = (MenuItem) this.SelectedItem;
        int oldIndex = mi.Group.Items.IndexOf(mi);
        if (oldIndex > 0) {
          mi.Group.Items.Move(oldIndex, oldIndex - 1);
        }
      }
    }

    public void MoveSelectedElementDown() {
      if (this.SelectedItem is MenuGroup) {
        var mg = (MenuGroup) this.SelectedItem;
        int oldIndex = mg.Menu.Groups.IndexOf(mg);
        if (oldIndex < mg.Menu.Groups.Count - 1) {
          mg.Menu.Groups.Move(oldIndex, oldIndex + 1);
        }
      } else if (this.SelectedItem is MenuItem) {
        var mi = (MenuItem) this.SelectedItem;
        int oldIndex = mi.Group.Items.IndexOf(mi);
        if (oldIndex < mi.Group.Items.Count - 1) {
          mi.Group.Items.Move(oldIndex, oldIndex + 1);
        }
      }
    }

    public void Save() {
      XElement xElement = this.OpenMenu.ToXML();
      if (this.goOnline) {
        xElement.Save(this.fileToStoreLocal);
        //FTPTools.Upload("dotob.de", 21, this.fileToStoreLocal, this.menuFileName, "fringshaus", "fringshaus", false);
        FTPTools.UploadScp("dotob.de", this.fileToStoreLocal, this.menuFileName, "fringshaus", "fringshaus");
      } else {
        xElement.Save(this.fileToStoreLocal);
      }
    }
  }
}