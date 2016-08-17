using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Energy.UI.Model
{
    public class TaskModel
    {
        public TaskModel()
        {
            FeaturesNames = new ObservableCollection<string>();
            Stations = new ObservableCollection<FeaturedItem>();
            Consumers = new ObservableCollection<FeaturedItem>();
        }
        
        public ObservableCollection<string> FeaturesNames { get; }
        public ObservableCollection<FeaturedItem> Stations { get; }
        public ObservableCollection<FeaturedItem> Consumers { get; }

        public void AddStation(string name)
        {
            Stations.Add(new FeaturedItem(name, FeaturesNames));
        }

        public void AddConsumer(string name)
        {
            Consumers.Add(new FeaturedItem(name, FeaturesNames));
        }

        public void RemoveStation(FeaturedItem station)
        {
            Stations.Remove(station);
        }

        public void RemoveConsumer(FeaturedItem consumer)
        {
            Consumers.Remove(consumer);
        }
        
        public void AddFeature(string featureName)
        {
            FeaturesNames.Add(featureName);
            AddFeatureToCollection(Stations, featureName);
            AddFeatureToCollection(Consumers, featureName);
        }

        private static void AddFeatureToCollection(IEnumerable<FeaturedItem> featuredItems, string featureName)
        {
            foreach (var item in featuredItems)
            {
                item.Add(featureName, 0.0);
            }
        }

        public void RemoveFeature(string featureName)
        {
            RemoveFeatureFromCollection(Stations, featureName);
            RemoveFeatureFromCollection(Consumers, featureName);
            FeaturesNames.Remove(featureName);
        }

        private static void RemoveFeatureFromCollection(IEnumerable<FeaturedItem> featuredItem, string featureName)
        {
            foreach (var item in featuredItem)
            {
                item.Remove(featureName);
            }
        }
    }

    public class FeaturedItem : Dictionary<string, double>, ICustomTypeDescriptor
    {
        public string Name { get; }
        private readonly ObservableCollection<string> _featuresNames;

        public FeaturedItem(string name, ObservableCollection<string> featuresNames)
        {
            if (featuresNames == null)
                throw new ArgumentException("featuresNames");

            Name = name;
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

    public class FeaturedItemPropertyDescriptor : PropertyDescriptor
    {
        public FeaturedItemPropertyDescriptor(string name) : base(name, null)
        {
        }
        public override bool CanResetValue(object component) => true;
        public override bool ShouldSerializeValue(object component) => false;
        public override Type ComponentType => typeof (Dictionary<string, double>);
        public override bool IsReadOnly => false;
        public override Type PropertyType => typeof (double);
        public override object GetValue(object component)
        {
            var featuredItem = (Dictionary<string, double>) component;
            return featuredItem[Name];
        }
        public override void ResetValue(object component)
        {
            SetValue(component, 0.0);
        }
        public override void SetValue(object component, object value)
        {
            var featuredItem = (Dictionary<string, double>) component;
            featuredItem[Name] = Convert.ToDouble(value);
        }
    }
}
