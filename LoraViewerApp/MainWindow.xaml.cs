using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LoraViewerApp.Utils;
using System.IO;
using Newtonsoft.Json; // 需要安装 Newtonsoft.Json 包

namespace LoraViewerApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<LoraModel> LoraList { get; set; }
        private string currentLoraFolderPath = ""; // 当前 Lora 文件夹路径
        private string defaultImagePath = "Assets/DefaultImage.jpg"; // 默认图片路径
        private string configFilePath = "config.json"; // 配置文件路径

        public MainWindow()
        {
            InitializeComponent();

            // 初始化 Lora 文件夹路径
            currentLoraFolderPath = ""; // 替换为你的默认路径
            LoadLoraFolder(currentLoraFolderPath);

            // 加载保存的路径
            LoadSavedPath();
        }

        // 加载 Lora 文件夹并刷新界面
        private void LoadLoraFolder(string folderPath)
        {
            currentLoraFolderPath = folderPath;
            LoraList = LoraParser.ParseLoraFolder(folderPath, defaultImagePath);
            LoraListView.ItemsSource = LoraList;
            CurrentPathLabel.Text = $"当前路径：{currentLoraFolderPath}";
        }

        // 当用户点击列表项时，获取选中的 Lora 数据。
        private void LoraListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = LoraListView.SelectedItem as LoraModel;
            if (selected != null)
            {
                DetailName.Text = selected.Name;
                if (!string.IsNullOrEmpty(selected.ImagePath))
                {
                    Uri imageUri;
                    if (System.IO.Path.IsPathRooted(selected.ImagePath))
                        imageUri = new Uri(selected.ImagePath, UriKind.Absolute);
                    else
                        imageUri = new Uri(selected.ImagePath, UriKind.Relative);

                    DetailImage.Source = new BitmapImage(imageUri);
                }
                else
                {
                    DetailImage.Source = null;
                }
                TriggerListControl.ItemsSource = selected.TriggerList;
            }
        }


        private void CopySingleTrigger_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var trigger = (btn?.DataContext as string) ?? "";
            Clipboard.SetText(trigger);
        }
        private void CopyName_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(DetailName.Text);
        }

        private void ChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            dialog.SelectedPath = currentLoraFolderPath;
            if (dialog.ShowDialog(this) == true)
            {
                LoadLoraFolder(dialog.SelectedPath);
            }
        }

        private void LoadSavedPath()
        {
            if (File.Exists(configFilePath))
            {
                try
                {
                    var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFilePath));
                    if (config != null && !string.IsNullOrEmpty(config.LastPath))
                    {
                        currentLoraFolderPath = config.LastPath;
                        LoadLoraFolder(currentLoraFolderPath);
                    }
                }
                catch
                {
                    // 如果配置文件损坏，忽略错误
                }
            }
        }

        private void SavePath()
        {
            var config = new Config { LastPath = currentLoraFolderPath };
            File.WriteAllText(configFilePath, JsonConvert.SerializeObject(config));
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            SavePath(); // 在关闭时保存路径
        }
    }

    public class LoraModel
    {
        public string? Name { get; set; }
        public string? Trigger { get; set; }
        public string? ImagePath { get; set; }
        public List<string> TriggerList { get; set; } = new List<string>();
    }

    public class Config
    {
        public string? LastPath { get; set; }
    }
}