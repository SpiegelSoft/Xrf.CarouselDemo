namespace Xrf.CarouselDemo.Common

open XamarinForms.Reactive.FSharp.Themes
open Xamarin.Forms
open XamarinForms.Reactive.FSharp
open ViewHelpers
open System

type Postcard = {
    Image: string
    Description: string
}

type PostcardDisplay(theme: Theme) =
    inherit Grid()
    let image = theme.GenerateImage()
    let descriptionLabel = 
        theme.GenerateLabel() 
            |> withHorizontalTextAlignment TextAlignment.Center 
            |> withHorizontalOptions LayoutOptions.CenterAndExpand 
    do 
        base.Children.Add image; Grid.SetRow(image, 0); Grid.SetColumn(image, 0)
        base.Children.Add descriptionLabel; Grid.SetRow(descriptionLabel, 1); Grid.SetColumn(descriptionLabel, 0)
        base.WidthRequest <- 400.0
    override this.OnBindingContextChanged() =
        base.OnBindingContextChanged()
        match box this.BindingContext with 
        | :? Postcard as postcard -> 
            image.Source <- ImageSource.FromFile postcard.Image
            descriptionLabel.Text <- postcard.Description
        | _ -> 
            image.Source <- null
            descriptionLabel.Text <- null

module UiExtensions =
    open Syncfusion.SfCarousel.XForms
    open System

    type Theme with
        member __.GenerateCarousel([<ParamArray>] setUp: (SfCarousel -> unit)[]) = new SfCarousel() |> apply setUp
        member __.GenerateCarousel(view, property, [<ParamArray>] setUp: (SfCarousel -> unit)[]) = new SfCarousel() |> initialise property view |> apply setUp

module ViewHelpers =
    open Syncfusion.SfCarousel.XForms
    open System.Linq
    open XamarinForms.Reactive.FSharp.ObservableExtensions
    open ReactiveUI
    open Xamarin.Forms

    let withCarouselItemsSource (source:'a seq) (element: #SfCarousel) = element.ItemsSource <- source |> Seq.cast<obj>; element
    let withSelectedIndex selectedIndex (element: #SfCarousel) = element.SelectedIndex <- selectedIndex; element
    let withItemSpacing spacing (element: #SfCarousel) = element.ItemSpacing <- spacing; element
    let withFlowDirection flowDirection (element: #SfCarousel) = element.FlowDirection <- flowDirection; element
    let withViewMode viewMode (element: #SfCarousel) = element.ViewMode <- viewMode; element
    let withCarouselBinding bindingContext (element: #SfCarousel) = element.BindingContext <- bindingContext; element
    let withCarouselItemTemplate (createTemplate: unit -> View) (element: #SfCarousel) = element.ItemTemplate <- new DataTemplate(fun() -> createTemplate() :> obj); element
    let withCarouselTypeTemplate<'view when 'view :> View> (element: SfCarousel) = element.ItemTemplate <- new DataTemplate(typeof<'view>); element
    let withItemHeight itemHeight (element: #SfCarousel) = element.ItemHeight <- itemHeight; element
    let withItemWidth itemWidth (element: #SfCarousel) = element.ItemWidth <- itemWidth; element
