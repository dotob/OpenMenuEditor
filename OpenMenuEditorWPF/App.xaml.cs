using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using NLog;

namespace OpenMenuEditorWPF
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private static Logger nlogger = LogManager.GetCurrentClassLogger();

    public App() {
      Thread.GetDomain().UnhandledException += BackgroundThreadUnhandledException;
      if (System.Windows.Application.Current != null) {
        System.Windows.Application.Current.DispatcherUnhandledException += WPFUIThreadException;
      }
    }

    private void WPFUIThreadException(object sender, DispatcherUnhandledExceptionEventArgs e) {
      nlogger.ErrorException("WPFUIThreadException:", e.Exception);
      MessageBox.Show(string.Format("Schwerer Fehler ist aufgetreten: {0}", e.Exception.Message), "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void BackgroundThreadUnhandledException(object sender, UnhandledExceptionEventArgs e) {
      Exception exception = e.ExceptionObject as Exception;
      nlogger.ErrorException("WPFUIThreadException:", exception);
      MessageBox.Show(string.Format("Schwerer Fehler ist aufgetreten: {0}", exception), "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
    }
  }
}
