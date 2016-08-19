using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Energy.UI.Model
{
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