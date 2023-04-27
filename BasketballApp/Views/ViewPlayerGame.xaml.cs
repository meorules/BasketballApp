using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BasketballApp.ViewModels;
using SkiaSharp;
using BasketballApp.Models;
using SkiaSharp.Views.Forms;
using BasketballApp.Services;

namespace BasketballApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ViewPlayerGame : ContentPage
  {

    TimeSpan startTime;
    TimeSpan endTime;
    int quarter;

    public ViewPlayerGame()
    {
      InitializeComponent();

      this.BindingContext = new ViewPlayerGameViewModel();

      Title = "View Player Game Data";

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

    protected override async void OnAppearing()
    {
      base.OnAppearing();
      var viewModel = (ViewPlayerGameViewModel)BindingContext;
      if (ApplicationData.currentlySelectedTeam == null || ApplicationData.currentlySelectedGame == null)
      {
        await Shell.Current.DisplayAlert("Games List Error", "Please select a team to view their games", "OK");
        await Shell.Current.GoToAsync("//HomePage");
      }
      else
      {
        viewModel.initialiseData();
      }
      startTime = TimeSpan.Zero;
      endTime = TimeSpan.FromMinutes(12);
      quarter = 0;
    }

    void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
      var viewModel = (ViewPlayerGameViewModel)BindingContext;
      viewModel.initialiseData();

      SKSurface surface = args.Surface;
      SKCanvas canvas = surface.Canvas;

      int width = args.Info.Width;
      int height = args.Info.Height;

      canvas.Save();
      canvas.Translate(width, height - 5);
      canvas.RotateDegrees(180);
      canvas.DrawRect(0, 0, 1400, 960, halfcourtBackgroundColor);
      canvas.Restore();

      //Drawing the HalfCourt
      canvas.Save();
      canvas.Translate(width - 10, height - 20);
      canvas.Scale(6.5f, 7);
      canvas.RotateDegrees(180);
      canvas.DrawPath(basketballHalfCourt, whiteStrokePaint);
      canvas.Restore();


      //Green Circle Drawing, this can be called to add where makes are
      List<GameLogActivity> makes = viewModel.returnShots(1, startTime, endTime, quarter);
      foreach (GameLogActivity make in makes)
      {
        if (make != null)
        {
          canvas.Save();
          //canvas.Translate(width/2, height/2);
          float x = (float)(3.525 * make.positionX);
          float y = (float)(3.546 * make.positionY);

          canvas.Translate(x, y);
          canvas.Scale(5, 5);
          canvas.DrawCircle(0, 0, 5, greenLineCircle);
          canvas.Restore();
        }

      }


      //Drawing Red Crosses for misses
      List<GameLogActivity> misses = viewModel.returnShots(2, startTime, endTime, quarter);
      foreach (GameLogActivity miss in misses)
      {
        if (miss != null)
        {
          canvas.Save();
          //canvas.Translate(width / 2, height / 2);
          float x = (float)(3.525 * miss.positionX);
          float y = (float)(3.546 * miss.positionY);

          canvas.Translate(x, y);
          canvas.Scale(5, 5);
          canvas.DrawPath(redCrossPath, redCross);
          canvas.Restore();
        }
      }

    }

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

    private async void BackClicked(object sender, EventArgs e)
    {
      ApplicationData.currentlySelectedPlayer = null;
      await Shell.Current.GoToAsync("//ViewGamePage");
    }

    private void QuarterPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (QuarterPicker.SelectedIndex)
      {
        case 0:
          quarter = 0;
          break;
        case 1:
          quarter = 1;
          break;
        case 2:
          quarter = 2;
          break;
        case 3:
          quarter = 3;
          break;
        case 4:
          quarter = 4;
          break;
        default:
          quarter = 0;
          break;
      }
      canvasView.InvalidateSurface();
    }

    private void StartTimePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
      startTime = TimeSpan.FromMinutes(StartTimePicker.SelectedIndex * 2);

      canvasView.InvalidateSurface();

    }

    private void EndTimePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
      endTime = TimeSpan.FromMinutes(EndTimePicker.SelectedIndex * 2 + 2);
      if (EndTimePicker.SelectedIndex == 5)
      {
        endTime = TimeSpan.FromMinutes(5 * 2 + 2 + 0.0001);
      }
      canvasView.InvalidateSurface();

    }

    private void PlayerPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (PlayerPicker.SelectedItem != null)
      {
        Player player = BasketballDBService.getPlayer(PlayerPicker.SelectedItem.ToString());
        if (player != null)
        {
          ApplicationData.currentlySelectedPlayer = player;
          var viewModel = (ViewPlayerGameViewModel)BindingContext;
          viewModel.initialiseData();
          canvasView.InvalidateSurface();
        }

      }
    }
  }
}