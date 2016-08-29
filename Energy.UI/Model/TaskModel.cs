﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
            if (Stations.Any(s => s.Name.Equals(name)) || Consumers.Any(c => c.Name.Equals(name)))
                throw new DuplicateNameException();

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
            if (FeaturesNames.Any(featureName.Equals))
                throw new DuplicateNameException();

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

        public void AddLink(LinkModel link)
        {
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
