using HealthCodeLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace HealthCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog openFileDlg;
        private Graph graph;
        public MainWindow()
        {
            InitializeComponent();
            openFileDlg = new OpenFileDialog();
            openFileDlg.AddExtension = true;
            openFileDlg.Filter = "txt|*.txt";
            graph = new Graph();
        }

        private void LoadStudents(object sender, RoutedEventArgs e)
        {
            if (openFileDlg.ShowDialog() == true)
            {
                CBox_Source.Items.Clear();
                CBox_Target.Items.Clear();
                foreach (var item in File.ReadAllLines(openFileDlg.FileName))
                {
                    int i = item.IndexOf(',');
                    if (i >= 0)
                    {
                        var id = int.Parse(item[..i]);
                        var name = item[(i + 1)..];
                        var stu = new Student(id, name);
                        CBox_Source.Items.Add(stu);
                        CBox_Target.Items.Add(stu);
                    }
                }
            }
        }

        private void LoadContacts(object sender, RoutedEventArgs e)
        {

            if (openFileDlg.ShowDialog() == true)
            {
                graph.ClearData();
                foreach (var item in File.ReadAllLines(openFileDlg.FileName))
                {
                    int i = item.IndexOf(',');
                    if (i >= 0)
                    {
                        var leftId = int.Parse(item[..i]);
                        var rightId = int.Parse(item[(i + 1)..]);
                        graph.AddContact(leftId, rightId);
                    }
                }
            }
        }

        private void CalcInfactionRate(object sender, SelectionChangedEventArgs e)
        {
            if (CBox_Source.SelectedIndex >= 0 && CBox_Target.SelectedIndex >= 0)
            {
                var sourceId = (CBox_Source.SelectedItem as Student)?.Id;
                var targetId = (CBox_Target.SelectedItem as Student)?.Id;
                if (sourceId is not null && targetId is not null)
                {
                    var rate = graph.GetInfactionRate(sourceId.Value, targetId.Value);
                    if (rate == 0)
                    {
                        Rect_Health.Fill = Brushes.LightGreen;
                        TBlock_Rate.Foreground = Brushes.Black;
                        TBlock_Rate.Text = "0%";
                    }
                    else
                    {
                        Rect_Health.Fill = Brushes.Crimson;
                        TBlock_Rate.Foreground = Brushes.White;
                        TBlock_Rate.Text = $"{rate * 100:N2}%";
                    }
                }
            }
        }
    }
}
