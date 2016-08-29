using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Energy.UI.Annotations;

namespace Energy.UI.Model
{
    public class FeaturedItem : Dictionary<string, double>, ICustomTypeDescriptor, INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        private readonly ObservableCollection<string> _featuresNames;

        public FeaturedItem(string name, ObservableCollection<string> featuresNames)
        {
            if (featuresNames == null)
                throw new ArgumentException("featuresNames");

            _name = name;
            _featuresNames = featuresNames;

            foreach (var featureName in featuresNames)
            {
                Add(featureName, 0.0);
            }
        }

        public string Serialize()
        {
            var resultList = new List<string>();
            foreach (var featureName in _featuresNames)
                resultList.Add(this[featureName].ToString("0.00", CultureInfo.InvariantCulture));
            
            return string.Join(",", resultList);
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region ICustomTypeDescriptorMemebers
        public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this);
        public string GetClassName() => null;
        public string GetComponentName() => null;
        public TypeConverter GetConverter() => null;
        public EventDescriptor GetDefaultEvent() => null;
        public PropertyDescriptor GetDefaultProperty() => null;
        public object GetEditor(Type editorBaseType) => null;
        public EventDescriptorCollection GetEvents() => new EventDescriptorCollection(null);
        public EventDescriptorCollection GetEvents(Attribute[] attributes) => new EventDescriptorCollection(null);
        public object GetPropertyOwner(PropertyDescriptor pd) => this;
        public PropertyDescriptorCollection GetProperties() => GetProperties(null);
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var propertyDescriptors = _featuresNames
                .Select(f => new FeaturedItemPropertyDescriptor(f))
                .Union(_featuresNames
                    .Select(f => new FeaturedItemPropertyDescriptor(f)))
                .ToArray();
            return new PropertyDescriptorCollection(propertyDescriptors);
        }
        #endregion
    }
}