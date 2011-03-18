using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using NLog;
using OpenMenuEditor.OpenMenu;

namespace OpenMenuEditorWPF {
  /// <summary>
  /// Interaction logic for MainView.xaml
  /// </summary>
  public partial class MainView : Window, INotifyPropertyChanged {
    private ICommand closeCommand;
    private ICommand deleteElementCommand;
    private ICommand moveElementDownCommand;
    private ICommand moveElementUpCommand;
    private ICommand newMenuCommand;
    private ICommand newMenuGroupCommand;
    private ICommand newMenuItemCommand;
    private ICommand saveCommand;
    private static Logger nlogger = LogManager.GetCurrentClassLogger();

    public MainView() {
      nlogger.Debug("mainview started");
      this.ViewModel = new MainViewModel();
      this.InitializeComponent();
      this.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
      // sparkle updater
#if !DEBUG
      WinSparkleWrapper.Startup("http://update.dotob.de/openmenueditor/appcast.xml");
#endif
    }

    private MainViewModel viewModel;
    public MainViewModel ViewModel {
      get { return this.viewModel; }
      set {
        this.viewModel = value;
        var tmp = this.PropertyChanged;
        if (tmp != null) {
          tmp(this, new PropertyChangedEventArgs("ViewModel"));
        }
      }
    }

    public ICommand NewMenuCommand {
      get {
        if (this.newMenuCommand == null) {
          this.newMenuCommand = new DelegateCommand(() => this.ViewModel.NewMenu(),
                                                    () => true);
        }
        return this.newMenuCommand;
      }
    }

    public ICommand NewMenuGroupCommand {
      get {
        if (this.newMenuGroupCommand == null) {
          this.newMenuGroupCommand = new DelegateCommand(() => this.ViewModel.NewMenuGroup(),
                                                         () => true);
        }
        return this.newMenuGroupCommand;
      }
    }

    public ICommand NewMenuItemCommand {
      get {
        if (this.newMenuItemCommand == null) {
          this.newMenuItemCommand = new DelegateCommand(() => this.ViewModel.NewMenuItem(),
                                                        () => this.ViewModel.SelectedItem is MenuGroup);
        }
        return this.newMenuItemCommand;
      }
    }

    public ICommand DeleteElementCommand {
      get {
        if (this.deleteElementCommand == null) {
          this.deleteElementCommand = new DelegateCommand(() => this.ViewModel.DeleteSelectedElement(),
                                                          () => this.ViewModel.SelectedItem != null);
        }
        return this.deleteElementCommand;
      }
    }

    public ICommand MoveElementUpCommand {
      get {
        if (this.moveElementUpCommand == null) {
          this.moveElementUpCommand = new DelegateCommand(() => this.ViewModel.MoveSelectedElementUp(),
                                                          () => this.ViewModel.SelectedItem != null);
        }
        return this.moveElementUpCommand;
      }
    }

    public ICommand MoveElementDownCommand {
      get {
        if (this.moveElementDownCommand == null) {
          this.moveElementDownCommand = new DelegateCommand(() => this.ViewModel.MoveSelectedElementDown(),
                                                            () => this.ViewModel.SelectedItem != null);
        }
        return this.moveElementDownCommand;
      }
    }

    public ICommand SaveCommand {
      get {
        if (this.saveCommand == null) {
          this.saveCommand = new DelegateCommand(() => this.Save(),
                                                 () => true);
        }
        return this.saveCommand;
      }
    }

    private void Save() {
      this.Cursor = Cursors.Wait;
      this.ViewModel.Save();
      this.Cursor = Cursors.Arrow;
    }

    public ICommand CloseCommand {
      get {
        if (this.closeCommand == null) {
          this.closeCommand = new DelegateCommand(this.Close,
                                                  () => true);
        }
        return this.closeCommand;
      }
    }

    private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
      this.ViewModel.SelectedItem = e.NewValue as MenuBase;
    }

    private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      if (e.AddedItems.Count > 0) {
        this.ViewModel.SelectedMenu = e.AddedItems[0] as Menu;
      } else {
        this.ViewModel.SelectedMenu = null;
      }
    }

    private string version;
    public string Version {
      get { return this.version; }
      set {
        this.version = value;
        var tmp = this.PropertyChanged;
        if(tmp!=null) {
          tmp(this, new PropertyChangedEventArgs("Version"));
        }
      }
    }

    private void uc_Closed(object sender, System.EventArgs e) {
#if !DEBUG
      WinSparkleWrapper.Cleanup();
#endif
    }

    private void uc_Loaded(object sender, RoutedEventArgs e) {
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}