using da2mvc.framework.collection.view;
using R3EHUDManager.motec.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace R3EHUDManager.motec.view
{
    /// <summary>
    /// Logique d'interaction pour MotecListView.xaml
    /// </summary>
    public partial class MotecMenuView : UserControl, ICollectionView<MotecModel>
    {
        public event EventHandler Disposed;

        public MotecMenuView()
        {
            InitializeComponent();
        }

        public void Add(MotecModel[] models)
        {
            foreach(var model in models)
            {
                list.Items.Add(model);
            }
        }

        public void Remove(MotecModel[] models)
        {
            foreach (var model in models)
            {
                list.Items.Remove(model);
            }
        }

        public void Clear()
        {
            list.Items.Clear();
        }

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
