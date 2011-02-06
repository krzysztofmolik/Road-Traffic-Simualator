using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Arcane.Xna.Presentation {

  /// <summary>Service allowing object to monitor user input for a form or control</summary>
  public interface IInputPublisherService {

    /// <summary>Fired when the mouse has been clicked</summary>
    event MouseEventHandler MouseClick;
    /// <summary>Fired when a mouse button is pressed down</summary>
    event MouseEventHandler MouseDown;
    /// <summary>Fired when a mouse button is released again</summary>
    event MouseEventHandler MouseUp;
    /// <summary>Fired when the mouse has been moved</summary>
    event MouseEventHandler MouseMove;

    /// <summary>Fired when a key is pressed down</summary>
    event KeyEventHandler KeyDown;
    /// <summary>Fired when a key is released again</summary>
    event KeyEventHandler KeyUp;

  }

  /// <summary>Service allowing object to monitor user input for a form or control</summary>
  internal class ControlInputPublisher : IInputPublisherService, IDisposable {

    /// <summary>Fired when the mouse has been clicked</summary>
    public event MouseEventHandler MouseClick;
    /// <summary>Fired when a mouse button is pressed down</summary>
    public event MouseEventHandler MouseDown;
    /// <summary>Fired when a mouse button is released again</summary>
    public event MouseEventHandler MouseUp;
    /// <summary>Fired when the mouse has been moved</summary>
    public event MouseEventHandler MouseMove;

    /// <summary>Fired when a key is pressed down</summary>
    public event KeyEventHandler KeyDown;
    /// <summary>Fired when a key is released again</summary>
    public event KeyEventHandler KeyUp;

    /// <summary>Initializes a new user control input event publisher</summary>
    /// <param name="control">User control whose input events to publish</param>
    public ControlInputPublisher(FrameworkElement control) {
      this.control = control;


      control.MouseDown += new MouseButtonEventHandler(mouseDown);
      control.MouseUp += new MouseButtonEventHandler(mouseUp);
      control.MouseMove += new MouseEventHandler(mouseMove);
      control.KeyDown += new KeyEventHandler(keyDown);
      control.KeyUp += new KeyEventHandler(keyUp);
    }

    /// <summary>Immediately releases all resources owned by the object</summary>
    public void Dispose() {
      if(this.control != null) {
        this.control.MouseDown -= new MouseButtonEventHandler(mouseDown);
        this.control.MouseUp -= new MouseButtonEventHandler(mouseUp);
        this.control.MouseMove -= new MouseEventHandler(mouseMove);
        this.control.KeyDown -= new KeyEventHandler(keyDown);
        this.control.KeyUp -= new KeyEventHandler(keyUp);

        this.control = null;

        GC.SuppressFinalize(this);
      }
    }


    /// <summary>Called when a mouse button has been pressed</summary>
    /// <param name="sender">Window on which the button has been pressed</param>
    /// <param name="arguments">
    ///   Informations about the mouse state at the time of press
    /// </param>
    private void mouseDown(object sender, MouseButtonEventArgs arguments) {
      if(MouseDown != null)
        MouseDown(sender, arguments);
    }

    /// <summary>Called when a mouse button has been released</summary>
    /// <param name="sender">Window on which the button has been released</param>
    /// <param name="arguments">
    ///   Informations about the mouse state at the time of release
    /// </param>
    private void mouseUp(object sender, MouseButtonEventArgs arguments)
    {
      if(MouseUp != null)
        MouseUp(sender, arguments);
    }

    /// <summary>Called when the mouse has been moved</summary>
    /// <param name="sender">Window over which the mouse has been moved</param>
    /// <param name="arguments">
    ///   Informations about the mouse state at the time of movement
    /// </param>
    private void mouseMove(object sender, MouseEventArgs arguments) {
      if(MouseMove != null)
        MouseMove(sender, arguments);
    }

    /// <summary>Called when a key has been pressed</summary>
    /// <param name="sender">Window having the input focus</param>
    /// <param name="arguments">
    ///   Informations about the keyboard state at the time of press
    /// </param>
    private void keyDown(object sender, KeyEventArgs arguments) {
      if(KeyDown != null)
        KeyDown(sender, arguments);
    }

    /// <summary>Called when a key has been released</summary>
    /// <param name="sender">Window having the input focus</param>
    /// <param name="arguments">
    ///   Informations about the keyboard state at the time of release
    /// </param>
    private void keyUp(object sender, KeyEventArgs arguments) {
      if(KeyUp != null)
        KeyUp(sender, arguments);
    }

    /// <summary>User control whose input events this publisher makes public</summary>
    private FrameworkElement control;
  }

} // namespace Arcane.Windows.Forms.Xna
