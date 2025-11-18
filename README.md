
# LoraViewerApp

LoraViewerApp is a desktop application based on WPF (.NET) for browsing and viewing Lora model names, preview images, and trigger words.

## Features
- Display Lora list (name, preview image, trigger words)
- Show large preview image and all trigger words in the details area
- One-click copy for Lora name and each set of trigger words

## How to Use
1. In the `MainWindow.xaml.cs` file, find the following code:
	```csharp
	string loraFolderPath = "D:/your/Lora/data/folder/path";
	string defaultImagePath = "Assets/DefaultImage.png";
	LoraList = LoraParser.ParseLoraFolder(loraFolderPath, defaultImagePath);
	```
	Set `loraFolderPath` to the actual folder path where your Lora files are stored.
	Adjust the default image path as needed for your resources.
2. Save and run the application to browse and copy the information you need.

## Project Structure
- `LoraViewerApp/` Main application directory
- `Utils/LoraParser.cs` Data parsing utility class
- `Assets/DefaultImage.png` Default image resource

## Development Environment
- .NET 6/7/8
- WPF

## Publish & Distribution
To publish the app, use the following command:
```
dotnet publish -c Release -r win-x64 --self-contained
```

## Contributing
Issues and PRs are welcome.
