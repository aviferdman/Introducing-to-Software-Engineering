using KanbanProjectWPF.PresentationLayer;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Milstone2;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using System.Data;
using System.Windows.Media;

namespace KanbanProjectWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private Board MainB;
        private PresentationBoard selectedTask;
        private DataGrid selectedData;
        private ObservableCollection<DataGrid> columns;
        private DataGrid col;
        private ObservableCollection<PresentationBoard> tasks;
        private string userName;
        private int columnCounter;
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow(string userName, string password)
        {
            InitializeComponent();
            MainB = SystemInterface.getBoard(userName);
            this.userName = userName;
            show(MainB);
        }
        public void show(Board MainB)
        {
            this.columns = new ObservableCollection<DataGrid>();
            this.tasks = new ObservableCollection<PresentationBoard>();
            var hashColumns = MainB.getColumns();
            this.columnCounter = MainB.getCurrColumnCount();
            foreach (var c in hashColumns.Values)
            {
                this.col = new DataGrid();
                var hashTasks = ((Column)c).getTasksHash();
                DataGridTextColumn colm = new DataGridTextColumn();
                col.AutoGenerateColumns = false;
                String cp;
                if (((Column)c).getColumnMaxTasks() != int.MaxValue)
                {
                    cp = ((Column)c).getName().ToString() + " (" + ((Column)c).getCurrentTaskCount() + "/" + ((Column)c).getColumnMaxTasks() + ")";
                }
                else
                {
                    cp = ((Column)c).getName().ToString() + " " + "(Unlimited capacity)";

                }
                foreach (var t in hashTasks.Values)
                {
                    PresentationBoard p = new PresentationBoard(((Column)c).getName(), this.columnCounter, ((Task)t).getTitle(), ((Task)t).getDescription(), DateTime.Parse(((Task)t).getDueDate()), ((Task)t).getCreationDate(), ((Task)t).getTaskUID().ToString());
                    tasks.Add(p);
                }
                colm.MinWidth = 350;
                colm.Header = cp;
                colm.Binding = new Binding("Task");
                colm.CanUserSort = true;
                colm.SortMemberPath = "DueDate";
                col.Columns.Add(colm);
                col.IsReadOnly = true;
                col.FontSize = 12;
                col.Name = ((Column)c).getName().ToString().Replace(" ", "_");
                col.Tag = this.columnCounter;
                this.columns.Insert(0, col);
                if (this.columnCounter != MainB.getCurrColumnCount())
                {
                    col.LoadingRow += Col_LoadingRow;
                }
                col.AllowDrop = true;
                col.SelectionChanged += Col_SelectionChanged;
                col.PreviewMouseLeftButtonDown += Col_PreviewMouseLeftButtonDown;
                col.PreviewMouseMove += Col_PreviewMouseMove;
                col.PreviewMouseUp += Col_PreviewMouseUp;
                col.Drop += Col_Drop;
                col.ItemsSource = tasks;
                this.columnCounter--;
                this.tasks = new ObservableCollection<PresentationBoard>();
            }
            this.columnCounter = MainB.getCurrColumnCount();
            MainBoard.SelectionChanged += MainBoard_SelectionChanged;
            MainBoard.ItemsSource = columns;
        }

        private void Col_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGrid d = (DataGrid)sender;
            var currRow = (PresentationBoard)e.Row.DataContext;
            DateTime dueDate = currRow.DueDate;
            if (dueDate.CompareTo(DateTime.Now) < 1)
            {
                if (e.Row != null)
                {
                    int days = DateTime.Now.Subtract(dueDate).Days;
                    if (days > 1)
                    {
                        string duedatesover = DateTime.Now.Subtract(dueDate).Days.ToString();
                        string toolTipText = "You passed your due date by " + duedatesover + " days";
                        e.Row.ToolTip = toolTipText;
                    }
                    else if (days == 1)
                    {
                        string duedatesover = DateTime.Now.Subtract(dueDate).Days.ToString();
                        string toolTipText = "You passed your due date by 1 day";
                        e.Row.ToolTip = toolTipText;
                    }
                    else
                    {
                        string duedatesover = DateTime.Now.Subtract(dueDate).Days.ToString();
                        string toolTipText = "You passed your due date";
                        e.Row.ToolTip = toolTipText;
                    }
                }
                e.Row.Background = new SolidColorBrush(Colors.IndianRed);
                e.Row.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void Col_Drop(object sender, DragEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg == null) return;
            var dgSrc = e.Data.GetData("DragSource") as DataGrid;
            var data = e.Data.GetData(typeof(PresentationBoard));
            if (dgSrc == null || data == null) return;
            // Implement move data here, depends on your implementation
            int dgTag = Int32.Parse(dg.Tag.ToString());
            int dgSTag = Int32.Parse(dgSrc.Tag.ToString());

            if (dgTag > dgSTag && dgSTag + 1 == dgTag)
            {
                Move_Task_Click((PresentationBoard)data);
            }
            else
            {
                MessageBox.Show("Illegal task move");
            }

        }

        private Point? _startPoint;

        private void Col_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }

        private void Col_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            // No drag operation
            if (_startPoint == null)
                return;

            var dg = sender as DataGrid;
            if (dg == null) return;
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint.Value - mousePos;
            // test for the minimum displacement to begin the drag
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {

                // Get the dragged DataGridRow
                var DataGridRow =
                    FindAnchestor<DataGridRow>((DependencyObject)e.OriginalSource);

                if (DataGridRow == null)
                    return;
                // Find the data behind the DataGridRow
                var dataTodrop = (PresentationBoard)dg.ItemContainerGenerator.
                    ItemFromContainer(DataGridRow);

                if (dataTodrop == null) return;

                // Initialize the drag & drop operation
                var dataObj = new DataObject(dataTodrop);
                dataObj.SetData("DragSource", sender);
                DragDrop.DoDragDrop(dg, dataObj, DragDropEffects.Copy);
                _startPoint = null;
            }
        }

        private void Col_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _startPoint = null;
        }



        // Helper to search up the VisualTree
        private static T FindAnchestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }



        private void MainBoard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((DataGrid)((ListView)sender).SelectedValue != null)
            {
                this.selectedData = (DataGrid)((ListView)sender).SelectedValue;
            }
            else
            {
                this.selectedData = null;
            }
        }

        private void Col_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((PresentationBoard)((DataGrid)sender).SelectedValue) != null)
            {
                this.selectedTask = ((PresentationBoard)((DataGrid)sender).SelectedValue);
            }
            else
            {
                this.selectedTask = null;
            }
        }

        private void Move_Task_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTask != null)
            {
                InfoObject info = SystemInterface.moveTask(userName, selectedTask.getTaskUID(), selectedTask.getTasColumn());
                if (info.getIsSucceeded())
                {
                    this.show(SystemInterface.getBoard(userName));
                    this.selectedTask = null;
                }
                else
                {
                    MessageBox.Show(info.getMessage());
                }
            }
            else
            {
                MessageBox.Show("Please select task");
            }
        }

        private void Move_Task_Click(PresentationBoard currRow)
        {
            if (selectedTask != null)
            {
                InfoObject info = SystemInterface.moveTask(userName, currRow.getTaskUID(), currRow.getTasColumn());
                if (info.getIsSucceeded())
                {
                    this.show(SystemInterface.getBoard(userName));
                    this.selectedTask = null;
                }
                else
                {
                    MessageBox.Show(info.getMessage());
                }
            }
            else
            {
                MessageBox.Show("Please select task");
            }
        }

        private void Power_Button_Click(object sender, RoutedEventArgs e)
        {
            SystemInterface.logout(userName);
            Application.Current.Shutdown();
        }

        private void Add_New_Task_Click(object sender, RoutedEventArgs e)
        {
            AddTask addTask = new AddTask(userName, this);
            addTask.Show();
        }

        private void Log_Off_Button_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            SystemInterface.logout(userName);
            this.Close();
            login.Show();

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Add_Column_Button_Click(object sender, RoutedEventArgs e)
        {
            AddCoulmn addCoulmn = new AddCoulmn(userName, this);
            addCoulmn.Show();
        }

        private void Remove_Column_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedData != null)
            {
                InfoObject info = SystemInterface.removeColumn(userName, this.selectedData.Name.ToString().Replace("_", " "));
                if (info.getIsSucceeded())
                {
                    this.selectedData = null;
                    this.show(SystemInterface.getBoard(userName));
                }
                else
                {
                    MessageBox.Show(info.getMessage());
                }
            }
            else
            {
                MessageBox.Show("Please select column");
            }
        }

        private void Move_Column_Right_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedData != null)
            {
                InfoObject info = SystemInterface.moveColumn(userName, (int)this.selectedData.Tag);
                if (info.getIsSucceeded())
                {
                    this.selectedData = null;
                    this.show(SystemInterface.getBoard(userName));
                }
                else
                {
                    MessageBox.Show(info.getMessage());
                }
            }
            else
            {
                MessageBox.Show("Please select column");
            }
        }

        private void Move_Column_Left_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedData != null)
            {
                InfoObject info = SystemInterface.moveColumnBack(userName, (int)this.selectedData.Tag);
                if (info.getIsSucceeded())
                {
                    this.selectedData = null;
                    this.show(SystemInterface.getBoard(userName));
                }
                else
                {
                    MessageBox.Show(info.getMessage());
                }
            }
            else
            {
                MessageBox.Show("Please select column");
            }
        }

        private void Edit_Task_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTask != null)
            {
                Edit TaskEdit = new Edit(userName, selectedTask, this);

                if (!(this.selectedTask.getTasColumn() == columns.Count))
                {
                    TaskEdit.Show();
                    this.selectedTask = null;
                }
                else
                {
                    MessageBox.Show("Cant edit task in last column");

                    if (!(this.selectedTask.getTasColumn().Equals(columns.Count)))
                    {
                        TaskEdit.Show();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select task");
            }

        }

        private void Sort_By_Input_Click(object sender, RoutedEventArgs e)
        {
            Filter f = new Filter(this);
            f.Show();
        }

        private void Eidt_Column_Capacity_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedData != null)
            {
                ChangeColumnCapacity ccc = new ChangeColumnCapacity(userName, this, this.selectedData);
                ccc.Show();
            }
            else
            {
                MessageBox.Show("Select column please");
            }
        }


        public void sortByWord(String toSort)
        {
            this.columns = new ObservableCollection<DataGrid>();
            this.tasks = new ObservableCollection<PresentationBoard>();
            var hashColumns = MainB.getColumns();
            this.columnCounter = MainB.getCurrColumnCount();
            foreach (var c in hashColumns.Values)
            {
                this.col = new DataGrid();
                var hashTasks = ((Column)c).getTasksHash();
                DataGridTextColumn colm = new DataGridTextColumn();
                col.AutoGenerateColumns = false;
                String cp;
                if (((Column)c).getColumnMaxTasks() != int.MaxValue)
                {
                    cp = ((Column)c).getName().ToString() + " (" + ((Column)c).getCurrentTaskCount() + "/" + ((Column)c).getColumnMaxTasks() + ")";
                }
                else
                {
                    cp = ((Column)c).getName().ToString() + " " + "(Unlimited capacity)";

                }
                foreach (var t in hashTasks.Values)
                {
                    String Tuser = ((Column)c).getName();
                    int CCounter = this.columnCounter;
                    String TTile = ((Task)t).getTitle();
                    String TDes = ((Task)t).getDescription();
                    DateTime TDDate = DateTime.Parse(((Task)t).getDueDate());
                    String TCDate = ((Task)t).getCreationDate();

                    if (Tuser.ToLower().Contains(toSort) | CCounter.ToString().ToLower().Contains(toSort) | TTile.ToLower().Contains(toSort) | TDes.ToLower().Contains(toSort) | TCDate.ToLower().Contains(toSort))
                    {
                        PresentationBoard p = new PresentationBoard(Tuser, this.columnCounter, TTile, TDes, TDDate, TCDate, ((Task)t).getTaskUID().ToString());
                        this.tasks.Add(p);
                    }
                }
                colm.MinWidth = 350;
                colm.Header = cp;
                colm.Binding = new Binding("Task");
                colm.CanUserSort = true;
                colm.SortMemberPath = "DueDate";
                col.Columns.Add(colm);
                col.IsReadOnly = true;
                col.FontSize = 12;
                col.Name = ((Column)c).getName().ToString().Replace(" ", "_");
                col.Tag = this.columnCounter;
                this.columns.Insert(0, col);
                if (this.columnCounter != MainB.getCurrColumnCount())
                {
                    col.LoadingRow += Col_LoadingRow;
                }
                col.AllowDrop = true;
                col.SelectionChanged += Col_SelectionChanged;
                col.PreviewMouseLeftButtonDown += Col_PreviewMouseLeftButtonDown;
                col.PreviewMouseMove += Col_PreviewMouseMove;
                col.PreviewMouseUp += Col_PreviewMouseUp;
                col.Drop += Col_Drop;
                col.ItemsSource = tasks;
                this.columnCounter--;
                this.tasks = new ObservableCollection<PresentationBoard>();
            }
            this.columnCounter = MainB.getCurrColumnCount();
            MainBoard.SelectionChanged += MainBoard_SelectionChanged;
            MainBoard.ItemsSource = columns;
        }

        private void Stop_Sort_Click(object sender, RoutedEventArgs e)
        {
            this.show(SystemInterface.getBoard(userName));
        }
    }
}