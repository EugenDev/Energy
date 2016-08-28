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
            FeaturesNames = new ObservableCollection<string> {"Расстояние", "Проводимость"};

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

        public void DeleteParticipant(ModelBase item)
        {
            var partisipantType = item.ParticipantType;
            switch (partisipantType)
            {
                case ParticipantType.Station:
                    Stations.Remove(item as StationModel);
                    break;

                case ParticipantType.Consumer:
                    Consumers.Remove(item as ConsumerModel);
                    break;

                default:
                    throw new InvalidOperationException("Invalid ParticipantType");
            }

            var deletedLinks = Links.Where(l => l.From == item || l.To == item).ToList();
            foreach (var deletedLink in deletedLinks)
                Links.Remove(deletedLink);
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

        public void RemoveFeature(string featureName)
        {
            RemoveFeatureFromCollection(Stations, featureName);
            RemoveFeatureFromCollection(Consumers, featureName);
            FeaturesNames.Remove(featureName);
        }

        private static void RemoveFeatureFromCollection(IEnumerable<FeaturedItem> featuredItem, string featureName)
        {
            foreach (var item in featuredItem)
                item.Remove(featureName);
        }

        public void AddLink(ModelBase from, ModelBase to)
        {
            var link = new LinkModel(from, to);
            if (App.IsDebug)
            {
                var r = new Random(DateTime.Now.Millisecond);
                link.Distance = r.Next(5, 20);
                link.Conduction = r.Next(1, 4);
            }
            else
            {
                var editWindow = new EditLinkWindow(link, true);
                editWindow.ShowDialog();
                var dialogResult = editWindow.DialogResult;
                if (!dialogResult.HasValue || !dialogResult.Value)
                    return;
            }
            Links.Add(link);
        }

        public void DeletLink(LinkModel linkModel)
        {
            Links.Remove(linkModel);
        }

        public DataGridTextColumn[] GetColumns()
        {
            return FeaturesNames
                .Select(CreateFeatureColumn)
                .ToArray();
        }

        private DataGridTextColumn CreateFeatureColumn(string featureName)
        {
            return new DataGridTextColumn
            {
                Header = featureName,
                Binding = new Binding { Path = new PropertyPath(featureName) },
                IsReadOnly = featureName.Equals("Расстояние") || featureName.Equals("Проводимость")
            };
        }
    }
}
