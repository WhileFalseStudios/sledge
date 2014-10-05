﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Sledge.Editor.Properties;
using Sledge.Gui;
using Sledge.Gui.Controls;
using Sledge.Gui.Interfaces;
using Sledge.Gui.Shell;
using Sledge.Gui.WinForms;
using Button = Sledge.Gui.Controls.Button;
using ComboBox = Sledge.Gui.Controls.ComboBox;
using IContainer = Sledge.Gui.Interfaces.IContainer;
using Label = Sledge.Gui.Controls.Label;
using PictureBox = Sledge.Gui.Controls.PictureBox;
using Size = Sledge.Gui.Interfaces.Size;
using TextBox = Sledge.Gui.Controls.TextBox;

namespace Sledge.Editor
{
    class BindingObject : INotifyPropertyChanged
    {
        private string _value1;
        private string _value2;

        public string Value1
        {
            get { return _value1; }
            set
            {
                if (value == _value1) return;
                _value1 = value;
                OnPropertyChanged("Value1");
            }
        }

        public string Value2
        {
            get { return _value2; }
            set
            {
                if (value == _value2) return;
                _value2 = value;
                OnPropertyChanged("Value2");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class BindingControl : VerticalBox
    {
        public BindingObject BindingObject { get; set; }

        public BindingControl(IUIManager man)
        {
            var hbox1 = new HorizontalBox();
            var hbox2 = new HorizontalBox();
            var label1 = new Label();
            var textbox1 = new TextBox();
            var pic = new PictureBox();

            var label2 = new Label();
            var combo2 = new ComboBox();

            BindingSource = BindingObject;
            label1.Text = "Value 1";
            label2.Text = "Value 2";

            combo2.Items.Add("Item 1");
            combo2.Items.Add("Item 2");
            combo2.Items.Add("Item 3");
            combo2.SelectedItem = "Item 2";
            combo2.SelectedIndexChanged += (sender, args) => Debug.WriteLine(combo2.SelectedIndex);

            var bmp = new Bitmap(100, 100);
            using (var g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.FillRectangle(System.Drawing.Brushes.Red, 0, 0, 100, 100);
                g.FillRectangle(System.Drawing.Brushes.Blue, 25, 25, 50, 50);
            }
            pic.Image = bmp;

            hbox1.Add(label1);
            hbox1.Add(textbox1, true);
            this.Add(hbox1);

            hbox2.Add(label2);
            hbox2.Add(combo2);
            this.Add(hbox2);

            this.Add(pic);
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IUIManager man;
            man = new WinFormsUIManager();
            //man = new GtkUIManager();

            UIManager.Manager = man; // todo

            man.Shell.WindowLoaded += (sender, args) =>
            {
                var vbox = new VerticalBox();

                // var button = man.Construct<IButton>();
                var button = new Button();
                button.Text = "Button";
                button.Enabled = true;
                button.Clicked += (o, eventArgs) =>
                {
                    var window = man.CreateWindow();
                    window.Title = "Test Window";
                    window.AutoSize = true;

                    var box = new VerticalBox();
                    var btn = new Button();
                    btn.Text = "Add Button";
                    box.Add(btn);
                    window.Container.Set(box);

                    var col = new Collapsible();
                    col.Set(new Button());
                    box.Add(col);

                    btn.Clicked += (sender1, args1) =>
                    {
                        box.Add(new Button());
                    };

                    window.Open();
                };
                vbox.Add(button);

                var button2 = new Button();
                button2.Text = "This is another button";
                button2.PreferredSize = new Size(50, 100);
                vbox.Add(button2);

                var table = man.Construct<IResizableTable>();
                table.Insert(0, 0, new Button());
                table.Insert(0, 1, new Button());
                table.Insert(1, 0, new Button());

                //var scroll = man.Construct<IVerticalScrollContainer>();
                //var scrollInner = man.Construct<IVerticalBox>();
                //for (int i = 0; i < 10; i++)
                //{
                //    var b = man.Construct<IButton>();
                //    b.Text = i.ToString(CultureInfo.InvariantCulture);
                //    scrollInner.Add(b);
                //}
                //scroll.Set(scrollInner);
                //table.Insert(1, 1, scroll);

                vbox.Add(table, true);

                for (int j = 0; j < 2; j++)
                {
                    var tempSidebar = new Collapsible();
                    var sideBox = new VerticalBox();
                    for (int i = 0; i < 3; i++)
                    {
                        sideBox.Add(new TextBox());
                    }
                    tempSidebar.Set(sideBox);
                    man.Shell.AddSidebarPanel(tempSidebar, SidebarPanelLocation.Right);
                }

                var sb = new Collapsible();
                var bc = new BindingControl(man);
                sb.Set(bc);
                man.Shell.AddSidebarPanel(sb, SidebarPanelLocation.Right);

                man.Shell.Container.Set(vbox);

                // we be loaded
                man.Shell.Title = "Sledge Editor";
                man.Shell.AddMenu();
                man.Shell.AddToolbar();

                var m = man.Shell.Menu.AddMenuItem("File", "File").AddSubMenuItem("New", "New");
                m.Icon = Resources.Menu_New;
                m.Clicked += (o, eventArgs) =>
                {
                    Debug.WriteLine("New");
                };

                var t = man.Shell.Toolbar.AddToolbarItem("Open", "Open");
                t.Icon = Resources.Menu_Open;
                t.Clicked += (o, eventArgs) =>
                {
                    button.Enabled = !button.Enabled;
                    Debug.WriteLine("Open");
                };
            };

            man.Start();
            return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RegisterHandlers();
            SingleInstance.Start(typeof(Editor));
        }

        private static void RegisterHandlers()
        {
            Application.ThreadException += ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            LogException((Exception) args.ExceptionObject);
        }

        private static void ThreadException(object sender, ThreadExceptionEventArgs args)
        {
            LogException(args.Exception);
        }

        private static void LogException(Exception ex)
        {
            var st = new StackTrace();
            var frames = st.GetFrames() ?? new StackFrame[0];
            var msg = "Unhandled exception";
            foreach (var frame in frames)
            {
                var method = frame.GetMethod();
                msg += "\r\n    " + method.ReflectedType.FullName + "." + method.Name;
            }
            Logging.Logger.ShowException(new Exception(msg, ex), "Unhandled exception");
        }
    }
}
