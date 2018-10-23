namespace Xrf.CarouselDemo.Common

open XamarinForms.Reactive.FSharp

open Themes
open Xamarin.Forms
open Xrf.Carousel.Common

open UiComponentExtensions
open ViewHelpers

type DashboardView(theme: Theme) =
    inherit ContentPage<DashboardViewModel, DashboardView>(theme)
    new() = new DashboardView(Themes.DefaultTheme)
    override this.CreateContent() =
        theme.GenerateGrid([Star 1], [Star 1]) |> withRow(
            [|
                theme.GenerateListView(ListViewCachingStrategy.RetainElement)
                    |> withItemsSource this.ViewModel.Images
                    |> withCellTemplate(fun () -> PostcardDisplayCell(theme))
            |]) |> createFromRows :> View
