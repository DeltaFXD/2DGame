using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace GameEngine
{
    public static class Sprite
    {
        public static Dictionary<int, CanvasBitmap> Sprites = new Dictionary<int, CanvasBitmap>();

        public static CanvasAnimatedControl canvas = null;

        /// <summary>
        /// Sprite betoltes
        /// </summary>
        /// <param name="id">Sprite neve</param>
        /// <param name="path">Sprite helye</param>
        /// <returns></returns>
        public static async Task<bool> addSprite(int id, string path)
        {
            if (canvas == null) return false;

            CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(canvas, path);

            if (bitmap == null) return false;

            Sprites.Add(id, bitmap);

            if (Sprites.ContainsKey(id))
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
    }
}
