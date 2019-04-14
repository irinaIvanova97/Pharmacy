using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Pharmacy.Controls
{
    public class BaseDialog : Window
    {
        private bool isUsingOK;
        private bool isUsingCancel;
        protected int  TabOrder { get; private set; }

        public enum DialogModes
        {
            Preview,
            Add,
            Edit
        }

        protected DialogModes DialogMode { get; private set; }

        public BaseDialog(DialogModes dialogMode, DependencyObject parent = null, bool _isUsingOK = true, bool _isUsingCancel = true)
        {
            isUsingOK = _isUsingOK;
            isUsingCancel = _isUsingCancel;
            TabOrder = 0;
            DialogMode = dialogMode;

 /* this */ SourceInitialized += BaseDialog_SourceInitialized;
 /* this */ Initialized += BaseDialog_Initialized;
 /* this */ Loaded += BaseDialog_Loaded;

          //  Uri iconUri = new Uri("pack://application:,,,/Pharmacy;component/Resources/favicon.ico");
           // this.Icon = BitmapFrame.Create(iconUri);

            if (parent != null)
                Owner = /* Window */ GetWindow(parent);
        }

        private void BaseDialog_Loaded(object sender, RoutedEventArgs e)
        {
            CopyDataToControls();
        }

        private void BaseDialog_Initialized(object sender, EventArgs e)
        {
            LoadData();
            OnInitControls();
        }

        protected virtual void OnInitControls()
        {
            if(isUsingOK)
            {
                Button okButton = null;
                GetOKButton(ref okButton);

                okButton.Click += OnOkClick;
            }

            if(isUsingCancel)
            {
                Button cancelButton = null;
                GetCancelButton(ref cancelButton);

                cancelButton.Click += OnCancelClick;
            }
        }

        protected void OnOkClick(object sender, RoutedEventArgs e)
        {
            if (!ValidateData())
                return;

            CopyControlsToData();

            if (!OnOk())
                return;

 /* this */
            DialogResult = true;
 /* this */ Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
 /* this */ DialogResult = false;
 /* this */ Close();
        }

        protected virtual void GetOKButton(ref Button okButton)
        {
            if(isUsingOK)
                throw new NotImplementedException();
        }

        protected virtual void GetCancelButton(ref Button cancelButton)
        {
            if (isUsingCancel)
                throw new NotImplementedException();
        }

        protected virtual void CopyDataToControls() { }

        protected virtual void CopyControlsToData() { }

        protected virtual bool ValidateData() { return true; }

        protected virtual bool OnOk() { return true; }

        protected virtual void LoadData() { }

        protected int AddTabIndex()
        {
            return TabOrder++;
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000; // maximize button
        private const int WS_MINIMIZEBOX = 0x20000; // minimize button

        private void BaseDialog_SourceInitialized(object sender, EventArgs e)
        {
            // remove maximize and minimize buttons

            IntPtr _windowHandle = new WindowInteropHelper(this).Handle;
            if (_windowHandle == IntPtr.Zero)
                throw new InvalidOperationException("The window has not yet been completely initialized");

            SetWindowLong(_windowHandle, GWL_STYLE, GetWindowLong(_windowHandle, GWL_STYLE) & ~WS_MINIMIZEBOX & ~WS_MAXIMIZEBOX);
        }
    }
}
