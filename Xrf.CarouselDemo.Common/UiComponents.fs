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

module UiComponentExtensions =
    open Telerik.XamarinForms.Primitives

    type Theme with
        member __.GenerateSlideView([<ParamArray>] setUp: (RadSlideView -> unit)[]) = new RadSlideView() |> apply setUp
        member __.GenerateSlideView(view, property, [<ParamArray>] setUp: (RadSlideView -> unit)[]) = new RadSlideView() |> initialise property view |> apply setUp

module ViewHelpers =
    open Telerik.XamarinForms.Primitives

    // Original assignment
    // let withSlideViewItemsSource (source:'a seq) (element: #RadSlideView) = element.ItemsSource <- source |> Seq.cast<obj>; element
    
    // Fix
    let withSlideViewItemsSource (source:'a seq) (element: #RadSlideView) = element.ItemsSource <- source; element
    
    
    let withSlideViewItemTemplate (createTemplate: unit -> View) (element: #RadSlideView) = element.ItemTemplate <- new DataTemplate(fun() -> createTemplate() :> obj); element
    let withSlideViewTypeTemplate<'view when 'view :> View> (element: RadSlideView) = element.ItemTemplate <- new DataTemplate(typeof<'view>); element
    let animated (element: #RadSlideView) = element.IsAnimated <- true; element
    let withHorizontalContentOptions options (element: #RadSlideView) = element.HorizontalContentOptions <- options; element
    let withVerticalContentOptions options (element: #RadSlideView) = element.VerticalContentOptions <- options; element
    let withSlideButtonsSize size (element: #RadSlideView) = element.SlideButtonsSize <- size; element
    let withSelectedIndicatorFontSize size (element: #RadSlideView) = element.SelectedIndicatorFontSize <- size; element
    let withIndicatorFontSize size (element: #RadSlideView) = element.IndicatorFontSize <- size; element
