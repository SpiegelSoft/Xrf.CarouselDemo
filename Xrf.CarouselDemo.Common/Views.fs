namespace Xrf.CarouselDemo.Common

open XamarinForms.Reactive.FSharp

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
                    |> withCarouselItemsSource this.ViewModel.PageDisposables this.ViewModel.Images
                    |> withHeightAndWidthRequest 200.0 200.0
                    |> withCarouselItemTemplate(fun () -> PostcardDisplay(theme) :> View)
            |]) |> createFromRows :> View
