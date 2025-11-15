using Game_Library.Audio;
using Game_Library.Input;
using Game_Library.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game_Library;

public class Core : Game
{
    internal static Core s_instance;

    /// <summary>
    /// Gets a reference to the Core instance.
    /// </summary>
    public static Core Instance => s_instance;
    // The scene that is currently active.
    private static Scene s_activeScene;
    // The next scene to switch to, if there is one.
    private static Scene s_nextScene;
    /// <summary>
    /// Gets the graphics device manager to control the presentation of graphics.
    /// </summary>
    public static GraphicsDeviceManager Graphics { get; private set; }

    /// <summary>
    /// Gets the graphics device used to create graphical resources and perform primitive rendering.
    /// </summary>
    public static new GraphicsDevice GraphicsDevice { get; private set; }

    /// <summary>
    /// The Resolution Width of the players monitor, defaults to 1920
    /// </summary>
    public static int ViewportResoutionWidth { get; private set; } = 1920;
    /// <summary>
    /// The Resolution Height of the players monitor, defaults to 1080
    /// </summary>
    public static int ViewportResoutionHeight { get; private set; } = 1080;

    /// <summary>
    /// Gets the sprite batch used for all 2D rendering.
    /// </summary>
    public static SpriteBatch SpriteBatch { get; private set; }

    /// <summary>
    /// Gets the content manager used to load global assets.
    /// </summary>
    public static new ContentManager Content { get; private set; }

    /// <summary>
    /// Gets a reference to to the input management system.
    /// </summary>
    public static InputManager Input { get; private set; }

    /// <summary>
    /// Set True to close the game
    /// </summary>
    public static bool ExitGame { get; set; }
    /// <summary>
    /// Set True to close the game
    /// </summary>
    public static bool PauseOnUnfocus { get; set; }
    /// <summary>
    /// RNG, use Next(float min, float max) to get a random number
    /// </summary>
    public static Random Randomizer { get; set; }
    /// <summary>
    /// Gets a reference to the audio control system.
    /// </summary>
    public static AudioController Audio { get; private set; }
    /// <summary>
    /// Gets a reference to the Save Manager System
    /// </summary>
    public static SaveManager SaveManager { get; private set; }
    /// <summary>
    /// Deltatime which is used for physics and timer calculations
    /// </summary>
    public static float Deltatime { get; set; }
    TimeSpan elapsedTime;
    float previousCount;
    RenderTarget2D _renderTarget;
    /// <summary>
    /// Creates a new Core instance.
    /// </summary>
    /// <param name="title">The title to display in the title bar of the game window.</param>
    /// <param name="viewportWidth">The initial width, in pixels, of the game window.</param>
    /// <param name="viewportHeight">The initial height, in pixels, of the game window.</param>
    /// <param name="fullScreen">Indicates if the game should start in fullscreen mode.</param>
    public Core(string title, int viewportWidth, int viewportHeight,bool fullScreen)
    {
        // Ensure that multiple cores are not created.
        if (s_instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }

        // Store reference to engine for global member access.
        s_instance = this;

        // Create a new graphics device manager.
        Graphics = new GraphicsDeviceManager(this);

        // Set the graphics defaults.
        Graphics.HardwareModeSwitch = false;
        Graphics.IsFullScreen = fullScreen;
        // Apply the graphic presentation changes.
        Graphics.ApplyChanges();
        ViewportResoutionWidth = viewportWidth;
        ViewportResoutionHeight = viewportHeight;
        // Set the window title.
        Window.Title = title;
        // Set the core's content manager to a reference of the base Game's
        // content manager.
        Content = base.Content;

        // Set the root directory for content.
        Content.RootDirectory = "Content";

        // Mouse is visible by default.
        IsMouseVisible = true;
        //Intialize the randomizer, use Next(min,max) when using the randomizer
        Randomizer = new Random();
    }

