using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.Storage;

namespace GameEngine
{
    class PlannedLevel : Level
    {

        public PlannedLevel(string path)
        {
            load_level(path);
        }

        async void load_level(string path)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(Environment.CurrentDirectory + path);
            IRandomAccessStream stream = await RandomAccessStreamReference.CreateFromFile(file).OpenReadAsync();
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
            byte[] bytes = pixelData.DetachPixelData();

            mapWidth = Convert.ToInt32(decoder.PixelWidth);
            mapHeight = Convert.ToInt32(decoder.PixelHeight);
            
            map = new int[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                { 
                    map.SetValue(-BitConverter.ToInt32(bytes, (x + y * mapWidth) * sizeof (int)), x, y);
                    //Debug.WriteLine("values: " + map[x, y]);
                }
            }
        }
    }
}
