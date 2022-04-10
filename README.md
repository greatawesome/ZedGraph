# ZedGraph

*The charting library for .NET*

created by John Champion
customized by Paul Martinsen
ported to Eto.Forms by Brian Gallaway

forked from https://sourceforge.net/projects/zedgraph/

ZedGraph is a class library, user control, and web control for .net, written in C#, for drawing 2D Line, Bar, and Pie Charts. 
It features full, detailed customization capabilities, but most options have defaults for ease of use.

## Eto.Forms specific changes

### Modifications to ZedGraph Project

1. Default fonts reverted to "Arial" to remove references to System.Windows.Forms in ZedGraph (WinForms control's constructor changes them back to WinForm defaults)
2. ReversibleFrame: GDI calls placed in try/catch, as they would not work on non-Windows platforms
3. Moved Editors/FormatSelector to ZedGraph.Web (which appeared to be the only place it is used)
4. NetStandard2.0 target added

### Missing items in Eto's ZedGraphControl versus WinForms ZedGraphControl

1. Mouse DoubleClick not yet handled (coming soon)
2. Printing
3. Scrollbars / Scrollable area
4. DataSourcePointList equivalent (WinForms DataSourcePointList implements an IPointList that uses WinForms binding)
5. Proper Reversible Frame support to create drag/zoom window rectangle
6. Support for EMF Image format
7. Locale support might be broken for non-English locales

## Customization
* Basic support for cursors. Cursors are horizontal or vertical lines you can add to the 
  graph as guides or reference markers. The user can (optionally) move cursors using the
  mouse. Todo: serialization, labels. 

## Download

ZedGraph is available via NuGet:
- [ZedGraph](http://nuget.org/packages/ZedGraph)
- [ZedGraph.WinForms](http://nuget.org/packages/ZedGraph.WinForms)
