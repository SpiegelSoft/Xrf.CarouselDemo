namespace Xrf.CarouselDemo.Common

open XamarinForms.Reactive.FSharp

open ReactiveUI

open System

open LocatorDefaults

type Postcard() =
    member val Image = String.Empty with get, set
    member val Description = String.Empty with get, set

type DashboardViewModel(?host: IScreen) =
    inherit PageViewModel()
    let host = LocateIfNone host
    let postcard n = new Postcard(Image = sprintf "image%i" n, Description = sprintf "Image %i" n)
    member val Images = new ReactiveList<Postcard>([postcard 1; postcard 2; postcard 3; postcard 4; postcard 5; postcard 6])
    interface IRoutableViewModel with
        member __.HostScreen = host
        member __.UrlPathSegment = "Dashboard"
