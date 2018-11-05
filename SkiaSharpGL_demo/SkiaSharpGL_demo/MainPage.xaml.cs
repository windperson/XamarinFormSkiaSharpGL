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
        private readonly Dictionary<long, SKPath> _temporaryPaths = new Dictionary<long, SKPath>();
        private readonly List<SKPath> _paths = new List<SKPath>();

        public MainPage()
        {
            InitializeComponent();
        }

        private void SKGLView_OnPaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            canvas.Clear(SKColors.White);

            DrawPath(canvas);
        }

        private void DrawPath(SKCanvas canvas)
        {
            var touchPathStroke = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Purple,
                StrokeWidth = 5
            };

            // draw the paths,
            // use ToList() to prevent touch event occured at once with Paint event handler,
            // causing instance level variable's data structure consistence.
            // https://stackoverflow.com/a/604843
            foreach (var touchPath in _temporaryPaths.ToList())
            {
                canvas.DrawPath(touchPath.Value, touchPathStroke);
            }
            foreach (var touchPath in _paths.ToList())
            {
                canvas.DrawPath(touchPath, touchPathStroke);
            }
        }



        private void SKGLView_OnTouch(object sender, SKTouchEventArgs e)
        {
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    // start of a stroke
                    var p = new SKPath();
                    p.MoveTo(e.Location);
                    _temporaryPaths[e.Id] = p;
                    break;
                case SKTouchAction.Moved:
                    // the stroke, while pressed
                    if (e.InContact && _temporaryPaths.ContainsKey(e.Id))
                    {
                        _temporaryPaths[e.Id].LineTo(e.Location);
                    }
                    break;
                case SKTouchAction.Released:
                    // end of a stroke
                    _paths.Add(_temporaryPaths[e.Id]);
                    _temporaryPaths.Remove(e.Id);
                    break;
                case SKTouchAction.Cancelled:
                    // we don't want that stroke
                    _temporaryPaths.Remove(e.Id);
                    break;
            }

            // we have handled these events
            e.Handled = true;

            // update the UI
            ((SKGLView)sender).InvalidateSurface();
        }
    }
}
