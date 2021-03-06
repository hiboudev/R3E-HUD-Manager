﻿using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using da2mvc.core.view;
using R3EHUDManager.huddata.model;

namespace R3EHUDManager.huddata.view
{
    class LayoutSourceView : TextBlock, IView
    {
        private const byte MAX_LENGTH = 60;

        public event EventHandler Disposed;

        public LayoutSourceView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Force autosize now.
            Text = "";
            VerticalAlignment = System.Windows.VerticalAlignment.Center;
            FontSize = 10;
            Foreground = new SolidColorBrush(Color.FromArgb(255,150,162,168));
        }

        internal void SetSource(LayoutSourceType sourceType, string name)
        {
            string headerText = "Layout source";
            string sourceText = "";

            switch (sourceType)
            {
                case LayoutSourceType.R3E:
                    sourceText = "R3E";
                    break;

                case LayoutSourceType.BACKUP:
                    sourceText = "Backup";
                    break;

                case LayoutSourceType.PROFILE:
                    sourceText = "Profile";
                    break;

                case LayoutSourceType.DELETED_PROFILE:
                    sourceText = "Deleted profile";
                    break;
            }

            Text = $"{headerText}: {sourceText} \"{TroncateText(name)}\"";
        }

        private string TroncateText(string text)
        {
            if (text.Length <= MAX_LENGTH)
                return text;

            return $"...{text.Substring(text.Length - MAX_LENGTH, MAX_LENGTH)}";
        }

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
