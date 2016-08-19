using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Energy.UI.Model
{
    public class FeaturesModel
    {
        public ObservableCollection<string> FeaturesNames { get; }
        public ObservableCollection<FeaturedItem> Stations { get; }
        public ObservableCollection<FeaturedItem> Consumers { get; }

        public FeaturesModel()
        {
            FeaturesNames = new ObservableCollection<string>();
            Stations = new ObservableCollection<FeaturedItem>();
            Consumers = new ObservableCollection<FeaturedItem>();
        }

        public void AddStation(string name)
        {
            if(Stations.Any(x => x.Name.Equals(name)))
                throw new InvalidOperationException("Такая станция уже есть");

            Stations.Add(new FeaturedItem(name, FeaturesNames));
        }

        public void AddConsumer(string name)
        {
            if (Consumers.Any(x => x.Name.Equals(name)))
                throw new InvalidOperationException("Такая станция уже есть");

            Consumers.Add(new FeaturedItem(name, FeaturesNames));
        }

        public void RemoveStation(string name)
        {
            var removedStation = Stations.First(x => x.Name.Equals(name));
            if(removedStation == null)
                throw new InvalidOperationException("Ошибка при удалении: нет такой станции");

            Stations.Remove(removedStation);
        }

        public void RemoveConsumer(string name)
        {
            var removedConsumer = Consumers.First(x => x.Name.Equals(name));
            if (removedConsumer == null)
                throw new InvalidOperationException("Ошибка при удалении: нет такого потребителя");

            Consumers.Remove(removedConsumer);
        }

        public void AddFeature(string featureName)
        {
            FeaturesNames.Add(featureName);
            AddFeatureToCollection(Stations, featureName);
            AddFeatureToCollection(Consumers, featureName);
        }

        public void RemoveFeature(string featureName)
        {
            RemoveFeatureFromCollection(Stations, featureName);
            RemoveFeatureFromCollection(Consumers, featureName);
            FeaturesNames.Remove(featureName);
        }

        private static void AddFeatureToCollection(IEnumerable<FeaturedItem> featuredItems, string featureName)
        {
            foreach (var item in featuredItems)
            {
                item.Add(featureName, 0.0);
            }
        }

        private static void RemoveFeatureFromCollection(IEnumerable<FeaturedItem> featuredItem, string featureName)
        {
            foreach (var item in featuredItem)
            {
                item.Remove(featureName);
            }
        }
    }
}
