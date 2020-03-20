using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.UI;
using System.Numerics;
using Windows.UI.ViewManagement;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.System;
using System;

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
        Player player;
        Vector2 screenOffset;
        KeyBoard key;

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
            screen = new Screen(1366, 768);
            //Offset to center screen
            screenOffset = Coordinate.isoToNormal(new Vector2((screen.getWidth() / 2), (screen.getHeight() / 2)));

            //Create level
            level = new PlannedLevel(@"\resources\planned_levels\test_map.png");

            //Create KeyBoard instance
            key = new KeyBoard();

            //Create Player
            player = new Player(500, 320, key);

            //Add Player
            level.addEntity(player);

            //Ido meres
            watch.Start();
            lastTime = watch.ElapsedMilliseconds;

            //Init level
            level.init();
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Register for keyboard events
            Window.Current.CoreWindow.KeyDown += KeyDown_UIThread;
            Window.Current.CoreWindow.KeyUp += KeyUp_UIThread;
        }

        // The KeyDown handler runs on the UI thread...
        private void KeyDown_UIThread(CoreWindow sender, KeyEventArgs args)
        {
            args.Handled = true;

            // extract the data from the args before marshaling it to the
            // game loop thread
            var virtualKey = args.VirtualKey;

            var action = canvas.RunOnGameLoopThreadAsync(() => KeyDown_GameLoopThread(virtualKey));
        }

        // The KeyUp handler runs on the UI thread...
        private void KeyUp_UIThread(CoreWindow sender, KeyEventArgs args)
        {
            args.Handled = true;

            // extract the data from the args before marshaling it to the
            // game loop thread
            var virtualKey = args.VirtualKey;

            var action = canvas.RunOnGameLoopThreadAsync(() => KeyUp_GameLoopThread(virtualKey));
        }

        void KeyDown_GameLoopThread(VirtualKey vk)
        {
            key.update(vk, true);
        }

        void KeyUp_GameLoopThread(VirtualKey vk)
        {
            key.update(vk, false);
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
            int xMap = (int) Math.Round(player.getX() - screenOffset.X);
            int yMap = (int) Math.Round(player.getY() - screenOffset.Y);

            level.Render(xMap,yMap, screen);
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
            Window.Current.CoreWindow.KeyDown -= KeyDown_UIThread;
            Window.Current.CoreWindow.KeyUp -= KeyUp_UIThread;

            canvas.RemoveFromVisualTree();
            canvas = null;
        }
    }
}
