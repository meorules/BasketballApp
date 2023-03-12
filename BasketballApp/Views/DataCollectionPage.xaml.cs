using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

namespace BasketballApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class DataCollectionPage : ContentPage
  {

    SKPaint greenLineCircle = new SKPaint
    {
      Style = SKPaintStyle.Stroke,
      Color = SKColors.Green,
      StrokeWidth = 2
    };

    SKPaint redCross = new SKPaint
    {
      Style = SKPaintStyle.Stroke,
      Color = SKColors.Red,
      StrokeWidth = 2
    };

    SKPaint whiteStrokePaint = new SKPaint
    {
      Style = SKPaintStyle.Stroke,
      Color = SKColors.White,
      StrokeWidth = 1,
      StrokeCap = SKStrokeCap.Butt,
      IsAntialias = true
    };

    SKPaint halfcourtBackgroundColor = new SKPaint
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = SKColors.White,
      ColorF = SKColors.Navy,
      StrokeCap = SKStrokeCap.Round,
      IsAntialias = true
    };

    SKPath basketballHalfCourt = new SKPath();
    SKPath redCrossPath = new SKPath();

    public DataCollectionPage()
    {
      InitializeComponent();
      Title = "Data Collection Page";

      Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
      {
        canvasView.InvalidateSurface();
        Timer.Text = DateTime.Now.ToString("mm:ss");
        return true;
      });

      redCrossPath.MoveTo(0, 0);
      redCrossPath.LineTo(10, 10);
      redCrossPath.MoveTo(10, 0);
      redCrossPath.LineTo(0, 10);
      redCrossPath.Close();
      

      basketballHalfCourt.MoveTo(20, 0);
      basketballHalfCourt.LineTo(20, 30);
      basketballHalfCourt.CubicTo(20, 125, 195, 125, 195, 30);
      basketballHalfCourt.LineTo(195, 0);
      basketballHalfCourt.MoveTo(135, 65);
      basketballHalfCourt.LineTo(80, 65);
      basketballHalfCourt.LineTo(80, 0);
      basketballHalfCourt.LineTo(135, 0);
      basketballHalfCourt.LineTo(135, 65);
      basketballHalfCourt.AddCircle(107.5f, 65, 22); 
      basketballHalfCourt.MoveTo(0, 0);
      basketballHalfCourt.LineTo(0, 130);
      basketballHalfCourt.LineTo(210, 130);
      basketballHalfCourt.LineTo(210, 0);
      basketballHalfCourt.LineTo(0, 0);
      basketballHalfCourt.Close();

    }

    void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {

      SKSurface surface = args.Surface;
      SKCanvas canvas = surface.Canvas;
      canvas.Clear(SKColors.AliceBlue);

      int width = args.Info.Width;
      int height = args.Info.Height;
      int middleX = width / 2;
      int middleY = height / 2;

      canvas.DrawRoundRect(1430, 260, 1500, 1000,10,10, halfcourtBackgroundColor);

      //Drawing the HalfCourt
      canvas.Save();
      canvas.Translate(width, height);
      canvas.Scale(6.5f, 7);
      canvas.RotateDegrees(180);
      canvas.DrawPath(basketballHalfCourt, whiteStrokePaint);
      canvas.Restore();

      //Green Circle Drawing, this can be called to add where makes are
      canvas.Save();
      canvas.Translate(600, 750);
      canvas.Scale(5, 5);
      canvas.DrawCircle(0, 0, 5, greenLineCircle);
      canvas.Restore();

      //Drawing Red Crosses for misses
      canvas.Save();
      canvas.Translate(500, 530);
      canvas.Scale(5, 5);
      canvas.DrawPath(redCrossPath, redCross);
      canvas.Restore();

      /*SKImageInfo info = args.Info;
    }

    /*private void ImageButton_Clicked(object sender, EventArgs e)
    {
      myframe.IsVisible = !myframe.IsVisible;
    }

    */
    }
    private void TouchEffect_TouchAction(object sender, TouchTracking.TouchActionEventArgs args)
    {

      var x = args.Location.X;
      var y = args.Location.Y;

      //Opens up menu to select player, shot type and make or miss
      //Once this is done, these get added to the data storage and then 

      var v = new DataCollectionPage();
      AbsoluteLayout.SetLayoutBounds(v, new Rectangle(x, y, 0.25, 0.25));
      AbsoluteLayout.SetLayoutFlags(v, AbsoluteLayoutFlags.SizeProportional);

      //myabs.Children.Add(v);
    }
  }
}