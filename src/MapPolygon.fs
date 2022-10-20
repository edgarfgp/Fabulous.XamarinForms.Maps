﻿namespace Fabulous.XamarinForms.Maps

open System.Runtime.CompilerServices
open Fabulous
open Fabulous.StackAllocatedCollections
open Fabulous.StackAllocatedCollections.StackList
open Fabulous.XamarinForms
open Xamarin.Forms.Maps

type IMapPolygon =
    inherit IMapElement

module MapPolygon =
    let WidgetKey = Widgets.register<Polygon> ()

    let FillColor = Attributes.defineBindableAppThemeColor Polygon.FillColorProperty

    let GeoPathList =
        Attributes.defineSimpleScalarWithEquality<Position list> "Polygon_GeoPath" (fun _ newValueOpt node ->
            let map = node.Target :?> Polygon

            match newValueOpt with
            | ValueNone -> map.Geopath.Clear()
            | ValueSome geoPaths -> geoPaths |> List.iter map.Geopath.Add)

[<AutoOpen>]
module MapPolygonBuilders =

    type Fabulous.XamarinForms.View with

        static member inline MapPolygon<'msg>(geoPaths: Position list) =
            WidgetBuilder<'msg, IMapPolygon>(
                MapPolygon.WidgetKey,
                AttributesBundle(StackList.one (MapPolygon.GeoPathList.WithValue(geoPaths)), ValueNone, ValueNone)
            )

[<Extension>]
type MapPolygonModifiers =
    [<Extension>]
    static member inline fillColor(this: WidgetBuilder<'msg, #IMapPolygon>, light: FabColor, ?dark: FabColor) =
        this.AddScalar(MapPolygon.FillColor.WithValue(AppTheme.create light dark))

    /// <summary>Link a ViewRef to access the direct Polygon control instance</summary>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IMapPolygon>, value: ViewRef<Polygon>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
