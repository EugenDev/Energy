using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Energy.UI.Controls;
using Energy.UI.Windows;

namespace Energy.UI.Model
{
    public class TaskModel
    {
        public ObservableCollection<string> FeaturesNames { get; }

        public ObservableCollection<StationModel> Stations { get; }
        public ObservableCollection<ConsumerModel> Consumers { get; }
        public ObservableCollection<LinkModel> Links { get; }

        public CompositeCollection Controls { get; }
        
        public TaskModel()
        {
            FeaturesNames = new ObservableCollection<string>();

            Stations = new ObservableCollection<StationModel>();
            Consumers = new ObservableCollection<ConsumerModel>();
            Links = new ObservableCollection<LinkModel>();

            Controls = new CompositeCollection
            {
                new CollectionContainer {Collection = Links},
                new CollectionContainer {Collection = Stations},
                new CollectionContainer {Collection = Consumers}
            };
        }

        public void AddParticipant(string name, ParticipantType participantType)
        {
            switch (participantType)
            {
                case ParticipantType.Consumer:
                    Consumers.Add(new ConsumerModel(name, FeaturesNames));
                    break;

                case ParticipantType.Station:
                    Stations.Add(new StationModel(name, FeaturesNames));
                    break;

                default:
                    throw new InvalidOperationException("Unhandled enum value");
            }
        }

        public void AddFeature(string featureName)
        {
            FeaturesNames.Add(featureName);
            AddFeatureToCollection(Stations, featureName);
            AddFeatureToCollection(Consumers, featureName);
        }
        
        private static void AddFeatureToCollection(IEnumerable<ModelBase> featuredItems, string featureName)
        {
            foreach (var item in featuredItems)
            {
                item.Add(featureName, 0.0);
            }
        }

        public void AddLink(ModelBase from, ModelBase to)
        {
            var link = new LinkModel(from, to);
            var editWindow = new EditLinkWindow(link, true);
            editWindow.ShowDialog();
            var dialogResult = editWindow.DialogResult;
            if (dialogResult.HasValue && dialogResult.Value)
                Links.Add(link);
        }

        //public void DeleteStation(StationModel station)
        //{
        //    FeaturesModel.RemoveStation(station.Name);
        //    GraphicModel.Models.Remove(station);
        //}

        //public void DeleteConsumer(ConsumerModel consumer)
        //{
        //    FeaturesModel.RemoveConsumer(consumer.Name);
        //    GraphicModel.Models.Remove(consumer);
        //}

        private DataGridTextColumn CreateFeatureColumn(string featureName)
        {
            return new DataGridTextColumn
            {
                Header = featureName,
                Binding = new Binding { Path = new PropertyPath(featureName) },
                IsReadOnly = featureName.Equals("Расстояние") || featureName.Equals("Проводимость")
            };
        }

        public DataGridTextColumn[] GetColumns()
        {
            return FeaturesNames
                .Select(CreateFeatureColumn)
                .ToArray();
        }

        //public void RemoveStation(string name)
        //{
        //    var removedStation = Stations.First(x => x.Name.Equals(name));
        //    Stations.Remove(removedStation);
        //}

        //public void RemoveConsumer(string name)
        //{
        //    var removedConsumer = Consumers.First(x => x.Name.Equals(name));
        //    Consumers.Remove(removedConsumer);
        //}

        //public void RemoveFeature(string featureName)
        //{
        //    RemoveFeatureFromCollection(Stations, featureName);
        //    RemoveFeatureFromCollection(Consumers, featureName);
        //    FeaturesNames.Remove(featureName);
        //}

        //private static void RemoveFeatureFromCollection(IEnumerable<FeaturedItem> featuredItem, string featureName)
        //{
        //    foreach (var item in featuredItem)
        //    {
        //        item.Remove(featureName);
        //    }
        //}
    }
}
