using System;
using Microsoft.Xna.Framework;

namespace Game_Library.Graphics
{
    public class AnimatedSprites : Sprite
    {
        public int _currentFrame;
        private TimeSpan _elapsed;
        public Animation _animation;
        TimeSpan estimatedDelay;
        public bool stop;

        /// <summary>
        /// Gets or Sets the animation for this animated sprite.
        /// </summary>
        public Animation Animation
        {
            get => _animation;
            set
            {
                _animation = value;
                Region = _animation.Frames[0];
            }
        }

        /// <summary>
        /// Creates a new animated sprite.
        /// </summary>
        public AnimatedSprites() { }

        /// <summary>
        /// Creates a new animated sprite with the specified frames and delay.
        /// </summary>
        /// <param name="animation">The animation for this animated sprite.</param>
        public AnimatedSprites(Animation animation)
        {
            Animation = animation;
        }

        public void swapAnimations(Animation newAnimation)
        {
            _currentFrame = 0;
            _animation = newAnimation;
            _animation.finished = false;
            estimatedDelay = TimeSpan.FromMilliseconds(_animation.Delay.TotalMilliseconds * _animation.FrameCounts[_currentFrame]);
            _elapsed.Equals(0);
        }
        /// <summary>
        /// Updates this animated sprite.
        /// </summary>
        /// <param name="gameTime">A snapshot of the game timing values provided by the framework.</param>
        public void Update(GameTime gameTime)
        {
            _elapsed += gameTime.ElapsedGameTime;
            if (_elapsed >= estimatedDelay && !stop)
            {
                _elapsed -= estimatedDelay;
                if (_animation.loop)
                {
                    if (_currentFrame >= _animation.Frames.Count - 1)
                    {
                        _currentFrame = 0;
                    }
                    else
                    {
                        _currentFrame++;
                        estimatedDelay = TimeSpan.FromMilliseconds(_animation.Delay.TotalMilliseconds * _animation.FrameCounts[_currentFrame]);
                    }
                }
                else
                {
                    if (!_animation.finished)
                    {
                        if (_currentFrame >= _animation.Frames.Count - 1)
                        {
                            _animation.finished = true;
                        }
                        else
                        {
                            _currentFrame++;
                            estimatedDelay = TimeSpan.FromMilliseconds(_animation.Delay.TotalMilliseconds * _animation.FrameCounts[_currentFrame]);
                        }
                    }
                }

                Region = _animation.Frames[_currentFrame];
            }
        }

    }
}
