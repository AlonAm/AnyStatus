﻿using AnyStatus.API.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;

namespace AnyStatus.Apps.Windows.Infrastructure.Mvvm.Controls.PropertyGrid
{
    public class DropDownPropertyViewModel : PropertyViewModelBase
    {
        private IEnumerable<NameValueItem> _items;

        public DropDownPropertyViewModel(PropertyInfo propertyInfo, object source) : base(propertyInfo, source)
        {
            Name = propertyInfo.Name;
        }

        public DropDownPropertyViewModel(PropertyInfo propertyInfo, object source, IEnumerable<NameValueItem> items) : this(propertyInfo, source)
        {
            Items = items;
        }

        public DropDownPropertyViewModel(PropertyInfo propertyInfo, object source, IItemsSource itemsSource, IEnumerable<IPropertyViewModel> properties, bool autoload) : this(propertyInfo, source)
        {
            Refresh = () => Items = itemsSource.GetItems(source);

            Cascade(propertyInfo, properties);

            if (autoload)
            {
                Refresh();
            }
        }

        public DropDownPropertyViewModel(PropertyInfo propertyInfo, object source, IAsyncItemsSource asyncItemsSource, IEnumerable<IPropertyViewModel> properties, bool autoload) : this(propertyInfo, source)
        {
            Refresh = async () => Items = await asyncItemsSource.GetItemsAsync(source);

            Cascade(propertyInfo, properties);

            if (autoload)
            {
                Refresh();
            }
        }

        public string Name { get; set; }

        public Action Refresh { get; set; }

        public ICommand SelectionChanged { get; set; }

        public IEnumerable<NameValueItem> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        private void Cascade(PropertyInfo propertyInfo, IEnumerable<IPropertyViewModel> properties)
        {
            if (propertyInfo.GetCustomAttribute<RefreshAttribute>() is RefreshAttribute refreshAttribute)
            {
                SelectionChanged = new Command(_ =>
                {
                    foreach (var property in properties)
                    {
                        if (property is DropDownPropertyViewModel dropDownProperty && dropDownProperty.Name.Equals(refreshAttribute.Name))
                        {
                            dropDownProperty.Refresh();
                        }
                    }
                });
            }
        }
    }
}
