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
open Xamarin.Forms

type DashboardViewModel(?host: IScreen) =
    inherit PageViewModel()
    let host = LocateIfNone host
    let postcard n = { Image = sprintf "image%i" n; Description = sprintf "Image %i" n }
    let images = new SourceList<Postcard>()
    member val Images = new ObservableCollectionExtended<Postcard>() :> IObservableCollection<Postcard>
    override this.SetUpCommands() =
        let handle (changes: IChangeSet<Postcard>) =
            changes |> ignore
        images.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(this.Images).Subscribe(handle) |> disposeWith this.PageDisposables |> ignore
        
        // Uncomment this line and comment out the rest of the method to see the expected behaviour of the
        // component. Images implements IObservableCollection<Postcard>, which extends INotifyCollectionChanged<Postcard>,
        // and so I would expect the slideview to populate itself when the collection is changed after the page has loaded.
        // This method is called in the PageAppearing() handler, so by uncommenting the line below, we are populating the
        // collection before the slideview is rendered. However, in a typical use-case, I would like to load the page and then
        // populate the collection asynchronously.

        //images.AddRange([|postcard 1; postcard 2; postcard 3; postcard 4; postcard 5; postcard 6|])
        
        let generateImages(_:Unit) = async { return [|postcard 1; postcard 2; postcard 3; postcard 4; postcard 5; postcard 6|] }
        let generateImagesCommand = createFromAsync(generateImages, None)
        generateImagesCommand.Execute().Subscribe(images.AddRange) |> disposeWith this.PageDisposables |> ignore
    interface IRoutableViewModel with
        member __.HostScreen = host
        member __.UrlPathSegment = "Dashboard"
