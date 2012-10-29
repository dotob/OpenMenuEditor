using System;
using System.ComponentModel;
using System.Text;
using System.Xml.Linq;
using Tamir.SharpSsh.java.lang;

namespace OpenMenuEditor.OpenMenu {
  public interface IMenuXMLSerializable {
    void FromXML(XElement xml);
    XElement ToXML();
  }  
  
  public interface IMenuHTMLSerializable {
    void ToHTML(StringBuilder sb);
  }

  public abstract class MenuBase : NotifyPropertyBase, IMenuXMLSerializable, IMenuHTMLSerializable
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

    public abstract void ToHTML(StringBuilder sb);
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