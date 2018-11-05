using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SkiaSharpGL_demo
{
    public partial class MainPage : ContentPage
    {
        Stopwatch stopwatch = new Stopwatch();
        bool pageIsActive;
        float scale;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pageIsActive = true;
            AnimationLoop();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageIsActive = false;
        }

        async Task AnimationLoop()
        {
            stopwatch.Start();

            while (pageIsActive)
            {
                double cycleTime = slider.Value;
                double t = stopwatch.Elapsed.TotalSeconds % cycleTime / cycleTime;
                scale = (1 + (float)Math.Sin(2 * Math.PI * t)) / 2;
                canvasView.InvalidateSurface();
                await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));
            }
            stopwatch.Stop();
        }

        private void SKGLView_OnPaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {

            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            var info = e.RenderTarget;

            var maxRadius = 0.75f * Math.Min(info.Width, info.Height) / 2;
            var minRadius = 0.25f * maxRadius;

            var xRadius = minRadius * scale + maxRadius * (1 - scale);
            var yRadius = maxRadius * scale + minRadius * (1 - scale);

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
