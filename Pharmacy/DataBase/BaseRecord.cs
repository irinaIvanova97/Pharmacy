using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Pharmacy.DataBase
{
    public class BaseRecord : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Type DerivedType { get; private set; }
        public int StartFromProperty { get; private set; }
        public int EndToProperty { get; private set; }

        public BaseRecord(Type type, int endToProperty = -1, int startFromProperty = 0)
        {
            DerivedType = type;
            StartFromProperty = startFromProperty;
            EndToProperty = endToProperty;
        }

        public virtual List<string> GetFields()
        {
            List<string> fields = new List<string>();
            PropertyInfo[] propertyArray = DerivedType.GetProperties();

            if (StartFromProperty < 0 || StartFromProperty > propertyArray.Count())
            {
                Log.LogError("startFrom is not in range.");
                return fields;
            }

            if (EndToProperty < 0)
                EndToProperty = propertyArray.Count();

            if (EndToProperty > propertyArray.Count())
            {
                Log.LogError("endTo is not in range.");
                return fields;
            }

            for (int i = StartFromProperty; i < EndToProperty; i++)
            {
                PropertyInfo property = propertyArray[i];
                fields.Add(property.Name);
            }

            return fields;
        }

        public void UpdateAll()
        {
            foreach (string column in GetFields())
                NotifySinglePropertyChanged(column);
        }

        private void NotifySinglePropertyChanged([CallerMemberName]string name = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }
    }

    public class BaseRecordObservableCollection : ObservableCollection<BaseRecord>
    {
        public BaseRecordObservableCollection() : base()
        {
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                IList newItems = e.NewItems;
                if (newItems == null)
                {
                    Log.LogWarning("Can't get items.");
                    return;
                }

                foreach (BaseRecord record in newItems)
                {
                    record.UpdateAll();
                }
            }
        }
    }
}


