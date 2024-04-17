using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace TreeViewSelection
{
    public class TreeItem : INotifyPropertyChanged
    {
        public string Name { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetField(ref _isSelected, value);
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetField(ref _isExpanded, value);
        }

        public TreeItem Parent { get; set; }

        public List<TreeItem> Children { get; set; } = new List<TreeItem>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class TreeViewModel : INotifyPropertyChanged
    {
        private List<TreeItem> _items;

        public List<TreeItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructor
        public TreeViewModel()
        {
            Items = new List<TreeItem>();
            // Initialize your tree structure here or through a method
        }

        // Add methods to manipulate your Items collection here if needed
    }

    public partial class MainPage : Page
    {
        private readonly TreeViewModel _viewModel = new TreeViewModel();
        public MainPage()
        {
            this.InitializeComponent();

            // Create and add items to the viewModel.Items here
            var rootItem = new TreeItem { Name = "Root" };
            PopulateChildren(rootItem, 5); // Assuming this method populates your tree
            _viewModel.Items.Add(rootItem);

            this.DataContext = _viewModel;
        }

        private void PopulateChildren(TreeItem item, int depth)
        {
            if (depth <= 0) return;

            for (int i = 1; i <= 3; i++) // Example: Each node will have 3 children
            {
                var child = new TreeItem { Name = $"{item.Name} - Child {i}", Parent = item };
                item.Children.Add(child);
                PopulateChildren(child, depth - 1);
            }
        }

        private TreeItem GetItem1()
        {
            return _viewModel.Items[0].Children[0].Children[1];
        }

        private TreeItem GetItem2()
        {
            return _viewModel.Items[0].Children[1].Children[2];
        }

        private void Select(TreeItem item)
        {
            item.IsSelected = true;
            var parent = item;
            while (parent != null)
            {
                parent.IsExpanded = true;
                parent = parent.Parent;
            }
        }

        public void ExpandItem1(object sender, EventArgs args)
        {
            Select(GetItem1());
        }

        public void ExpandItem2(object sender, EventArgs args)
        {
            Select(GetItem2());
        }
    }
}
