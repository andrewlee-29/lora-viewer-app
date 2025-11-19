# Lora Viewer App

A desktop application built with WPF (.NET 9) for viewing and managing Lora (Low-Rank Adaptation) model files. This tool provides an intuitive interface to browse Lora models with their preview images and trigger words, making it easier for AI image generation enthusiasts to manage their Lora collections.

## 📋 Features

- **Browse Lora Models**: Display all `.safetensors` files in a selected folder
- **Preview Images**: Automatically loads associated preview images (`.jpg`, `.jpeg`, `.png`)
- **Trigger Words Management**: 
  - View trigger words from associated `.txt` files
  - Display multiple trigger words line-by-line
  - Quick copy functionality for individual trigger words
- **Quick Copy**: One-click copy for Lora names and trigger words to clipboard
- **Path Persistence**: Automatically saves and restores the last browsed folder
- **Clean UI**: Split-view layout with list and detail panels

## 🖼️ Screenshots

### Main Interface
The application features a two-column layout:
- **Left Panel**: List of Lora models with thumbnails
- **Right Panel**: Detailed view with full-size preview and trigger words

## 🏗️ Architecture

### Project Structure

```
lora-viewer-app/
├── LoraViewerApp/
│   ├── MainWindow.xaml           # Main UI layout
│   ├── MainWindow.xaml.cs        # Main window logic
│   ├── App.xaml                  # Application resources
│   ├── App.xaml.cs               # Application entry point
│   ├── Assets/
│   │   └── DefaultImage.jpg      # Fallback image for models without previews
│   ├── Utils/
│   │   └── LoraParser.cs         # Lora folder parsing logic
│   └── LoraViewerApp.csproj      # Project configuration
├── TestingLora/                   # Example Lora files for testing
└── lora-viewer-app.sln           # Visual Studio solution file
```

### Key Components

#### 1. **MainWindow (MainWindow.xaml & MainWindow.xaml.cs)**
- Main application window with split-panel layout
- Handles UI interactions and event management
- Manages folder selection and configuration persistence
- Features:
  - Folder browser integration using `Ookii.Dialogs.Wpf`
  - ListView for displaying Lora collection
  - Detail panel for selected Lora information
  - Clipboard operations for quick copying

#### 2. **LoraParser (Utils/LoraParser.cs)**
- Static utility class for parsing Lora folders
- Scans directory for `.safetensors` files
- Loads associated files:
  - `.txt` files for trigger words
  - `.jpg`, `.jpeg`, or `.png` for preview images
- Returns `ObservableCollection<LoraModel>` for data binding

#### 3. **LoraModel (MainWindow.xaml.cs)**
- Data model representing a Lora file
- Properties:
  - `Name`: Filename without extension
  - `Trigger`: Combined trigger words (for display)
  - `ImagePath`: Path to preview image
  - `TriggerList`: List of individual trigger words

#### 4. **Config (MainWindow.xaml.cs)**
- Configuration model for persistence
- Stores `LastPath` for folder location memory

## 🚀 Getting Started

### Prerequisites

- Windows OS (Windows 10 or later recommended)
- .NET 9.0 SDK or later
- Visual Studio 2022 (or compatible IDE with WPF support)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/andrewlee-29/lora-viewer-app.git
   cd lora-viewer-app
   ```

2. **Open the solution**
   - Launch Visual Studio 2022
   - Open `lora-viewer-app.sln`

3. **Restore NuGet packages**
   The project will automatically restore required packages:
   - `Newtonsoft.Json` (v13.0.4) - JSON serialization for config
   - `Ookii.Dialogs.Wpf` (v5.0.1) - Modern folder browser dialog

4. **Build and run**
   - Press `F5` or click "Start" in Visual Studio
   - Or build from command line:
     ```powershell
     dotnet build
     dotnet run --project LoraViewerApp
     ```

## 📖 Usage

### Basic Workflow

1. **Launch the application**
   - On first run, the folder path will be empty
   - Previously selected folder is restored on subsequent launches

2. **Select Lora folder**
   - Click "选择Lora文件夹" (Select Lora Folder) button
   - Browse to your Lora models directory
   - The application will scan for `.safetensors` files

3. **Browse models**
   - View the list of Lora models in the left panel
   - Each entry shows the name and thumbnail preview

4. **View details**
   - Click on any Lora model in the list
   - Right panel displays:
     - Full model name
     - Large preview image
     - All trigger words from the `.txt` file

5. **Copy information**
   - Click "复制名称" (Copy Name) to copy the model name
   - Click "复制" (Copy) next to any trigger word to copy it

### File Organization

Your Lora folder should follow this structure:

```
YourLoraFolder/
├── ModelName1.safetensors
├── ModelName1.txt              # Trigger words (one per line)
├── ModelName1.jpg              # Preview image
├── ModelName2.safetensors
├── ModelName2.txt
├── ModelName2.png
└── ...
```

**Supported image formats**: `.jpg`, `.jpeg`, `.png` (priority in that order)

### Trigger Words Format

The `.txt` file should contain trigger words, one per line:

```
character_name
specific_outfit
pose_description
style_tag
```

Empty lines are automatically filtered out.

## 🔧 Configuration

### config.json

The application automatically creates a `config.json` file to store settings:

```json
{
  "LastPath": "C:\\Path\\To\\Your\\Lora\\Folder"
}
```

This file is saved on application exit and loaded on startup.

**Note**: `config.json` is excluded from version control via `.gitignore`

## 🛠️ Technical Details

### Technology Stack

- **Framework**: .NET 9.0 (Windows)
- **UI**: WPF (Windows Presentation Foundation)
- **Language**: C# with nullable reference types enabled
- **Dependencies**:
  - Newtonsoft.Json - Configuration persistence
  - Ookii.Dialogs.Wpf - Enhanced folder selection dialog

### Key Features Implementation

#### Data Binding
Uses `ObservableCollection<T>` for automatic UI updates when the collection changes.

#### Image Loading
Supports both absolute and relative paths with automatic URI resolution:
```csharp
Uri imageUri = Path.IsPathRooted(imagePath) 
    ? new Uri(imagePath, UriKind.Absolute)
    : new Uri(imagePath, UriKind.Relative);
```

#### Configuration Persistence
Uses JSON serialization with exception handling for robust config management:
```csharp
var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFilePath));
```

## 🔮 Future Enhancements

Potential features for future development:

1. **Subfolder Support with Tabs**
   - Organize models by categories (styles, clothes, characters)
   - Tab-based navigation for subfolder browsing

2. **Search and Filter**
   - Search models by name
   - Filter by trigger words
   - Tag-based organization

3. **Model Information**
   - Display model metadata from `.safetensors` files
   - Show file size and creation date
   - Support for model descriptions

4. **Batch Operations**
   - Copy multiple trigger words at once
   - Export model lists to CSV/JSON
   - Batch rename functionality

5. **Image Gallery View**
   - Grid view option for browsing
   - Zoom and pan for large images
   - Slideshow mode

6. **Favorites and Collections**
   - Mark favorite models
   - Create custom collections
   - Quick access bookmarks

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is open source. Please check the repository for license details.

## 🙏 Acknowledgments

- Built for the AI art generation community
- Uses Lora models compatible with Stable Diffusion and similar frameworks
- Inspired by the need for better Lora model management tools

## 📞 Support

For issues, questions, or suggestions:
- Open an issue on GitHub
- Check existing issues for solutions

## 🔖 Version History

### Current Version
- Initial release with core functionality
- Lora browsing and trigger word management
- Path persistence
- Image preview support

---

**Note**: This is a desktop application for Windows. Cross-platform support may be considered in future versions.
