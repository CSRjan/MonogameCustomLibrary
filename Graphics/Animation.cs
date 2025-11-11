using System;
using System.Collections.Generic;

namespace Game_Library.Graphics
{
    public class Animation
    {
        public string Name { get; set; }
        /// <summary>
        /// The texture regions that make up the frames of this animation.  The order of the regions within the collection
        /// are the order that the frames should be displayed in.
        /// </summary>
        public List<TextureRegion> Frames { get; set; }

        /// <summary>
        /// The amount of time to delay between each frame before moving to the next frame for this animation.
        /// </summary>
        /// <remarks>
        /// Default is a 30 FPS Animation (500 Milliseconds)
        /// </remarks>
        public TimeSpan Delay { get; set; } = TimeSpan.FromMilliseconds(500);
        /// <summary>
        /// Will the animation loop once it plays all it's frames
        /// </summary>
        /// <remarks>
        /// Default is set to true
        /// </remarks>
        public bool loop { get; set; } = true;
        /// <summary>
        /// If the animation is finshed
        /// </summary>
        public bool finished { get; set; } = false;
        /// <summary>
        /// The frames the animation is animated on
        /// </summary>
        public List<int> FrameCounts { get; set; }
        /// <summary>
        /// Creates a new animation.
        /// </summary>
        public Animation()
        {
            Frames = new List<TextureRegion>();
            Delay = TimeSpan.FromMilliseconds(100);
        }

        /// <summary>
        /// Creates a new animation with the specified frames and delay.
        /// </summary>
        /// <param name="frames">An ordered collection of the frames for this animation.</param>
        /// <param name="delay">The amount of time to delay between each frame of this animation.</param>
        /// <param name="looping">Will the animation reset once it hits the final frame.</param>
        /// <param name="done">If you set looping to false, and need an event to happen once it finishes, the animation file will read this.</param>
        /// <param name="frameCounts">The amount of times a frame is stayed on, 1 is the default.</param>
        public Animation(string name, List<TextureRegion> frames, TimeSpan delay, bool looping, bool done, List<int> frameCounts)
        {
            Name = name;
            Frames = frames;
            Delay = delay;
            loop = looping;
            finished = done;
            FrameCounts = frameCounts;
        }

    }
}
