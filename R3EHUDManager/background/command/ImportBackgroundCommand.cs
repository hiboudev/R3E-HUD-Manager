using da2mvc.command;
using R3EHUDManager.application.events;
using R3EHUDManager.background.events;
using R3EHUDManager.background.model;
using R3EHUDManager.database;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace R3EHUDManager.background.command
{
    class ImportBackgroundCommand : ICommand
    {
        private readonly ImportBackgroundEventArgs args;
        private readonly SelectedBackgroundModel selection;
        private readonly LocationModel locationModel;
        private readonly Database database;
        private readonly BackgroundCollectionModel collection;

        public ImportBackgroundCommand(ImportBackgroundEventArgs args, SelectedBackgroundModel selection, LocationModel locationModel, Database database, BackgroundCollectionModel collection)
        {
            this.args = args;
            this.selection = selection;
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

            string destinationPath = Path.Combine(locationModel.LocalDirectoryBackgrounds, fileName);

            Bitmap originalBitmap = new Bitmap(args.FilePath);
            Bitmap destinationBitmap;

            if (!args.CropArea.IsEmpty)
            {
                destinationBitmap = new Bitmap(args.CropArea.Width, args.CropArea.Height);
                using (Graphics g = Graphics.FromImage(destinationBitmap))
                {
                    g.DrawImage(originalBitmap, new Rectangle(0, 0, destinationBitmap.Width, destinationBitmap.Height),
                                     args.CropArea, GraphicsUnit.Pixel);
                }
                SaveJpeg(destinationBitmap, destinationPath);
            }
            else
                SaveJpeg(originalBitmap, destinationPath);

            BackgroundModel background = BackgroundFactory.NewBackgroundModel(args.Name, fileName, BaseDirectoryType.BACKGROUNDS_DIRECTORY, false);
            database.AddBackground(background);
            collection.AddBackground(background);
            selection.SelectBackground(background);
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

        private void SaveJpeg(Bitmap bitmap, string path)
        {
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, 65L);

            bitmap.Save(path, GetEncoderInfo("image/jpeg"), parameters);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
