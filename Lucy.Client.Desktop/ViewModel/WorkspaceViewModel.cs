using Lucy.Client.Desktop.Model;
using Lucy.Client.Desktop.Service;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows;

namespace Lucy.Client.Desktop.ViewModel
{
    /// <summary>
    /// Control the display logic of the Workspace page
    /// </summary>
    public class WorkspaceViewModel : BindableBase
    {
        /// <summary>
        /// Store the instance of workspace service
        /// </summary>
        private WorkspaceService service = new WorkspaceService();

        /// <summary>
        /// Create a new instance of <see cref="WorkspaceViewModel"/>
        /// </summary>
        public WorkspaceViewModel()
        {

            ToggleControl = new DelegateCommand<UIElement>(this.OnToggleControl);

            AddWorkspaceCommand = new DelegateCommand<TextBox>(
               this.OnAddWorkspaceCommand);

            RemoveWorkspaceCommand = new DelegateCommand(
                this.OnRemoveWorkspaceCommand);

            OpenWorkspace = new DelegateCommand(
                this.OnOpenWorkspace);

            ObservableCollection<WorkspaceModel> data = new ObservableCollection<WorkspaceModel>(
              service.Load());

            CurrentWorkspaces = new ListCollectionView(
                data);

            CurrentWorkspaces.MoveCurrentToPosition(-1);
            CurrentWorkspaces.CurrentChanged += CurrentWorkspaces_CurrentChanged;

        }


        void CurrentWorkspaces_CurrentChanged(object sender, EventArgs e)
        {
            if (CurrentWorkspaces.CurrentItem != null)
            {
                this.OnOpenWorkspace();
            }
        }

        /// <summary>
        /// Get a command that trigger of workspace creation
        /// </summary>
        public ICommand AddWorkspaceCommand { get; private set; }

        /// <summary>
        /// Get a command that trigger of workspace removal
        /// </summary>
        public ICommand RemoveWorkspaceCommand { get; private set; }

        /// <summary>
        /// Get a command that trigger the workspace opening
        /// </summary>
        public ICommand OpenWorkspace { get; private set; }

        public ICommand ToggleControl { get; private set; }

        public void OnToggleControl(UIElement ctrl)
        {
            ctrl.Visibility = ctrl.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;

        }

        public void OnOpenWorkspace()
        {
            if (this.CurrentWorkspaces == null || this.CurrentWorkspaces.CurrentItem == null)
            {
                return;
            }

            var current = (WorkspaceModel)this.CurrentWorkspaces.CurrentItem;
            App.ActiveWorkSpace = current;
            ((NavigationWindow)App.Current.MainWindow)
                .Navigate(
                new Uri("\\DocumentStore.xaml", UriKind.Relative),
                current);
        }

        /// <summary>
        /// Handle the command of workspace creation
        /// </summary>
        /// <param name="textbox">Instance of textbox</param>
        private void OnAddWorkspaceCommand(TextBox textbox)
        {
            var newWorkspaceName = textbox.Text;
            if (!this.service.Exist(newWorkspaceName))
            {
                var workspace = this.service.Create(newWorkspaceName);
                ((ObservableCollection<WorkspaceModel>)this.CurrentWorkspaces.SourceCollection).Add(workspace);
            }
            textbox.Text = string.Empty;
        }


        /// <summary>
        /// Handle the command of workspace removal
        /// </summary>
        /// <param name="obj">not useds</param>
        private void OnRemoveWorkspaceCommand()
        {

            var current = App.ActiveWorkSpace;
            service.Remove(current);
            ((NavigationWindow)App.Current.MainWindow)
                .Navigate(
                new Uri("\\Workspace.xaml", UriKind.Relative),
                current);
        }


        /// <summary>
        /// Get the collection of Workspace
        /// </summary>
        public ICollectionView CurrentWorkspaces
        {
            get;
            private set;
        }
    }
}
