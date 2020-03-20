﻿using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.UI;
using System.Numerics;
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
        Screen screen;
        long lastTime;
        int upCount = 0;
        int frameCount = 0;
        Matrix3x2 iso;
        bool assests_ready = false;

        public MainPage()
        {
            InitializeComponent();

            init();
        }

        void init()
        {
            //TODO: change it to a loadable property
            ApplicationView.PreferredLaunchViewSize = new Size(1366, 768);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            iso = new Matrix3x2(2.828f , 1.414f, -2.828f, 1.414f , 0.0f, 0.0f);

            //Create screen
            screen = new Screen(1000, 500);

            //Create level
            level = new PlannedLevel(@"\resources\planned_levels\test_map.png");

            //Add Player
            level.addEntity(new Player(144, 144));

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

            //Load spritesheets
            assests_ready = await Sprite.loadSheet(@"\resources\spritesheets\tiles_sheet_data.txt", @"\resources\spritesheets\tiles.png");
        }

        /// <summary>
        /// Canvas render
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            frameCount++;
            if (!assests_ready) return;
            args.DrawingSession.Transform = iso;
            screen.setCDS(args.DrawingSession);
            level.Render(-120,120, screen);
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
            canvas.RemoveFromVisualTree();
            canvas = null;
        }
    }
}
