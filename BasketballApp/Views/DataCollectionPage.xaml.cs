using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BasketballApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class DataCollectionPage : ContentPage
  {
    public DataCollectionPage()
    {
      InitializeComponent();
      Title = "Simple Circle";

      SKCanvasView canvasView = new SKCanvasView();
      canvasView.PaintSurface += OnCanvasViewPaintSurface;
      Content = canvasView;
    }

    void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
      SKImageInfo info = args.Info;
      SKSurface surface = args.Surface;
      SKCanvas canvas = surface.Canvas;

      canvas.Clear();

      SKPaint paint = new SKPaint
      {
        Style = SKPaintStyle.Stroke,
        Color = Color.Red.ToSKColor(),
        StrokeWidth = 25
      };

      canvas.DrawCircle(info.Width / 2, info.Height / 2, 100, paint);

    }
  }
}