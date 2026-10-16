using Microsoft.AspNetCore.Components;

namespace PublicCMS;

public class ContentChangeEventArgs : EventArgs
{
    public string? Value { get; set; }
}

[EventHandler("oncontentchange", typeof(ContentChangeEventArgs),
    enableStopPropagation: true, enablePreventDefault: true)]
public static class EventHandlers { }
