namespace Xrf.CarouselDemo.Common

open XamarinForms.Reactive.FSharp

open ReactiveUI

open System

open DynamicData.Binding
open DynamicData

open ObservableExtensions
open LocatorDefaults

type DashboardViewModel(?host: IScreen) =
    inherit PageViewModel()
    let host = LocateIfNone host
    let postcard n = new Postcard(Image = sprintf "image%i" n, Description = sprintf "Image %i" n)
    let images = new SourceList<Postcard>()
    member val Images = new ObservableCollectionExtended<Postcard>() :> IObservableCollection<Postcard>
    override this.SetUpCommands() =
        images.Connect().Bind(this.Images).Subscribe() |> disposeWith this.PageDisposables |> ignore
        images.AddRange([postcard 1; postcard 2; postcard 3; postcard 4; postcard 5; postcard 6])
    interface IRoutableViewModel with
        member __.HostScreen = host
        member __.UrlPathSegment = "Dashboard"
