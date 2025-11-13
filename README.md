Implementing Core

In Monogame's Game1 Class, next to Game 1() add : base("Insert Name for the Game Window", 
Game Viewport Resolution Width, Game Viewport Resolution Height, Game Window Resolution Width,
Game Window Resolution Height).

In your Game1's Initialize() class, use a ChangeScene() function call with what is your first scene. After that, Game 1 can be ignored from here.


Viewport-
There is now the ability to support Upscaling, which can be done by assigning a viewport resolution when assigning the base of the Game1. This is done by assigning a RenderTarget2D with the game's planned resolution, done with assigning the Core's viewportWidth and viewportHeight when first declared. This can be done by setting the Game1 class such as class Game1():Core, and then when declaring the class do, public Game1() : base(nameForTheGameWindow, viewportResolutionWidth, viewportResolutionHeight, windowResolutionWidth, windowResolutionHeight, boolscreenIsTrue). Everything else handling the viewport rendering is done automatically via the core class.

Drawing-
As part of the restructuring to support the viewport automatically, I have set up the Core's Draw function to automatically clear the sprite batch, so now developers only need to focus on sending draw calls to the Core's Spritebatch done via Core.Spritebatch.Draw(texture, position, etc.)

Deltatime-
Deltatime has now been added to the core file, calculated automatically and referenceable to any scene. This is done by obtaining the Seconds passed since the start of the Update function using a Timespan variable, which is then subtracted by the previous amount of time the Update Loop took to get through all of it's functions, to which it assigns that timeSpan time to the lastTime variable and resets the Timespan variable to zero. And now it continues infinitely.

Animator-
(Animator still uses the XML format seen in Chapter 09, besides the changes)
In the Animation regions, there is now a looping variable in terms of looping = "bool param", which is pretty obvious how the function works. There was support for the only looping in the original animator, meaning one time animations were unsupported.

In the Frame Regions, you can put the amount of frames the frame will be held on. If you are familiar with animation terms, this is a way to implement frame timings and drawing on one, two, threes.

Using the animation XML seen in Monogame 2D game Tutorial, the XML will go from this

<Animation name="bat-animation" delay="200">
            <Frame region="bat-1" />
            <Frame region="bat-2" />
            <Frame region="bat-1" />
            <Frame region="bat-3" />

<Animation name="bat-animation" delay="200" looping="true">
            <Frame region="bat-1" frameCount=1 />
            <Frame region="bat-2" frameCount=1 />
            <Frame region="bat-1" frameCount=1 />
            <Frame region="bat-3" frameCount=1 />

In the future I plan to create an XML document creator to create the files for the Monogame Animator at least somewhat faster

*https://docs.monogame.net/articles/tutorials/building_2d_games/
You can reference this tutorial to understand how to implement the core in more specific ways
