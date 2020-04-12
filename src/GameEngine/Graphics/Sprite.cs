using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Graphics.DirectX;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;

using GameEngine.Levels;

namespace GameEngine.Graphics
{
    public class Sprite
    {
        private static Dictionary<int, CanvasBitmap> Sprites = new Dictionary<int, CanvasBitmap>();
        private static Dictionary<String, int> table = new Dictionary<string, int>();

        private static CanvasAnimatedControl canvas = null;

        private static int DSS = 32;

        int _size;

        public int GetSize()
        {
            return _size;
        }

        public static void Init(CanvasAnimatedControl canvas)
        {
            Sprite.canvas = canvas;
        }

        public static CanvasBitmap GetSprite(int id)
        {
            if (Sprites.ContainsKey(id))
            {
                return Sprites[id];
            } 
            else
            {
                return null;
            }
        }

        public static int GetSpriteID(String name)
        {
            if (table.ContainsKey(name)) return table[name];
            else throw new ArgumentException("Cannot find name: " + name);
        }

        /// <summary>
        /// Sprite betoltes
        /// </summary>
        /// <param name="spriteSheetDataPath">sheet_data helye</param>
        /// <param name="spriteSheetPath">Spritesheet helye</param>
        /// <returns></returns>
        public static async Task<bool> LoadSheet(string spriteSheetDataPath, string spriteSheetPath)
        {
            if (canvas == null) return false;
            //Load whole sheet
            byte[] bytes;
            int sheetWidth;
            int sheetHeight;
            try {
                StorageFile sheet_file = await StorageFile.GetFileFromPathAsync(Environment.CurrentDirectory + spriteSheetPath);
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
            string data;
            try
            {
                StorageFile data_file = await StorageFile.GetFileFromPathAsync(Environment.CurrentDirectory + spriteSheetDataPath);
                data = await FileIO.ReadTextAsync(data_file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            int x, y, id, xAbs, yAbs;
            bool solid, penetrateable;
            byte[] bitmap_bytes = new byte[DSS * DSS * 4];
            //Sheet data proccesing
            MatchCollection matches = Regex.Matches(data, @"(\d) (\d) (\d+) (\d) (\d)");
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
                
                //Data for tiles
                solid = 1 == int.Parse(match.Groups[4].Value);
                penetrateable = 1 == int.Parse(match.Groups[5].Value);
     
                //Create tiles based on sprites
                new Tile(solid, penetrateable, id);

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
