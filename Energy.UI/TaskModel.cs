using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Energy.UI
{
    public class TaskModel
    {
        private readonly Random r = new Random(DateTime.Now.Millisecond);
        public TaskModel()
        {
            CommonFeatureNames = new ObservableCollection<string>();
            StationsFeaturesNames = new ObservableCollection<string>();
            ConsumersFeaturesNames = new ObservableCollection<string>();
            Stations = new ObservableCollection<FeaturedItem>();
            Consumers = new ObservableCollection<FeaturedItem>();
        }

        public ObservableCollection<string> CommonFeatureNames { get; }
        public ObservableCollection<string> ConsumersFeaturesNames { get; }
        public ObservableCollection<string> StationsFeaturesNames { get; }
        public ObservableCollection<FeaturedItem> Stations { get; }
        public ObservableCollection<FeaturedItem> Consumers { get; }

        public void AddStation(string name)
        {
            Stations.Add(new FeaturedItem(name, StationsFeaturesNames, CommonFeatureNames));
            AddConsumerFeature(name + "_dist");
        }

        public void RemoveStation(FeaturedItem station)
        {
            Stations.Remove(station);
            RemoveConsumerFeature(station.Name + "_dist");
        }

        public void AddConsumer(string name)
        {
            Consumers.Add(new FeaturedItem(name, ConsumersFeaturesNames, CommonFeatureNames));
            AddStationFeature(name + "_dist");
        }

        public void RemoveConsumer(FeaturedItem consumer)
        {
            Consumers.Remove(consumer);
            RemoveStationFeature(consumer.Name + "_dist");
        }

        public void AddStationFeature(string featureName)
        {
            StationsFeaturesNames.Add(featureName);
            AddFeatureToCollection(Stations, featureName);
        }
        public void AddConsumerFeature(string featureName)
        {
            ConsumersFeaturesNames.Add(featureName);
            AddFeatureToCollection(Consumers, featureName);
        }

        public void AddCommonFeature(string featureName)
        {
            CommonFeatureNames.Add(featureName);
            AddFeatureToCollection(Stations, featureName);
            AddFeatureToCollection(Consumers, featureName);
        }

        private void AddFeatureToCollection(IEnumerable<FeaturedItem> featuredItems, string featureName)
        {
            foreach (var item in featuredItems)
            {
                item.Add(featureName, 0.0);
            }
        }

        public void RemoveFeature(ObservableCollection<FeaturedItem> featuredItems, string featureName)
        {
            foreach (var item in featuredItems)
            {
                item.Remove(featureName);
            }
        }

        public void RemoveConsumerFeature(string featureName)
        {
            ConsumersFeaturesNames.Remove(featureName);
            CommonFeatureNames.Remove(featureName);
            RemoveFeature(Consumers, featureName);
        }

        public void RemoveStationFeature(string featureName)
        {
            StationsFeaturesNames.Remove(featureName);
            CommonFeatureNames.Remove(featureName);
            RemoveFeature(Stations, featureName);
        }
    }

    public class FeaturedItem : Dictionary<string, double>, ICustomTypeDescriptor
    {
        public string Name { get; private set; }
        private readonly ObservableCollection<string> _specificFeaturesNames;
        private readonly ObservableCollection<string> _commonFeaturesNames;

        public FeaturedItem(string name, ObservableCollection<string> specificFeaturesNames, ObservableCollection<string> commonFeaturesNames)
        {
            if (specificFeaturesNames == null)
                throw new ArgumentException("specificFeaturesNames");

            Name = name;
            _specificFeaturesNames = specificFeaturesNames;
            _commonFeaturesNames = commonFeaturesNames;
            foreach (var specificFeatureName in specificFeaturesNames)
            {
                Add(specificFeatureName, 0.0);
            }
            foreach (var commonFeatureName in commonFeaturesNames)
            {
                Add(commonFeatureName, 0.0);
            }
        }

        public string Serialize()
        {
            var resultList = new List<string>();
            foreach (var featureName in _specificFeaturesNames)
                resultList.Add(this[featureName].ToString("0.00", CultureInfo.InvariantCulture));

            foreach (var featureName in _commonFeaturesNames)
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
            var propertyDescriptors = _commonFeaturesNames
                .Select(f => new FeaturedItemPropertyDescriptor(f))
                .Union(_specificFeaturesNames
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
