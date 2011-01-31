using System.Linq;
using System.Windows;
using System.Windows.Input;
using OpenMenuEditor.OpenMenu;

namespace OpenMenuEditorWPF {
  /// <summary>
  /// Interaction logic for MainView.xaml
  /// </summary>
  public partial class MainView : Window {
    private ICommand closeCommand;
    private ICommand deleteElementCommand;
    private ICommand moveElementDownCommand;
    private ICommand moveElementUpCommand;
    private ICommand newMenuCommand;
    private ICommand newMenuGroupCommand;
    private ICommand newMenuItemCommand;
    private ICommand saveCommand;

    public MainView() {
      this.ViewModel = new MainViewModel();
      this.InitializeComponent();

      // sparkle updater
#if !DEBUG
      WinSparkleWrapper.Startup("http://update.dotob.de/openmenueditor.rss");
#endif
    }

    public MainViewModel ViewModel { get; set; }

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
          this.saveCommand = new DelegateCommand(() => this.ViewModel.Save(),
                                                 () => true);
        }
        return this.saveCommand;
      }
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

    private void uc_Closed(object sender, System.EventArgs e) {
#if !DEBUG
      WinSparkleWrapper.Cleanup();
#endif
    }
  }
}