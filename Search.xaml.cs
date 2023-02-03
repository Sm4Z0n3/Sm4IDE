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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace IDE
{
    /// <summary>
    /// Search.xaml 的交互逻辑
    /// </summary>
    public partial class Search : Window
    {
        MainWindow MainWindow = new MainWindow();
        string Context = "";

        public Search()
        {
            InitializeComponent();
            Context = MainWindow.CodeTextBox.Text;
            try
            {
                //this.MouseLeftButtonDown += (o, e) => { DragMove(); };
            }
            catch
            {
                //我他妈看不到就没问题
            }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)//查找
        {
            Context = MainWindow.CodeTextBox.Text;
        }

        private void Border_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)//替换
        {
            try
            {
                MessageBox.Show(MainWindow.GetText().Replace(_for.Text, to.Text));
                MainWindow.CodeTextBox.Text = MainWindow.GetText().Replace(_for.Text, to.Text);

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
