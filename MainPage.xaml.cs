using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.UI;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.ViewManagement;
using System.Diagnostics;

using GameEngine;

namespace Game2D
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Stopwatch watch = new Stopwatch();
        Level level;
        long lastTime;
        int upCount = 0;
        int frameCount = 0;
        Matrix3x2 iso;

        public MainPage()
        {
            this.InitializeComponent();

            this.init();
        }

        void init()
        {
            //TODO: change it to a loadable property
            ApplicationView.PreferredLaunchViewSize = new Size(1366, 768);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            iso = new Matrix3x2(2.828f , 1.414f, -2.828f, 1.414f , 0.0f, 0.0f);

            //Create level
            level = new PlannedLevel(@"\resources\planned_levels\test_map.png");
            
            //Ido meres
            watch.Start();
            lastTime = watch.ElapsedMilliseconds;

            //Init level
            level.init();
        }

        /// <summary>
        /// Eroforrasok betoltese
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void canvas_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            //Setup
            Sprite.init(sender);

            //Load sprites
            //0x00000001 - test tile
            //await Sprite.addSprite(0x00000001, @"resources/spritesheets/test_tile.png");
            //0x01000000 -  wall
            await Sprite.addSprite(0x01000000, @"resources/spritesheets/wall_tile.png");
            //0x00CCCCCD -  wall_north_west
            await Sprite.addSprite(0x00CCCCCD, @"resources/spritesheets/wall_nw_tile.png");
            //0x00B3B3B4 -  wall_west
            await Sprite.addSprite(0x00B3B3B4, @"resources/spritesheets/wall_w_tile.png");
            //0x009B9B9C - base floor
            await Sprite.addSprite(0x009B9B9C, @"resources/spritesheets/base_floor_tile.png");
            //0x00C8C8C8 - floor 2
            await Sprite.addSprite(0x00373738, @"resources/spritesheets/floor2_tile.png");
            //0x00FFFFFF - void
            await Sprite.addSprite(0x00000001, @"resources/spritesheets/void_tile.png");
        }

        /// <summary>
        /// Canvas render
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            frameCount++;
            //args.DrawingSession.Transform = scale;
            args.DrawingSession.Transform = iso;
            level.Render(args.DrawingSession);
        }

        private void canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            upCount++;
            if ((watch.ElapsedMilliseconds - lastTime) > 1000)
            {
                lastTime = watch.ElapsedMilliseconds;
                Debug.WriteLine("UPS: " + upCount + " , FPS: " + frameCount);
                upCount = 0;
                frameCount = 0;
            }
            level.update();
        }

        void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            this.canvas.RemoveFromVisualTree();
            this.canvas = null;
        }
    }
}
