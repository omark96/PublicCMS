namespace PublicCMS.Models;

public class Theme
{
    public int Id { get; set; }
    public bool IsSelected { get; set; }
    public string Name { get; set; } = "";
    public string Css { get; set; } = "";
    public string HeaderCss { get; set; } = "";
    public string ImageCss { get; set; } = "";
    public string ParagraphCss { get; set; } = "";
    public string LinkCss { get; set; } = "";


    public static Theme DefaultTheme()
    {
        return new()
        {
            HeaderCss = "background: red;"
        };
    }

    public void GenerateCss()
    {
        Css = $$"""
            .post-header {
                {{HeaderCss}}
            }
            .post-image {
                {{ImageCss}}
            }
            .post-paragraph {
                {{ParagraphCss}}
                a {
                    {{LinkCss}}
                }
            }
            """;
    }
}
