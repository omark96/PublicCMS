namespace PublicCMS.Models;

public enum NavMenuVisibility
{
    Everyone,
    GuestsOnly,
    SignedInOnly,
    AdminOnly,
}

public class NavMenuItem
{
    public int Id { get; set; }
    public string Link { get; set; } = "";
    public string Text { get; set; } = "";
    public string Icon { get; set; } = "";
    public NavMenuVisibility Visibility { get; set; }
    public int Order { get; set; }
}
