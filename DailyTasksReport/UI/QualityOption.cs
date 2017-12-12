﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;

namespace DailyTasksReport.UI
{
    internal class QualityOption : OptionsElement
    {
        private readonly ModConfig _config;
        private readonly OptionsEnum _option;
        
        private int _selectedQuality;
        private bool _isMouseOnMinusButton;
        private bool _isMouseOnPlusButton;

        private static readonly List<string> DisplayOptions = new List<string> {"None", "Silver", "Gold", "Iridium"};
        private static readonly Rectangle MinusButtonSource = new Rectangle(177, 345, 7, 8);
        private static readonly Rectangle PlusButtonSource = new Rectangle(184, 345, 7, 8);
        private static Rectangle _minusButton;
        private static Rectangle _plusButton;

        public QualityOption(string label, OptionsEnum whichOption, ModConfig config, int itemLevel = 0) :
            base(label, -1, -1, 7 * Game1.pixelZoom, 7 * Game1.pixelZoom, (int) whichOption)
        {
            _config = config;
            _option = whichOption;
            bounds.X += itemLevel * Game1.pixelZoom * 7;
            this.whichOption = (int) whichOption;

            var biggestOption = (int) Game1.dialogueFont.MeasureString(DisplayOptions[0]).X + 4 * Game1.pixelZoom;
            foreach (var displayOption in DisplayOptions)
                biggestOption = Math.Max((int) Game1.dialogueFont.MeasureString(displayOption).X + 4 * Game1.pixelZoom,
                    biggestOption);

            bounds = new Rectangle(bounds.X, bounds.Y, 7 * Game1.pixelZoom * 2 + biggestOption, 8 * Game1.pixelZoom);
            _minusButton = new Rectangle(bounds.X, 4 + Game1.pixelZoom * 4, 7 * Game1.pixelZoom, 8 * Game1.pixelZoom);
            _plusButton = new Rectangle(bounds.Right - 8 * Game1.pixelZoom, 4 + Game1.pixelZoom * 4,
                7 * Game1.pixelZoom, 8 * Game1.pixelZoom);

            RefreshStatus();
        }

        internal void RefreshStatus()
        {
            switch (_option)
            {
                case OptionsEnum.Cask:
                    _selectedQuality = _config.Cask == 4 ? 3 : _config.Cask;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Option {_option} is not possible on a QualityOption.");
            }
        }

        public override void receiveLeftClick(int x, int y)
        {
            if (greyedOut)
                return;
            var previousSelected = _selectedQuality;
            if (_minusButton.Contains(x, y) && _selectedQuality > 0)
            {
                _selectedQuality--;
                Game1.playSound("drumkit6");
            }
            else if (_plusButton.Contains(x, y) && _selectedQuality != DisplayOptions.Count - 1)
            {
                _selectedQuality++;
                Game1.playSound("drumkit6");
            }

            if (_selectedQuality < 0)
                _selectedQuality = 0;
            else if (_selectedQuality >= DisplayOptions.Count)
                _selectedQuality = DisplayOptions.Count - 1;
            if (previousSelected == _selectedQuality)
                return;

            switch (_option)
            {
                case OptionsEnum.Cask:
                    _config.Cask = _selectedQuality == 3 ? 4 : _selectedQuality;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Option {_option} is not possible on a QualityOption.");
            }
            SettingsMenu.RaiseReportConfigChanged(new SettingsChangedEventArgs(_option));
        }

        public override void receiveKeyPress(Keys key)
        {
            base.receiveKeyPress(key);
            if (!Game1.options.snappyMenus || !Game1.options.gamepadControls)
                return;
            if (Game1.options.doesInputListContain(Game1.options.moveRightButton, key))
            {
                receiveLeftClick(_plusButton.Center.X, _plusButton.Center.Y);
                _isMouseOnPlusButton = true;
                _isMouseOnMinusButton = false;
            }
            else if (Game1.options.doesInputListContain(Game1.options.moveLeftButton, key))
            {
                receiveLeftClick(_minusButton.Center.X, _minusButton.Center.Y);
                _isMouseOnPlusButton = false;
                _isMouseOnMinusButton = true;
            }
        }

        internal void CursorAboveOption()
        {
            if (!Game1.options.snappyMenus || !Game1.options.gamepadControls)
                return;
            if (_isMouseOnMinusButton || _isMouseOnPlusButton)
                return;
            _isMouseOnMinusButton = true;
            _isMouseOnPlusButton = false;
        }

        public override void draw(SpriteBatch b, int slotX, int slotY)
        {
            b.Draw(Game1.mouseCursors, new Vector2(slotX + _minusButton.X, slotY + _minusButton.Y), MinusButtonSource,
                Color.White * (greyedOut ? 0.33f : 1f) * (_selectedQuality == 0 ? 0.5f : 1f), 0.0f, Vector2.Zero,
                Game1.pixelZoom, SpriteEffects.None, 0.4f);
            b.Draw(Game1.mouseCursors, new Vector2(slotX + _plusButton.X, slotY + _plusButton.Y), PlusButtonSource,
                Color.White * (greyedOut ? 0.33f : 1f) * (_selectedQuality == DisplayOptions.Count - 1 ? 0.5f : 1f),
                0.0f, Vector2.Zero, Game1.pixelZoom, SpriteEffects.None, 0.4f);
            b.DrawString(Game1.dialogueFont,
                _selectedQuality >= DisplayOptions.Count || _selectedQuality == -1 ? "" : DisplayOptions[_selectedQuality],
                new Vector2(slotX + _minusButton.X + _minusButton.Width + Game1.pixelZoom,
                    slotY + _minusButton.Y - Game1.pixelZoom), Game1.textColor);

            if (Game1.options.snappyMenus && Game1.options.gamepadControls)
                if (_isMouseOnMinusButton)
                {
                    Game1.setMousePosition(slotX + _minusButton.Center.X, slotY + _minusButton.Center.Y);
                    _isMouseOnMinusButton = false;
                }
                else if (_isMouseOnPlusButton)
                {
                    Game1.setMousePosition(slotX + _plusButton.Center.X, slotY + _plusButton.Center.Y);
                    _isMouseOnPlusButton = false;
                }

            base.draw(b, slotX, slotY);
        }
    }
}