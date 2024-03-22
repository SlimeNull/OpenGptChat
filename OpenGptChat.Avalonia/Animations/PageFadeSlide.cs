using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using Avalonia.Styling;

namespace OpenGptChat.Animations
{
    internal class PageFadeSlide : IPageTransition
    {
        /// <summary>
        /// The axis on which the PageSlide should occur
        /// </summary>
        public enum SlideAxis
        {
            Vertical,
            Horizontal,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossFade"/> class.
        /// </summary>
        public PageFadeSlide()
        {
        }

        public double Offset { get; set; } = 10;

        /// <summary>
        /// Gets the duration of the animation.
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets element entrance easing.
        /// </summary>
        public Easing FadeInEasing { get; set; } = new LinearEasing();

        /// <summary>
        /// Gets or sets element exit easing.
        /// </summary>
        public Easing FadeOutEasing { get; set; } = new LinearEasing();

        /// <summary>
        /// Gets the orientation of the animation.
        /// </summary>
        public SlideAxis Orientation { get; set; }

        /// <inheritdoc cref="Start(Visual, Visual, CancellationToken)" />
        public async Task Start(Visual? from, Visual? to, CancellationToken cancellationToken)
        {
            AvaloniaProperty translateProperty = Orientation == SlideAxis.Horizontal ? TranslateTransform.XProperty : TranslateTransform.YProperty;

            var fadeOutAnimation = new Animation
            {
                Easing = FadeOutEasing,
                Children =
                {
                    new KeyFrame()
                    {
                        Setters =
                        {
                            //new Setter
                            //{
                            //    Property = translateProperty,
                            //    Value = 0d
                            //},
                            new Setter
                            {
                                Property = Visual.OpacityProperty,
                                Value = 1d
                            },
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame()
                    {
                        Setters =
                        {
                            //new Setter
                            //{
                            //    Property = translateProperty,
                            //    Value = -Offset
                            //},
                            new Setter
                            {
                                Property = Visual.OpacityProperty,
                                Value = 0d
                            },
                        },
                        Cue = new Cue(1d)
                    }

                }
            };
            var fadeInAnimation = new Animation
            {
                Easing = FadeInEasing,
                Children =
                {
                    new KeyFrame()
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = translateProperty,
                                Value = Offset
                            },
                            new Setter
                            {
                                Property = Visual.OpacityProperty,
                                Value = 0d
                            },
                        },
                        Cue = new Cue(0d)
                    },
                    new KeyFrame()
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = translateProperty,
                                Value = 0d,
                            },
                            new Setter
                            {
                                Property = Visual.OpacityProperty,
                                Value = 1d
                            },
                        },
                        Cue = new Cue(1d)
                    }

                }
            };

            fadeOutAnimation.Duration = fadeInAnimation.Duration = Duration;

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            Task? fromTask = null;
            Task? toTask = null;

            if (from != null)
            {
                fromTask = fadeOutAnimation.RunAsync(from, cancellationToken);
            }

            if (to != null)
            {
                to.IsVisible = true;
                toTask = fadeInAnimation.RunAsync(to, cancellationToken);
            }

            if (fromTask != null && from != null)
            {
                await fromTask;
                if (!cancellationToken.IsCancellationRequested)
                    from.IsVisible = false;
            }

            if (toTask != null)
            {
                await toTask;
            }
        }

        /// <summary>
        /// Starts the animation.
        /// </summary>
        /// <param name="from">
        /// The control that is being transitioned away from. May be null.
        /// </param>
        /// <param name="to">
        /// The control that is being transitioned to. May be null.
        /// </param>
        /// <param name="forward">
        /// Unused for cross-fades.
        /// </param>
        /// <param name="cancellationToken">allowed cancel transition</param>
        /// <returns>
        /// A <see cref="Task"/> that tracks the progress of the animation.
        /// </returns>
        Task IPageTransition.Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
        {
            return Start(from, to, cancellationToken);
        }
    }
}