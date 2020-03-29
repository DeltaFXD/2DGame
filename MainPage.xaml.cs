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

using GameEngine.Levels;
using GameEngine.Graphics;
using GameEngine.Inputs;
using GameEngine.Entities.Mobs;

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
        bool assests_ready = false;
        bool animated_assests_ready = false;
        Player player;
        KeyBoard key;

        public MainPage()
        {
            InitializeComponent();

            Init();
        }

        void Init()
        {
            //TODO: change it to a loadable property
            ApplicationView.PreferredLaunchViewSize = new Size(1366, 768);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            //Create screen
            screen = new Screen(1366, 768);

            //Create level
            level = new PlannedLevel(@"\resources\planned_levels\test_map.png");

            //Create KeyBoard instance
            key = new KeyBoard();

            //Create Player
            player = new Player(500, 320, key);

            //Add Player
            level.AddEntity(player);

            //Ido meres
            watch.Start();
            lastTime = watch.ElapsedMilliseconds;

            //Init level
            level.Init();
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
            key.Update(vk, true);
        }

        void KeyUp_GameLoopThread(VirtualKey vk)
        {
            key.Update(vk, false);
        }

        /// <summary>
        /// Eroforrasok betoltese
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void Canvas_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            //Setup
            Sprite.Init(sender);

            //Load spritesheets
            assests_ready = await Sprite.LoadSheet(@"\resources\spritesheets\tiles_sheet_data.txt", @"\resources\spritesheets\tiles.png");

            //AnimatedSprite Setup
            AnimatedSprite.Init(sender);

            //Load AnimatedSpriteSheets
            animated_assests_ready = await AnimatedSprite.LoadSheet(@"\resources\spritesheets\player_sheet_data.txt", @"\resources\spritesheets\player_sprites.png");
        }

        /// <summary>
        /// Canvas render
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            frameCount++;
            if (!assests_ready) return;
            if (!animated_assests_ready) return;
            screen.SetCDS(args.DrawingSession);
            level.Render(player.GetXY(), screen);
        }

        private void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            upCount++;
            if ((watch.ElapsedMilliseconds - lastTime) > 1000)
            {
                lastTime = watch.ElapsedMilliseconds;
                Debug.WriteLine("UPS: " + upCount + " , FPS: " + frameCount);
                upCount = 0;
                frameCount = 0;
            }
            level.Update();
            AnimatedSprite.GetUpdateables().ForEach(e => e.Update());
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
