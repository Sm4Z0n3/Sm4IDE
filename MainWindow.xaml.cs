using System;
using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;
using System.Threading;
using System.Windows.Threading;
using DragEventArgs = System.Windows.DragEventArgs;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using System.Net.Sockets;
using System.Net;

namespace IDE
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (o, e) => { DragMove(); };
        }


        string Pat = "";

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileContent = File.ReadAllText(openFileDialog.FileName);
                Pat = openFileDialog.FileName;
                CodeTextBox.Text = fileContent;
                Title.Content = Pat;
            }
            
        }
        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CodeTextBox.Text = "";
        }
        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(Pat == "")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.OpenFile()))
                    {
                        streamWriter.Write(CodeTextBox.Text);
                    }
                }
            }
            else
            {
                File.WriteAllText(Pat, CodeTextBox.Text);
            }
        }
        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.OpenFile()))
                {
                    streamWriter.Write(CodeTextBox.Text);
                }
            }
        }

        private void CodeTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
               // int caretIndex = CodeTextBox.CaretIndex;
               // CodeTextBox.Text = CodeTextBox.Text.Insert(caretIndex, "     ");
                //CodeTextBox.CaretIndex = caretIndex + 5;
                e.Handled = true;
            }

            if (e.Key == Key.F5)
            {
                Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + "\\bin");
                File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "\\bin\\Index.html", CodeTextBox.Text);
                System.Diagnostics.Process.Start("http://Cdn.SmaZone.Club:20776");
                Thread thread = new System.Threading.Thread(StartLis);
                thread.Start();
            }
        }
        private void CodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CodeTextBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                var fontSize = CodeTextBox.FontSize + (e.Delta / 120);
                if (fontSize > 10 || fontSize < 50)
                {
                    if (fontSize >= 6)
                    {
                        CodeTextBox.FontSize = fontSize;
                    }
                }
            }
        }

        private void CodeTextBox_Drop(object sender, DragEventArgs e)
        {
            try
            {
                var fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                CodeTextBox.Text = File.ReadAllText(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CodeTextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        int zt = 0;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if(zt == 0)
            {
                this.WindowState = WindowState.Maximized;
                zt = 1;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                zt = 0;
            }
        }
        private void Border_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void SearchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Search search = new Search();
            search.Show();
        }

        public string GetText()
        {
            MessageBox.Show(CodeTextBox.Text);
            return CodeTextBox.Text;
        }

        private static void StartLis()
        {
            try
            {
                int port = 20776;
                TcpListener server = new TcpListener(IPAddress.Any, port);
                server.Start();

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    IPEndPoint remoteEP = (IPEndPoint)client.Client.RemoteEndPoint;
                    byte[] data = new byte[1024];
                    int bytesRead = stream.Read(data, 0, data.Length);
                    string request = Encoding.UTF8.GetString(data, 0, bytesRead);
                    if (request.Contains("GET") || request.Contains("POST"))
                    {
                        string[] requestParts = request.Split(' ');
                        string[] urlParts = requestParts[1].Split('?');
                        try
                        {
                            byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\nContent-Type: text/html\r\n\r\n" + File.ReadAllText(System.IO.Directory.GetCurrentDirectory()+"\\bin\\Index.html"));
                            stream.Write(response, 0, response.Length);
                        }
                        catch
                        {

                        }

                        client.Close();
                    }
                }
            }
            catch
            {
                
            }
        }

        private void RunMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + "\\bin");
            File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "\\bin\\Index.html", CodeTextBox.Text);
            System.Diagnostics.Process.Start("http://Cdn.SmaZone.Club:20776");
            Thread thread = new System.Threading.Thread(StartLis);
            thread.Start();
        }

        private void CSharp_Click(object sender, RoutedEventArgs e)
        {
            CodeTextBox.SyntaxHighlighting = CSharp.SyntaxHighlighting;
        }
        private void Java_Click(object sender, RoutedEventArgs e)
        {
            CodeTextBox.SyntaxHighlighting = Java.SyntaxHighlighting;
        }
        private void JS_Click(object sender, RoutedEventArgs e)
        {
            CodeTextBox.SyntaxHighlighting = JavaScript.SyntaxHighlighting;
        }
        private void HTML_Click(object sender, RoutedEventArgs e)
        {
            CodeTextBox.SyntaxHighlighting = HTML.SyntaxHighlighting;
        }
        private void PHP_Click(object sender, RoutedEventArgs e)
        {
            CodeTextBox.SyntaxHighlighting = PHP.SyntaxHighlighting;
        }
        private void CPP_Click(object sender, RoutedEventArgs e)
        {
            CodeTextBox.SyntaxHighlighting = CPP.SyntaxHighlighting;
        }
        private void VB_Click(object sender, RoutedEventArgs e)
        {
            CodeTextBox.SyntaxHighlighting = VB.SyntaxHighlighting;
        }

        private void Light_Click(object sender, RoutedEventArgs e)
        {
            CodeTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF484848"));
            BackBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2EFEFEF"));
            FileM.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF545454"));
            Edit.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF545454"));
            Language.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF545454"));
            Mode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF545454"));
            Title.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF272727"));
        }
        private void Dark_Click(object sender, RoutedEventArgs e)
        {
            CodeTextBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFABABAB"));
            BackBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2525252"));
            FileM.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFABABAB"));
            Edit.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFABABAB"));
            Language.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFABABAB"));
            Mode.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFABABAB"));
            Title.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }
    }

}
