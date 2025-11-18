using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using LoraViewerApp;
using System.Linq;

namespace LoraViewerApp.Utils
{
    public static class LoraParser
    {
        // folderPath: 指定的Lora数据文件夹路径
        // defaultImagePath: 默认图片路径
        public static ObservableCollection<LoraModel> ParseLoraFolder(string folderPath, string defaultImagePath)
        {
            var loraList = new ObservableCollection<LoraModel>();
            if (!Directory.Exists(folderPath)) return loraList;

            // 只查找 .safetensors 文件
            var safetensorFiles = Directory.GetFiles(folderPath, "*.safetensors");
            foreach (var safetensorFile in safetensorFiles)
            {
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(safetensorFile);
                var triggerPath = Path.Combine(folderPath, fileNameWithoutExt + ".txt");
                var imagePathJpeg = Path.Combine(folderPath, fileNameWithoutExt + ".jpeg");
                var imagePathJpg = Path.Combine(folderPath, fileNameWithoutExt + ".jpg");
                var imagePathPng = Path.Combine(folderPath, fileNameWithoutExt + ".png");
                string previewImage = defaultImagePath;
                if (File.Exists(imagePathJpeg))
                    previewImage = imagePathJpeg;
                else if (File.Exists(imagePathJpg))
                    previewImage = imagePathJpg;
                else if (File.Exists(imagePathPng))
                    previewImage = imagePathPng;

                var trigger = File.Exists(triggerPath) ? File.ReadAllText(triggerPath).Trim() : "";

                var triggerLines = File.Exists(triggerPath)
                    ? File.ReadAllLines(triggerPath).Where(line => !string.IsNullOrWhiteSpace(line)).ToList()
                    : new List<string>();

                loraList.Add(new LoraModel
                {
                    Name = fileNameWithoutExt,
                    Trigger = trigger,
                    ImagePath = previewImage,
                    TriggerList = triggerLines
                });
            }
            return loraList;
        }
    }
}
