using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.Storage;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Windows.Graphics.DirectX;

namespace GameEngine
{
    public static class Sprite
    {
        private static Dictionary<int, CanvasBitmap> Sprites = new Dictionary<int, CanvasBitmap>();

        private static CanvasAnimatedControl canvas = null;

        private static CanvasBitmap defaultSprite = null;

        private static int DSS = 32;

        public static void init(CanvasAnimatedControl canvas)
        {
            Sprite.canvas = canvas;
        }

        public static CanvasBitmap getSprite(int id)
        {
            if (Sprites.ContainsKey(id))
            {
                return Sprites[id];
            } 
            else
            {
                return defaultSprite;
            }
        }

        /// <summary>
        /// Sprite betoltes
        /// </summary>
        /// <param name="id_path">sheet_data helye</param>
        /// <param name="path">Spritesheet helye</param>
        /// <returns></returns>
        public static async Task<bool> loadSheet(string id_path, string path)
        {
            if (canvas == null) return false;
            //Load whole sheet
            byte[] bytes = null;
            int sheetWidth = 0;
            int sheetHeight = 0;
            try {
                StorageFile sheet_file = await StorageFile.GetFileFromPathAsync(Environment.CurrentDirectory + path);
                IRandomAccessStream stream = await RandomAccessStreamReference.CreateFromFile(sheet_file).OpenReadAsync();
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
                bytes = pixelData.DetachPixelData();
                sheetWidth = Convert.ToInt32(decoder.PixelWidth);
                sheetHeight = Convert.ToInt32(decoder.PixelHeight);
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            //Load sheet data
            string data = "";
            try
            {
                StorageFile data_file = await StorageFile.GetFileFromPathAsync(Environment.CurrentDirectory + id_path);
                data = await FileIO.ReadTextAsync(data_file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            int x, y, id, xAbs, yAbs;
            byte[] bitmap_bytes = new byte[DSS * DSS * 4];
            //Sheet data proccesing
            MatchCollection matches = Regex.Matches(data, @"(\d) (\d) (\d+)");
            foreach (Match match in matches)
            {
                x = int.Parse(match.Groups[1].Value);
                y = int.Parse(match.Groups[2].Value);
                id = int.Parse(match.Groups[3].Value);
                //Out of bound check
                if (x * DSS > sheetWidth || y * DSS > sheetHeight || x < 0 || y < 0) continue;

                for (int by = 0; by < DSS; by++)
                {
                    yAbs = by + y * DSS;
                    for (int bx = 0; bx < DSS * 4; bx++)
                    {
                        xAbs = bx + x * DSS * 4;
                        bitmap_bytes[bx + by * DSS * 4] = bytes[xAbs + yAbs * sheetWidth * 4];
                    }
                }

                CanvasBitmap bitmap = CanvasBitmap.CreateFromBytes(canvas, bitmap_bytes, DSS, DSS, DirectXPixelFormat.R8G8B8A8UIntNormalized);

                if (bitmap == null) return false;

                Sprites.Add(id, bitmap);
                
                if (Sprites.ContainsKey(id))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}
