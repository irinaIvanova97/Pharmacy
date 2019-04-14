using Pharmacy.DataBase;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;

namespace Pharmacy.Controls
{
    public abstract class BaseTabView : ListView
    {
        #region Enums

        public enum ContextMenuTypes
        {
            Default,
            Custom,
            None
        }

        #endregion

        #region Members

        public BaseRecordObservableCollection itemsSource { get; set; }

        private enum ContextMenuItems
        {
            Preview, 
            Add, 
            Edit, 
            Delete, 
        }

        public ContextMenuTypes contextMenuType
        {
            get;
            private set;
        }

        private ContextMenu contextMenu = null;

        #endregion

        #region Constructor

        public BaseTabView(ContextMenuTypes _contextMenuType = ContextMenuTypes.Default) : base()
        {
            contextMenuType = _contextMenuType;
            itemsSource = new BaseRecordObservableCollection();
        }

        #endregion

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Initialize();
        }

        public void Update()
        {
            LoadData();
        }
        
        private void Initialize()
        {
            ItemsSource = itemsSource;
 
            MouseDoubleClick += OnMouseDoubleClick;

            OnViewIntialized();

            OnInitColumns();

            LoadData();

            CreateContextMenu();
        }

        private void OnMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                OnPreviewClick(sender, null);
        }

        private void CreateContextMenu()
        {
            if (contextMenuType == ContextMenuTypes.None)
                return;

            contextMenu = new ContextMenu();

 /* this */ ContextMenu = contextMenu;
            contextMenu.Opened += OnContextMenuOpened;
        }

        private void OnContextMenuOpened(object sender, RoutedEventArgs e)
        {
            contextMenu.Items.Clear();

            List<object> menuItems = new List<object>();

            if (contextMenuType == ContextMenuTypes.Default)
            {
                MenuItem item = null;

                item = new MenuItem();
                item.Header = "Преглед";
                item.Click += OnPreviewClick;
                item.IsEnabled = /* this */ SelectedItems.Count == 1;

                menuItems.Add(item);
                menuItems.Add(new Separator());

                item = new MenuItem();
                item.Header = "Добави";
                item.Click += OnAddClick;

                menuItems.Add(item);
                menuItems.Add(new Separator());

                item = new MenuItem();
                item.Header = "Редактирай";
                item.Click += OnEditClick;
                item.IsEnabled = /* this */ SelectedItems.Count == 1;

                menuItems.Add(item);
                menuItems.Add(new Separator());

                item = new MenuItem();
                item.Header = "Изтрий";
                item.Click += OnDeleteClick;
                item.IsEnabled = /* this */ SelectedItems.Count == 1;

                menuItems.Add(item);
            }

            if (contextMenuType == ContextMenuTypes.Custom)
            {
                menuItems = FillContextMenu(menuItems);
            }

            foreach(object obj in menuItems)
            {
                contextMenu.Items.Add(obj);
            }
        }

        /// <summary>Метод при натискане на "Преглед" от контекстното меню</summary>
        protected virtual void OnPreviewClick(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>Метод при натискане на "Добави" от контекстното меню</summary>
        protected virtual void OnAddClick(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>Метод при натискане на "Редактирай" от контекстното меню</summary>
        protected virtual void OnEditClick(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>Метод при натискане на "Изтрий" от контекстното меню</summary>
        protected virtual void OnDeleteClick(object sender, RoutedEventArgs e)
        {
        }

        #region Virtual Methods

        /// <summary>Метод при завършена инициализация на view</summary>
        protected virtual void OnViewIntialized() { }
        /// <summary>Метод за инициализация на колони</summary>
        protected abstract void OnInitColumns();
        /// <summary>Метод за инициализиране на данни</summary>
        protected abstract void LoadData();
        /// <summary>Метод при затваряне на view</summary>
        public virtual void Close() { }
        /// <summary>Метод за добавяне на елементи към контекстовото меню</summary>
        protected virtual List<object> FillContextMenu(List<object> items) { return items; }
        /// <summary>Метод за получаване на избран елемент</summary>
        public virtual object GetSelectedItem() { return null; }

        #endregion
    }
}
