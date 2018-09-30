namespace Xrf.Carousel.Common

open XamarinForms.Reactive.FSharp.Themes
open Xamarin.Forms
open XamarinForms.Reactive.FSharp
open ViewHelpers
open System

type Postcard() =
    member val Image = String.Empty with get, set
    member val Description = String.Empty with get, set

type PostcardDisplay(theme: Theme) =
    inherit StackLayout()
    let image = theme.GenerateImage()
    let descriptionLabel = 
        theme.GenerateLabel() 
            |> withHorizontalTextAlignment TextAlignment.Center 
            |> withHorizontalOptions LayoutOptions.CenterAndExpand 
    do 
        base.Orientation <- StackOrientation.Vertical
        base.Children.Add image
        base.Children.Add descriptionLabel
    override this.OnBindingContextChanged() =
        base.OnBindingContextChanged()
        match this.BindingContext with 
        | :? Postcard as postcard -> 
            image.Source <- ImageSource.FromResource postcard.Image
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

    let withCarouselItemsSource disposables (source:ReactiveList<'a>) (element: #SfCarousel) = 
        let populateElement() = match source.Count > 0 with | true -> element.ItemsSource <- source.ToArray() |> Seq.cast<obj> | false -> ()
        populateElement()
        source.ItemsAdded.Subscribe(fun _ -> populateElement()) |> disposeWith disposables |> ignore
        element
    let withSelectedIndex selectedIndex (element: #SfCarousel) = element.SelectedIndex <- selectedIndex; element
    let withItemSpacing spacing (element: #SfCarousel) = element.ItemSpacing <- spacing; element
    let withFlowDirection flowDirection (element: #SfCarousel) = element.FlowDirection <- flowDirection; element
    let withViewMode viewMode (element: #SfCarousel) = element.ViewMode <- viewMode; element
    let withCarouselBinding bindingContext (element: #SfCarousel) = element.BindingContext <- bindingContext; element
    let withCarouselItemTemplate (createTemplate: unit -> View) (element: #SfCarousel) = element.ItemTemplate <- new DataTemplate(fun() -> createTemplate() :> obj); element
    let withCarouselTypeTemplate<'view when 'view :> View> (element: SfCarousel) = element.ItemTemplate <- new DataTemplate(typeof<'view>); element
    let withItemHeight itemHeight (element: #SfCarousel) = element.ItemHeight <- itemHeight; element
    let withItemWidth itemWidth (element: #SfCarousel) = element.ItemWidth <- itemWidth; element
