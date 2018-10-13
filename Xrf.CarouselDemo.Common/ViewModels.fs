namespace Xrf.CarouselDemo.Common

open XamarinForms.Reactive.FSharp

open ReactiveUI

open System

open DynamicData.Binding
open DynamicData

open ObservableExtensions
open LocatorDefaults
open System.Reactive.Linq

module ReactiveCommands =
    let private create (factory:IObservable<bool> -> ReactiveCommand<'src, 'dest>) (canExecute) =
        let ce = match canExecute with | Some c -> c | None -> Observable.Return<bool>(true)
        let command = factory(ce)
        command.ThrownExceptions.Subscribe() |> ignore
        command.Catch(fun _ -> Observable.Return Unchecked.defaultof<'dest>) |> ignore
        command
    let createFromAsync(operation: 'src -> Async<'dest>, canExecute: IObservable<bool> option) =
        let factory ce = ReactiveCommand.CreateFromTask<'src, 'dest>(operation >> Async.StartAsTask, ce)
        create factory canExecute

open ReactiveCommands

type DashboardViewModel(?host: IScreen) =
    inherit PageViewModel()
    let host = LocateIfNone host
    let postcard n = { Image = sprintf "image%i" n; Description = sprintf "Image %i" n }
    let images = new SourceList<Postcard>()
    member val Images = new ObservableCollectionExtended<Postcard>() :> IObservableCollection<Postcard>
    override this.SetUpCommands() =
        images.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(this.Images).Subscribe() |> disposeWith this.PageDisposables |> ignore
        let generateImages(_:Unit) = async { return [|postcard 1; postcard 2; postcard 3; postcard 4; postcard 5; postcard 6|] }
        let generateImagesCommand = createFromAsync(generateImages, None)
        generateImagesCommand.Execute().Subscribe(images.AddRange) |> disposeWith this.PageDisposables |> ignore
    interface IRoutableViewModel with
        member __.HostScreen = host
        member __.UrlPathSegment = "Dashboard"
