namespace PublicCMS.Models.PageElements;

public class ParagraphElement : PostElement
{
    public string Content { get; set; } = "";

    public ParagraphElement()
    {
        Content = "This is some random text!";
    }
}
