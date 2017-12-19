using da2mvc.core.injection;
using R3EHUDManager.application.view;
using R3EHUDManager.coordinates;
using R3EHUDManager.database;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.r3esupport.command;
using R3EHUDManager.screen.model;
using R3EHUDManager.screen.view;
using R3EHUDManager.userpreferences.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.screen.utils
{
    class ScreenUtils
    {
        public static ScreenPositionType GetScreen(PlaceholderModel placeholder)
        {
            double Px = placeholder.Position.X;
            double Ax = placeholder.Anchor.X;

            double Py = placeholder.Position.Y;

            if (Py < -1 || Py > 1) return ScreenPositionType.OUTSIDE;

            if (Px == -1)
                return Ax > 0 ? ScreenPositionType.LEFT : ScreenPositionType.CENTER;

            if (Px == 1)
                return Ax > 0 ? ScreenPositionType.CENTER : ScreenPositionType.RIGHT;

            if (Px >= -3 && Px < -1)
                return ScreenPositionType.LEFT;

            if (Px > -1 && Px < 1)
                return ScreenPositionType.CENTER;

            if (Px > 1 && Px <= 3)
                return ScreenPositionType.RIGHT;

            return ScreenPositionType.OUTSIDE;
        }

        public static R3ePoint ToScreenOffset(PlaceholderModel placeholder, ScreenPositionType targetScreen)
        {
            ScreenPositionType currentScreen = GetScreen(placeholder);

            // Replace placeholder Y in screen in case it's outside.
            double positionYInScreen = placeholder.Position.Y;
            if (placeholder.Position.Y < -1) positionYInScreen = -1;
            else if (placeholder.Position.Y > 1) positionYInScreen = 1;

            double offsetX = 0;
            double offsetY = positionYInScreen - placeholder.Position.Y;

            if (currentScreen == ScreenPositionType.OUTSIDE)
                offsetX = GetOffsetXFromOutside(placeholder, targetScreen);
            else
                switch (targetScreen)
                {
                    case ScreenPositionType.LEFT:
                        if (currentScreen == ScreenPositionType.CENTER) offsetX = -2;
                        else if (currentScreen == ScreenPositionType.RIGHT) offsetX = -4;
                        break;

                    case ScreenPositionType.RIGHT:
                        if (currentScreen == ScreenPositionType.LEFT) offsetX = 4;
                        else if (currentScreen == ScreenPositionType.CENTER) offsetX = 2;
                        break;

                    case ScreenPositionType.CENTER:
                        if (currentScreen == ScreenPositionType.LEFT) offsetX = 2;
                        else if (currentScreen == ScreenPositionType.RIGHT) offsetX = -2;
                        break;

                    case ScreenPositionType.OUTSIDE:
                        offsetX = 0;
                        break;

                    default:
                        throw new Exception("Not implemented type.");
                }

            return new R3ePoint(offsetX, offsetY);
        }

        private static double GetOffsetXFromOutside(PlaceholderModel placeholder, ScreenPositionType targetScreen)
        {
            double zeroX = placeholder.Position.X + 3;
            if (zeroX < 0) zeroX += 2;

            double moduloX = zeroX % 2;
            double positionXInScreen = Math.Abs(moduloX);

            switch (targetScreen)
            {
                case ScreenPositionType.LEFT:
                    return positionXInScreen - 3 - placeholder.Position.X;

                case ScreenPositionType.RIGHT:
                    return positionXInScreen + 1 - placeholder.Position.X;

                case ScreenPositionType.CENTER:
                    return positionXInScreen - 1 - placeholder.Position.X;

                case ScreenPositionType.OUTSIDE:
                    return 0;

                default:
                    throw new Exception("Not implemented type.");
            }
        }

        public static string GetFormattedAspectRatio(Size size)
        {
            int denominator = GetHighestDenominator(size.Width, size.Height);

            return $"{size.Width / denominator}/{size.Height / denominator}";
        }

        public static string GetFormattedAspectRatio(int width, int height)
        {
            int denominator = GetHighestDenominator(width, height);

            return $"{width / denominator}/{height / denominator}";
        }

        private static int GetHighestDenominator(int a, int b)
        {
            while (b != 0)
            {
                int c = a % b;
                a = b;
                b = c;
            }
            return a;
        }

        public static bool PromptUserIfOutsideOfCenterScreenPlaceholders(PlaceHolderCollectionModel collectionModel, UserPreferencesModel preferences, Database database)
        {
            if (preferences.PromptOutsidePlaceholders == OutsidePlaceholdersPrefType.DO_NOTHING)
                return false;

            bool outsidePlaceholder = false;

            foreach (PlaceholderModel placeholder in collectionModel.Items)
            {
                ScreenPositionType screen = ScreenUtils.GetScreen(placeholder);
                if (screen != ScreenPositionType.CENTER)
                {
                    outsidePlaceholder = true;
                    break;
                }
            }
            
            if (outsidePlaceholder)
            {
                if (preferences.PromptOutsidePlaceholders == OutsidePlaceholdersPrefType.MOVE)
                {
                    MovePlaceholders(collectionModel);
                }
                else if (preferences.PromptOutsidePlaceholders == OutsidePlaceholdersPrefType.PROMPT)
                {
                    PromptView prompt = Injector.GetInstance<PromptView>();
                    CheckBoxData checkData = new CheckBoxData(PreferenceType.PROMPT_OUTSIDE_PLACEHOLDER, "Remember my choice");
                    prompt.Initialize("Placeholder(s) outside of center screen", "Some placeholders are now outside of the screen, move them to center screen?", new CheckBoxData[] { checkData });

                    DialogResult result = prompt.ShowDialog();
                    if (prompt.GetChecked(PreferenceType.PROMPT_OUTSIDE_PLACEHOLDER))
                    {
                        if (result == DialogResult.Yes)
                        {
                            preferences.PromptOutsidePlaceholders = OutsidePlaceholdersPrefType.MOVE;
                            database.SaveOutsidePlaceholdersPref(OutsidePlaceholdersPrefType.MOVE);
                        }
                        else
                        {
                            preferences.PromptOutsidePlaceholders = OutsidePlaceholdersPrefType.DO_NOTHING;
                            database.SaveOutsidePlaceholdersPref(OutsidePlaceholdersPrefType.DO_NOTHING);
                        }
                    }

                    if (result == DialogResult.Yes)
                    {
                        MovePlaceholders(collectionModel);
                        return true;
                    }
                }
            }
            return false;
        }

        private static void MovePlaceholders(PlaceHolderCollectionModel collectionModel)
        {
            Injector.ExecuteCommand<FixPlaceholderCollectionCommand>();
            //foreach (PlaceholderModel placeholder in collectionModel.Items)
            //{
            //    ScreenPositionType screen = ScreenUtils.GetScreen(placeholder);
            //    if (screen != ScreenPositionType.CENTER)
            //    {
            //        R3ePoint offset = ScreenUtils.ToScreenOffset(placeholder, ScreenPositionType.CENTER);
            //        R3ePoint newPosition = new R3ePoint(placeholder.Position.X + offset.X, placeholder.Position.Y + offset.Y);
            //        placeholder.Move(newPosition);
            //    }
            //}
        }
    }
}
