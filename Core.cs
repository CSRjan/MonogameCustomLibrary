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
    /// <param name="outputWidth">The width, in pixels, the game window will output to.</param>
    /// <param name="outputHeight">The height, in pixels, the game window will output to.</param>
    /// <param name="fullScreen">Indicates if the game should start in fullscreen mode.</param>
    /// <param name="pauseOnUF">Indicates if the game should pause when not focused.</param>
    public Core(string title, int viewportWidth, int viewportHeight, int outputWidth, int outputHeight, bool fullScreen, bool pauseOnUF)
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
        Graphics.PreferredBackBufferWidth = outputWidth;
        Graphics.PreferredBackBufferHeight = outputHeight;
        Graphics.IsFullScreen = fullScreen;
        // Apply the graphic presentation changes.
        Graphics.ApplyChanges();
        ViewportResoutionWidth = viewportWidth;
        ViewportResoutionHeight = viewportHeight;
        // Set the window title.
        Window.Title = title;

        PauseOnUnfocus = pauseOnUF;
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

        //Create the RenderTarget
        _renderTarget = new RenderTarget2D(GraphicsDevice,
ViewportResoutionWidth, ViewportResoutionHeight);
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
        if (PauseOnUnfocus)
        {
            s_activeScene.unFocused(this.IsActive);
        }
        else
        {
            
        }
        // If there is an active scene, update it.
        if (s_activeScene != null && s_activeScene.IsUnfocused)
        {
            s_activeScene.Update(gameTime);
        }
        base.Update(gameTime);
        Deltatime = (float)elapsedTime.TotalSeconds - previousCount;
        previousCount = (float)elapsedTime.TotalSeconds;
        elapsedTime.Equals(0);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_renderTarget);
        GraphicsDevice.Clear(new Color(0, 0, 0, 255));
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        // If there is an active scene, draw it.
        if (s_activeScene != null)
        {
            s_activeScene.Draw(gameTime);
        }
        SpriteBatch.End();
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin();
        SpriteBatch.Draw(_renderTarget,
            new Rectangle(0, 0, Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height),
            Color.White);
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

