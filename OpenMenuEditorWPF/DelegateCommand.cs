using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace OpenMenuEditorWPF {
  /// <summary>
  ///     This class allows delegating the commanding logic to methods passed as parameters,
  ///     and enables a View to bind commands to objects that are not part of the element tree.
  /// </summary>
  public class DelegateCommand : ICommand
  {
    #region Constructors

    /// <summary>
    ///     Constructor
    /// </summary>
    public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod){
      this.executeMethod = executeMethod;
      this.canExecuteMethod = canExecuteMethod;
    }

    #endregion

    #region ICommand Members

    /// <summary>
    ///     ICommand.CanExecuteChanged implementation
    /// </summary>
    public event EventHandler CanExecuteChanged {
      add {
        if (!this.isAutomaticRequeryDisabled) {
          CommandManager.RequerySuggested += value;
        }
        CommandManagerHelper.AddWeakReferenceHandler(ref this.canExecuteChangedHandlers, value, 2);
      }
      remove {
        if (!this.isAutomaticRequeryDisabled) {
          CommandManager.RequerySuggested -= value;
        }
        CommandManagerHelper.RemoveWeakReferenceHandler(this.canExecuteChangedHandlers, value);
      }
    }

    bool ICommand.CanExecute(object parameter) {
      return this.CanExecute();
    }

    void ICommand.Execute(object parameter) {
      this.Execute();
    }

    #endregion

    #region Data

    private readonly Func<bool> canExecuteMethod;
    private readonly Action executeMethod;
    private List<WeakReference> canExecuteChangedHandlers;
    private bool isAutomaticRequeryDisabled;

    #endregion

    #region Public Methods

    /// <summary>
    ///     Method to determine if the command can be executed
    /// </summary>
    public bool CanExecute() {
      if (this.canExecuteMethod != null) {
        return this.canExecuteMethod();
      }
      return true;
    }

    /// <summary>
    ///     Execution of the command
    /// </summary>
    public void Execute() {
      if (this.executeMethod != null) {
        this.executeMethod();
      }
    }

    /// <summary>
    ///     Raises the CanExecuteChaged event
    /// </summary>
    public void RaiseCanExecuteChanged() {
      this.OnCanExecuteChanged();
    }

    /// <summary>
    ///     Protected virtual method to raise CanExecuteChanged event
    /// </summary>
    protected virtual void OnCanExecuteChanged() {
      CommandManagerHelper.CallWeakReferenceHandlers(this.canExecuteChangedHandlers);
    }

    /// <summary>
    ///     Property to enable or disable CommandManager's automatic requery on this command
    /// </summary>
    public bool IsAutomaticRequeryDisabled {
      get { return this.isAutomaticRequeryDisabled; }
      set {
        if (this.isAutomaticRequeryDisabled != value) {
          if (value) {
            CommandManagerHelper.RemoveHandlersFromRequerySuggested(this.canExecuteChangedHandlers);
          } else {
            CommandManagerHelper.AddHandlersToRequerySuggested(this.canExecuteChangedHandlers);
          }
          this.isAutomaticRequeryDisabled = value;
        }
      }
    }

    #endregion
  }

  /// <summary>
  ///     This class contains methods for the CommandManager that help avoid memory leaks by
  ///     using weak references.
  /// </summary>
  internal class CommandManagerHelper
  {
    internal static void CallWeakReferenceHandlers(List<WeakReference> handlers) {
      if (handlers != null) {
        // Take a snapshot of the handlers before we call out to them since the handlers
        // could cause the array to me modified while we are reading it.

        EventHandler[] callees = new EventHandler[handlers.Count];
        int count = 0;

        for (int i = handlers.Count - 1; i >= 0; i--) {
          WeakReference reference = handlers[i];
          EventHandler handler = reference.Target as EventHandler;
          if (handler == null) {
            // Clean up old handlers that have been collected
            handlers.RemoveAt(i);
          } else {
            callees[count] = handler;
            count++;
          }
        }

        // Call the handlers that we snapshotted
        for (int i = 0; i < count; i++) {
          EventHandler handler = callees[i];
          handler(null, EventArgs.Empty);
        }
      }
    }

    internal static void AddHandlersToRequerySuggested(List<WeakReference> handlers) {
      if (handlers != null) {
        foreach (WeakReference handlerRef in handlers) {
          EventHandler handler = handlerRef.Target as EventHandler;
          if (handler != null) {
            CommandManager.RequerySuggested += handler;
          }
        }
      }
    }

    internal static void RemoveHandlersFromRequerySuggested(List<WeakReference> handlers) {
      if (handlers != null) {
        foreach (WeakReference handlerRef in handlers) {
          EventHandler handler = handlerRef.Target as EventHandler;
          if (handler != null) {
            CommandManager.RequerySuggested -= handler;
          }
        }
      }
    }

    internal static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler, int defaultListSize) {
      if (handlers == null) {
        handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
      }

      handlers.Add(new WeakReference(handler));
    }

    internal static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler) {
      if (handlers != null) {
        for (int i = handlers.Count - 1; i >= 0; i--) {
          WeakReference reference = handlers[i];
          EventHandler existingHandler = reference.Target as EventHandler;
          if ((existingHandler == null) || (existingHandler == handler)) {
            // Clean up old handlers that have been collected
            // in addition to the handler that is to be removed.
            handlers.RemoveAt(i);
          }
        }
      }
    }
  }

}