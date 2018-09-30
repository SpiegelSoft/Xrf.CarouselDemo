namespace Xrf.Carousel.Common

open XamarinForms.Reactive.FSharp

type IXrfPlatform =
    inherit IPlatform
    abstract member GetMetaDataEntry: key:string -> string


