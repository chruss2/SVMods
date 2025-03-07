﻿using System.Collections.Generic;
using StardewModdingAPI;
using DailyTasksReport.TaskEngines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace DailyTasksReport.Tasks
{
    public abstract class Task
    {

        internal TaskEngine _Engine;
        internal IModHelper _helper;
        internal static TaskReportConfig _config;

        internal static readonly Dictionary<int, string> ObjectsNames = new Dictionary<int, string>();
        public bool Enabled { get { return _Engine.Enabled; } }
        public string TaskClass { get { return _Engine.TaskClass; } }
        public string TaskSubClass { get { return _Engine.TaskSubClass; } }
        protected void FirstScan() { _Engine.FirstScan(); }

        public virtual void Clear() { _Engine.Clear(); }

        public abstract string GeneralInfo(out int usedLines);
        public abstract string DetailedInfo(out int usedLines, out bool skipNextPage);

        public void FinishedReport() { _Engine.FinishedReport(); }
        public void OnDayStarted()
        {
            Clear();
            FirstScan();
        }
        
        public virtual void Draw(SpriteBatch b)
        {
        }
        protected static void DrawBubble(SpriteBatch b, Texture2D texture, Rectangle sourceRectangle,
        Vector2 destinationPosition)
        {
            var r = new Rectangle((int)destinationPosition.X, (int)destinationPosition.Y, Game1.tileSize * 3 / 4,
                Game1.tileSize * 3 / 4);
            b.Draw(Game1.mouseCursors, r, new Rectangle(141, 465, 20, 24), Color.White * 0.75f);
            r.Offset(r.Width / 4, r.Height / 6);
            r.Height /= 2;
            r.Width /= 2;
            b.Draw(texture, r, sourceRectangle, Color.White);
        }
        protected static void DrawBubble2Icons(SpriteBatch b, Texture2D texture1, Rectangle sourceRectangle1,
        Texture2D texture2, Rectangle sourceRectangle2, Vector2 destinationPosition)
        {
            var r = new Rectangle((int)destinationPosition.X - Game1.tileSize / 4,
                (int)destinationPosition.Y,
                Game1.tileSize,
                Game1.tileSize * 3 / 4);
            b.Draw(Game1.mouseCursors, r, new Rectangle(141, 465, 20, 24), Color.White * 0.75f);
            r.Offset(r.Width / 6, r.Height / 6);
            r.Width = (r.Width - Game1.tileSize / 4) / 2;
            r.Height /= 2;
            b.Draw(texture1, r, sourceRectangle1, Color.White);
            r.Offset(r.Width, 0);
            b.Draw(texture2, r, sourceRectangle2, Color.White);
        }
    }
}