    protected override void Initialize()
    {
        // Set the core's graphics device to a reference of the base Game's
        // graphics device.
        GraphicsDevice = base.GraphicsDevice;

        // Create the sprite batch instance.
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        // Create a new input manager.
        Input = new InputManager();

        // Create a new audio controller.
        Audio = new AudioController();
        //Create a new save manager
        SaveManager = new SaveManager();
        //Load the Save Data
        SaveManager.LoadData();
        //Check if the resolution file was just created
        if (SaveManager.instance.filesFreshlyCreated)
        {
            //Assign monitor width to width of the Window
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //Assign monitor height to height of the Window
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //Assign Width of the Window to Game Settings
            SaveManager.instance.fs.windowResolutionWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //Assign Height of the Window to Game Settings
            SaveManager.instance.fs.windowResolutionHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //Save Changes
            SaveManager.Save();
        }
        else
        {
            //Assign width in File Settings to width of the Window
            Graphics.PreferredBackBufferWidth = SaveManager.instance.fs.windowResolutionWidth;
            //Assign height in File Settings to height of the Window
            Graphics.PreferredBackBufferHeight = SaveManager.instance.fs.windowResolutionHeight;
            //Assign pauseOnUF in File Settings to PauseOnUnfocus
            PauseOnUnfocus = SaveManager.instance.fs.pauseOnUF;
        }
        //Assign PauseOnUnfocus Setting value to PauseOnUnfocus
        PauseOnUnfocus = SaveManager.instance.fs.pauseOnUF;
        //Assign isFullscreen Setting value to Graphics.isFullscreen
        Graphics.IsFullScreen = SaveManager.instance.fs.isFullscreen;
        //Assign the graphics settings
        Graphics.ApplyChanges();
        //Create the RenderTarget
        _renderTarget = new RenderTarget2D(GraphicsDevice, ViewportResoutionWidth, ViewportResoutionHeight);
        base.Initialize();
    }

    protected override void UnloadContent()
    {
        // Dispose of the audio controller.
        Audio.Dispose();

        base.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        elapsedTime += gameTime.ElapsedGameTime;
        // Update the input manager.
        Input.Update(gameTime);

        // Update the audio controller.
        Audio.Update();
        //Check if the game has been set to closed
        if (ExitGame)
        {
            Exit();
        }
        // if there is a next scene waiting to be switch to, then transition
        // to that scene.
        if (s_nextScene != null)
        {
            TransitionScene();
        }
        //Check if the game was intilized w
        if (PauseOnUnfocus)
        {
            //Set the IsUnfocused variable of the 
            s_activeScene.unFocused(this.IsActive);
        }
        else
        {
            
        }
        // If there is an active scene and the game is focused on, update it.
        if (s_activeScene != null && s_activeScene.IsFocused)
        {
            //The active scene's update loop
            s_activeScene.Update(gameTime);
        }
        base.Update(gameTime);
        //Deltatime Formula calculation = Current Time(Delta 2)-Time at the start of the update loop(Delta 1)
        Deltatime = (float)elapsedTime.TotalSeconds - previousCount;
        //Assign Delta 1 the value of delta 2
        previousCount = (float)elapsedTime.TotalSeconds;
        //Set Delta 2 to Zero to restart the calculation
        elapsedTime.Equals(0);
    }

    protected override void Draw(GameTime gameTime)
    {
        //Assign the rendering of sprites and calculations to the Viewport
        GraphicsDevice.SetRenderTarget(_renderTarget);
        //Clear all sprites out the Viewport's spritebatch and set it to Cornflower Blue
        GraphicsDevice.Clear(Color.CornflowerBlue);
        //Begin rendering to spritebatch
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        // If there is an active scene, draw it.
        if (s_activeScene != null)
        {
            s_activeScene.Draw(gameTime);
        }
        //End rendering to spritebatch
        SpriteBatch.End();
        //Assign the rendering of sprites and calculations to the graphic device itself
        GraphicsDevice.SetRenderTarget(null);
        //Clear all sprites out the graphic device's spritebatch and set it to Cornflower Blue
        GraphicsDevice.Clear(Color.CornflowerBlue);
        //Begin rendering to spritebatch
        SpriteBatch.Begin();
        //Draw the graphical data of the render target to the graphic device
        SpriteBatch.Draw(_renderTarget,
            new Rectangle(0, 0, Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height),
            Color.White);
        //End rendering to spritebatch
        SpriteBatch.End();
        base.Draw(gameTime);
    }
    public static void ChangeScene(Scene next)
    {
        // Only set the next scene value if it is not the same
        // instance as the currently active scene.
        if (s_activeScene != next)
        {
            s_nextScene = next;
        }
    }

    private static void TransitionScene()
    {
        // If there is an active scene, dispose of it.
        if (s_activeScene != null)
        {
            s_activeScene.Dispose();
        }

        // Force the garbage collector to collect to ensure memory is cleared.
        GC.Collect();

        // Change the currently active scene to the new scene.
        s_activeScene = s_nextScene;

        // Null out the next scene value so it does not trigger a change over and over.
        s_nextScene = null;

        // If the active scene now is not null, initialize it.
        // Remember, just like with Game, the Initialize call also calls the
        // Scene.LoadContent
        if (s_activeScene != null)
        {
            s_activeScene.Initialize();
        }
    }
}

