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
            HeaderCss = """
            font-size: 2rem;
            font-weight: 700;
            line-height: 1.2;
            margin: 0 0 1rem;
            """,
            ImageCss = """
            display: block;
            max-width: 100%;
            height: auto;
            margin: 1.5rem 0;
            border-radius: 4px;
            """,
            ParagraphCss = """
            font-size: 1rem;
            line-height: 1.6;
            margin: 0 0 1rem;
            """,
            LinkCss = """
            color: #0066cc;
            text-decoration: underline;
            text-underline-offset: 2px;
                &:hover {
                    color: #004499;
                }
            """
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
