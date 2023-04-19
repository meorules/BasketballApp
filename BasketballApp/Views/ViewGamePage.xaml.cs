using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BasketballApp.ViewModels;
using BasketballApp.Models;
using BasketballApp.Services;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace BasketballApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class ViewGamePage : ContentPage
  {

    public ViewGamePage()
    {
      InitializeComponent();
      this.BindingContext = new ViewGameViewModel();
      Title = "View Game Data";

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
      var viewModel = (ViewGameViewModel)BindingContext;
      if (ApplicationData.currentlySelectedTeam == null || ApplicationData.currentlySelectedGame == null)
      {
        await Shell.Current.DisplayAlert("Games List Error", "Please select a team to view their games", "OK");
        await Shell.Current.GoToAsync("//HomePage");
      }
      else
      {
        viewModel.initialiseData();
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

    bool reDraw = false;

    void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
      var viewModel = (ViewGameViewModel)BindingContext;

      SKSurface surface = args.Surface;
      SKCanvas canvas = surface.Canvas;

      int width = args.Info.Width;
      int height = args.Info.Height;
      int middleX = width / 2;
      int middleY = height / 2;

      /*      canvas.DrawRoundRect(1430, 260, 1500, 1000,10,10, halfcourtBackgroundColor);
       *      
      */
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
      List<GameLogActivity> makes = viewModel.returnShots(1);
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
      List<GameLogActivity> misses = viewModel.returnShots(2);
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

    private void CloseBoxScore(object sender, EventArgs e)
    {
      BoxScoreOpen.IsVisible = true;
      BoxScoreClose.IsVisible = false;
      BoxScoreClose.IsEnabled = false;
      BoxScoreBackground.IsVisible = false;

      BoxScoreStack.IsVisible = false;
      BoxScoreStack.Children.Clear();
    }

    private void ViewBoxScore(object sender, EventArgs e)
    {
      var viewModel = (ViewGameViewModel)BindingContext;

      BoxScoreOpen.IsVisible = false;
      BoxScoreClose.IsVisible = true;
      BoxScoreClose.IsEnabled = true;

      BoxScoreBackground.IsVisible = true;

      StackLayout textStack = new StackLayout
      {
        Padding = new Thickness(5),
        Spacing = 10
      };
      Label titleLabel = new Label();

      titleLabel.Text = "Player Name  |  Minutes  | Points   |   Rebs    |   Assist   |   Steals   |   Blocks   |   FG   |   FT   |  3PTs  |  Turnovers  |   PF   |   +/-  ";
      titleLabel.TextColor = Color.Navy;
      titleLabel.FontAttributes = FontAttributes.Bold;


      textStack.Children.Add(titleLabel);

      List<BoxScore> boxes = viewModel.currentBoxScores;
      for (int i = 0; i < boxes.Count; i++)
      {
        if (boxes[i].minutesOnCourt != TimeSpan.Zero)
        {
          string boxScoreText = boxes[i].ToString();
          Label newLabel = new Label();
          newLabel.Text = boxScoreText;
          newLabel.FontAttributes = FontAttributes.Bold;
          newLabel.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
          newLabel.FontFamily = "Eigrantch";
          //newLabel.BackgroundColor = Color.White;
          newLabel.TextColor = Color.Black;
          textStack.Children.Add(newLabel);
        }
      }

      ScrollView scrollView = new ScrollView
      {
        Content = textStack,
        VerticalOptions = LayoutOptions.FillAndExpand,
        Padding = new Thickness(5, 0),
      };



      BoxScoreStack.Children.Add(scrollView);

      BoxScoreStack.InputTransparent = false;

      BoxScoreStack.IsVisible = true;

    }

    private async void BackClicked(object sender, EventArgs e)
    {
      await Shell.Current.GoToAsync("//ViewGamesListPage");
    }
  }
}