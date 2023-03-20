﻿using System;
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

using BasketballApp.ViewModels;
using BasketballApp.Models;
using Xamarin.Forms.Internals;

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

    TimeSpan clock= new TimeSpan(0, 12, 0);
    TimeSpan shotClock = new TimeSpan(0, 0, 24);

    bool timePlay = false;

    bool blocked = false;

    bool reDraw = false;

    public DataCollectionPage()
    {
      this.BindingContext = new GameObjectViewModel();
      var viewModel = (GameObjectViewModel)BindingContext;


      InitializeComponent();
      

      Title = "Data Collection Page";

      Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
      {
        if (reDraw)
        {
          canvasView.InvalidateSurface();
          reDraw = false;
        }
        clockButton.Text = clock.ToString(@"mm\:ss\.f");
        shotClockButton.Text = shotClock.ToString(@"ss\.f");
        if (timePlay)
        {
          //Call Add minutes to each player
          viewModel.AddMinutes(TimeSpan.FromMilliseconds(100));
          clock = clock-TimeSpan.FromMilliseconds(100);
          shotClock = shotClock - TimeSpan.FromMilliseconds(100);

          if (shotClock == TimeSpan.Zero)
          {
            shotClock = new TimeSpan(0, 0, 24);
            timePlay = false;
          }
        }
        // fire the command
        
        //Timer.Text = DateTime.Now.ToString("mm:ss");
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
      var viewModel = (GameObjectViewModel)BindingContext;

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
        List<GameLogActivity> makes = viewModel.returnShots(1);
        foreach (GameLogActivity make in makes)
        {
          if (make != null)
          {
            canvas.Save();
          //canvas.Translate(width/2, height/2);
          float x = (float)(3.525 *  make.positionX);
          float y = (float)(3.546 *  make.positionY);

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

          canvas.Translate(x,y);
            canvas.Scale(5, 5);
            canvas.DrawPath(redCrossPath, redCross);
            canvas.Restore();
          }
        }

    }
    private async void TouchEffect_TouchAction(object sender, TouchTracking.TouchActionEventArgs args)
    {
      if (blocked == false)
      {
        var x = args.Location.X;
        var y = args.Location.Y;
        if (x >= 405 && y >= 80)
        {
          timePlay = false;
          blocked=true;
          string assistedPlayer = "No One";

          string playerName = await Shell.Current.DisplayActionSheet("Pick a Player", "Cancel", null, Player1Name.Text, Player2Name.Text, Player3Name.Text, Player4Name.Text, Player5Name.Text);
          if (playerName != "Cancel")
          {
            string makeOrMiss = await Shell.Current.DisplayActionSheet("Made or Missed?", "Cancel", null, "Made", "Missed");
            if (makeOrMiss != "Cancel")
            {
              bool makeBool = false;
              if (makeOrMiss == "Made")
              {
                assistedPlayer = await Shell.Current.DisplayActionSheet("Who assisted the play?", "Cancel", null,"No One",Player1Name.Text, Player2Name.Text, Player3Name.Text, Player4Name.Text, Player5Name.Text);
                makeBool = true;
              }
              string pointWorth = await Shell.Current.DisplayActionSheet("2PT or 3PT?", "Cancel", null, "2PT", "3PT");

              if (pointWorth != "Cancel")
              {
                int pointsWorth = 2;
                if (pointWorth == "3PT")
                {
                  pointsWorth = 3;
                }
                //ADD SHOT TO SHOT MAP
                var viewModel = (GameObjectViewModel)BindingContext;

                // fire the command
                viewModel.registerShot(x, y, playerName, makeBool, pointsWorth, clock, shotClock,assistedPlayer);
                shotClock = new TimeSpan(0, 0, 24);
                timePlay = true;
                blocked = false;
                reDraw = true;

              }
              else
              {
                blocked = false;
              }
            }
            else
            {
              blocked = false;
            }
          }
          else
          {
            blocked = false;
          }



        }
      }

      //Opens up menu to select player, shot type and make or miss
      //Once this is done, these get added to the data storage and then 

      /*AbsoluteLayout.SetLayoutBounds(v, new Rectangle(x, y, 0.25, 0.25));
      AbsoluteLayout.SetLayoutFlags(v, AbsoluteLayoutFlags.SizeProportional);*/

      //myabs.Children.Add(v);
    }

    void TimerStart(object sender, EventArgs args) {
      timePlay = true;
    }

    void TimerPause(object sender, EventArgs args)
    {
      timePlay = false;
    }

    private void CloseBoxScore(object sender, EventArgs e)
    {
      BoxScoreOpen.IsVisible = true;
      BoxScoreClose.IsVisible = false;
      BoxScoreClose.IsEnabled = false;
      //BoxScore.IsVisible = false;
    }

    private void ViewBoxScore(object sender, EventArgs e)
    {
      var viewModel = (GameObjectViewModel)BindingContext;

      BoxScoreOpen.IsVisible = false;
      BoxScoreClose.IsVisible = true;
      BoxScoreClose.IsEnabled = true;
      //BoxScore.IsVisible = true;

      /*BoxScore.ItemsSource = viewModel.currentBoxScores;
      var template = new DataTemplate(typeof(TextCell));
      template.SetBinding(TextCell.TextProperty, "player.Name");
      template.SetBinding(TextCell.TextProperty, "Points");
      BoxScore.ItemTemplate = template;*/
    }
  }


}