using System;
using System.Collections.Generic;
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
        public MainPage()
        {
            InitializeComponent();
        }

        private void SKGLView_OnPaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            canvas.Clear(SKColors.White);

            DrawCircle(canvas);
        }

        private void DrawCircle(SKCanvas canvas)
        {
            using (var circleFill = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColors.Blue
            })
            {
                // draw the circle fill
                canvas.DrawCircle(100, 100, 40, circleFill);
            }

            // create the paint for the circle border
            using (var circleBorder = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Red,
                StrokeWidth = 5
            })
            {
                // draw the circle border
                canvas.DrawCircle(100, 100, 40, circleBorder);
            }
        }
    }
}
