using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Energy.UI
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

        public void AddFeature(string featureName)
        {
            FeaturesNames.Add(featureName);
            foreach (var station in Stations)
            {
                station.Add(featureName, 0.0);
            }
            foreach (var consumer in Consumers)
            {
                consumer.Add(featureName, 0.0);
            }
        }
    }

    public class FeaturedItem : Dictionary<string, double>, ICustomTypeDescriptor
    {
        public string Name { get; private set; }
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
        public override Type ComponentType => typeof(Dictionary<string, double>);
        public override bool IsReadOnly => false;
        public override Type PropertyType => typeof(double);

        public override object GetValue(object component)
        {
            var featuredItem = (Dictionary<string, double>)component;
            return featuredItem[Name];
        }

        public override void ResetValue(object component)
        {
            SetValue(component, 0.0);
        }

        public override void SetValue(object component, object value)
        {
            var featuredItem = (Dictionary<string, double>)component;
            featuredItem[Name] = Convert.ToDouble(value);
        }
    }


    //public class FeaturedItemsCollectionModel : ObservableCollection<FeaturedItemModel>, ICustomTypeDescriptor
    //{
    //    private readonly ObservableCollection<string> _features;

    //    public FeaturedItemsCollectionModel(ObservableCollection<string> features)
    //    {
    //        _features = features;
    //    }

    //    #region ICustomTypeDescriptorMemebers

    //    public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this);
    //    public string GetClassName() => null;
    //    public string GetComponentName() => null;
    //    public TypeConverter GetConverter() => null;
    //    public EventDescriptor GetDefaultEvent() => null;
    //    public PropertyDescriptor GetDefaultProperty() => null;
    //    public object GetEditor(Type editorBaseType) => null;
    //    public EventDescriptorCollection GetEvents() => new EventDescriptorCollection(null);
    //    public EventDescriptorCollection GetEvents(Attribute[] attributes) => new EventDescriptorCollection(null);
    //    public object GetPropertyOwner(PropertyDescriptor pd) => this;

    //    public PropertyDescriptorCollection GetProperties() => GetProperties(null);
    //    public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
    //    {
    //        var propertyDescriptors = _features
    //            .Select(f => new FeaturedItemModelPropertyDescriptor(f, attributes))
    //            .ToArray();
    //        return new PropertyDescriptorCollection(propertyDescriptors);
    //    }

    //    #endregion
    //}

    //public class FeaturedItemModel
    //{
    //    public FeaturedItemModel(IEnumerable<string> featuresNames)
    //    {
    //        if(featuresNames == null)
    //            throw new ArgumentException("featuresNames");

    //        Features = featuresNames.ToDictionary(i => i, i => 0.0);
    //    }

    //    public Dictionary<string, double> Features { get; }
    //}

    //public class FeaturedItemModelPropertyDescriptor: PropertyDescriptor
    //{
    //    public FeaturedItemModelPropertyDescriptor(string name, Attribute[] attrs) : base(name, attrs)
    //    {
    //    }

    //    public override bool CanResetValue(object component) => true;
    //    public override bool ShouldSerializeValue(object component) => true;
    //    public override Type ComponentType => typeof(FeaturedItemModel);
    //    public override bool IsReadOnly => false;
    //    public override Type PropertyType => typeof(string);

    //    public override object GetValue(object component)
    //    {
    //        var featuredItem = (FeaturedItemModel)component;
    //        return featuredItem.Features[Name];
    //    }

    //    public override void ResetValue(object component)
    //    {
    //        SetValue(component, 0.0);   
    //    }

    //    public override void SetValue(object component, object value)
    //    {
    //        var featuredItem = (FeaturedItemModel)component;
    //        featuredItem.Features[Name] = Convert.ToDouble(value);
    //    }
    //}

}
