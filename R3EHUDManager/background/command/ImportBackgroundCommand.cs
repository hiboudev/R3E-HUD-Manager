using da2mvc.core.command;
using da2mvc.framework.collection.model;
using R3EHUDManager.background.events;
using R3EHUDManager.background.model;
using R3EHUDManager.database;
using R3EHUDManager.location.model;
using R3EHUDManager.screen.model;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace R3EHUDManager.background.command
{
    class ImportBackgroundCommand : ICommand
    {
        private readonly ImportBackgroundEventArgs args;
        private readonly ScreenModel screenModel;
        private readonly LocationModel locationModel;
        private readonly Database database;
        private readonly CollectionModel<BackgroundModel> collection;

        public ImportBackgroundCommand(ImportBackgroundEventArgs args, ScreenModel screenModel, LocationModel locationModel, Database database, CollectionModel<BackgroundModel> collection)
        {
            this.args = args;
            this.screenModel = screenModel;
            this.locationModel = locationModel;
            this.database = database;
            this.collection = collection;
        }

        public void Execute()
        {
            string fileName = Path.GetFileName(args.FilePath);

            if (File.Exists(Path.Combine(locationModel.LocalDirectoryBackgrounds, fileName)))
            {
                fileName = GetUnusedName(locationModel.LocalDirectoryBackgrounds, fileName);
            }

            string destinationFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.jpg";
            string destinationPath = Path.Combine(locationModel.LocalDirectoryBackgrounds, destinationFileName);

            BitmapImage originalBitmap = new BitmapImage(new Uri(args.FilePath));

            if (!args.CropArea.IsEmpty)
            {
                SaveJpeg(CropBitmap(originalBitmap, args.CropArea), destinationPath);
            }
            else
                SaveJpeg(originalBitmap, destinationPath);

            BackgroundModel background = BackgroundFactory.NewBackgroundModel(args.Name, destinationFileName, BaseDirectoryType.BACKGROUNDS_DIRECTORY, false, args.Layout);
            database.AddBackground(background);
            collection.Add(background);
            screenModel.SetBackground(background);
        }

        private string GetUnusedName(string path, string fileName)
        {
            int counter = 0;
            string nameOnly = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            do { ++counter; }
            while (File.Exists(Path.Combine(path, $"{nameOnly}({counter}){extension}")));

            return $"{nameOnly}({counter}){extension}";
        }

        private void SaveJpeg(BitmapSource bitmap, string path)
        {
            Debug.WriteLine(path);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.QualityLevel = 70;
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
            }
        }

        private BitmapSource CropBitmap(BitmapImage bitmap, Rect rect)
        {
            return new CroppedBitmap(bitmap, new Int32Rect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height));
        }
    }
}
