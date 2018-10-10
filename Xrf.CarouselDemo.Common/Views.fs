namespace Xrf.CarouselDemo.Common

open XamarinForms.Reactive.FSharp

open Syncfusion.SfCarousel.XForms
open Themes
open Xamarin.Forms
open Xrf.Carousel.Common

open UiExtensions
open ViewHelpers

type DashboardView(theme: Theme) =
    inherit ContentPage<DashboardViewModel, DashboardView>(theme)
    new() = new DashboardView(Themes.DefaultTheme)
    override this.CreateContent() =
        theme.GenerateGrid([Star 1], [Star 1]) |> withRow(
            [|
                theme.GenerateCarousel()
                    |> withCarouselItemsSource this.ViewModel.Images
                    |> withCarouselItemTemplate(fun () -> PostcardDisplay(theme) :> View)
            |]) |> createFromRows :> View
