using System;
using System.Windows;
using System.Windows.Controls;

namespace Pharmacy.Controls
{
    public class BaseTabItem : TabItem
    {
        private BaseTabView listview = null;
        protected event EventHandler Close = null;

        public BaseTabItem(string headerTitle)
        {
            var header = new TextBlock { Text = headerTitle };

            Grid grid = new Grid();
            grid.Width = double.NaN;
            grid.Height = double.NaN;
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
 /* this */ Content = grid;

            SetHeader(header);
        }

        public void SetView(BaseTabView view)
        {
            if (listview != null)
                return;

            listview = view;
            listview.Width = double.NaN;
            listview.Height = double.NaN;
            listview.HorizontalAlignment = HorizontalAlignment.Stretch;
            listview.VerticalAlignment = VerticalAlignment.Stretch;
 /* this */ ((Grid)Content).Children.Add(listview);
        }

        protected void SetHeader(UIElement header)
        {
            // Container for header controls
            var dockPanel = new DockPanel();
            dockPanel.Children.Add(header);

            // Close button to remove the tab
            var closeButton = new CloseButton();
            closeButton.Click +=
                (sender, e) =>
                {
                    if (listview != null)
                        listview.Close();

                    var tabControl = this.Parent as ItemsControl;
                    tabControl.Items.Remove(this);

                    if (Close != null)
                        Close(this, null); // send data (emit event)
                };

            closeButton.Margin = new Thickness(
                                                closeButton.Margin.Left + 7, 
                                                closeButton.Margin.Top, 
                                                closeButton.Margin.Right, 
                                                closeButton.Margin.Bottom
                                              );

            //dockPanel.Children.Add(closeButton);

            // Set the header
 /* this */ Header = dockPanel;
        }
    }
}
