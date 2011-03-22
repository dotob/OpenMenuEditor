using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace OpenMenuEditor.OpenMenu {
  public interface IMenuXMLSerializable {
    void FromXML(XElement xml);
    XElement ToXML();
  }

  public abstract class MenuBase : NotifyPropertyBase, IMenuXMLSerializable
  {
    protected MenuBase() {
      this.UID = new Guid().ToString();
    }

    public string UID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    #region IMenuXMLSerializable Members

    public abstract void FromXML(XElement xml);
    public abstract XElement ToXML();

    #endregion
  }

  public abstract class NotifyPropertyBase : INotifyPropertyChanged {
    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    public void OnPropertyChange(string propertyName) {
      PropertyChangedEventHandler tmp = this.PropertyChanged;
      if (tmp != null) {
        tmp(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}