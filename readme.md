# Controls Extensions for Avalonia
---
## ABOUT FOLDER 

> Base
>> Basic, public library

> Controls
>> Control extension implementation

> Utils
>> Extension Method Library
---

## TODO LIST

-[x] ProgressRing
> inherit from Canvas,Implement loading animation

-[x] ClickableView
> Views that can trigger the click event

-[x] CellListView
> a Multiple-Column ListView/ListBox,Just like GridView in UWP

-[x] CellListViewCell
> Item for CellListView,inherit from ClickableView,with Left/Right Click Event

-[x] CircleImage
> inherit from Ellipse,Round picture.Just like PersonPicture in UWP

-[x] ExpandableView
> A view that shows items in a vertically scrolling two-level list
> PrimaryView
>> Main Item,show/hide the SecondView after selection
> SecondView
>> show or hide when select PrimaryView

-[x] ImageRemote
> inherit from Image,loading image from http/https

-[x] ListView
> inherit from ListBox,just like the ListView in UWP

-[x] ListViewItem
> Item for ListView,inherit from ListBoxItem

-[x] MessageBox
> Show message window

-[x] NotifyWindow
> Notify message window,the transition animation can be displayed according to the preset and automatically closed after a certain period of time

-[x] PopupDialog
> inherit from Popup,show message dialog and automatically shut down after a certain period of time

-[x] PopupMenu
> inherit from Window,close after selecting item and trigger the event

-[x] HorizontalItemsRepeater
> inherit from ItemsRepeater,Horizontal layout with Clickable Item

-[x] VerticalItemsRepeater
> inherit from ItemsRepeater,Vertical layout with Clickable Item

-[x] ItemsRepeaterContent
> Item for ItemsRepeater with Clickable Event

-[ ] ScrollView
>inherit from ScrollViewer,