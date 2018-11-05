using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SkiaSharpGL_demo
{
    public partial class MainPage : ContentPage
    {
        readonly Stopwatch _stopwatch = new Stopwatch();
        private bool _pageIsActive;
        private float _scale;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _pageIsActive = true;
            await AnimationLoop();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _pageIsActive = false;
        }

        async Task AnimationLoop()
        {
            _stopwatch.Start();

            while (_pageIsActive)
            {
                var cycleTime = slider.Value;
                var t = _stopwatch.Elapsed.TotalSeconds % cycleTime / cycleTime;
                _scale = (1 + (float)Math.Sin(2 * Math.PI * t)) / 2;
                canvasView.InvalidateSurface();
                await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));
            }
            _stopwatch.Stop();
        }

        private void SKGLView_OnPaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            var info = e.RenderTarget;
            var maxRadius = 0.75f * Math.Min(info.Width, info.Height) / 2;
            var minRadius = 0.25f * maxRadius;

            var xRadius = minRadius * _scale + maxRadius * (1 - _scale);
            var yRadius = maxRadius * _scale + minRadius * (1 - _scale);

            using (var paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Stroke;
                paint.Color = SKColors.Blue;
                paint.StrokeWidth = 50;
                canvas.DrawOval(info.Width / 2.0f, info.Height / 2.0f, xRadius, yRadius, paint);

                paint.Style = SKPaintStyle.Fill;
                paint.Color = SKColors.SkyBlue;
                canvas.DrawOval(info.Width / 2.0f, info.Height / 2.0f, xRadius, yRadius, paint);
            }
        }

    }
}
