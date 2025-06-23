using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Strategy;

public enum OutputFormat
{
    Markdown,
    Html
}

public interface IListStrategy
{
    void Start(StringBuilder sb);
    void End(StringBuilder sb);
    void AddListItem(StringBuilder sb, string item);
}


internal class HtmlListStrategy : IListStrategy
{
    public void AddListItem(StringBuilder sb, string item)
    {
        sb.AppendLine($"    <li>{item}</li>");
    }

    public void End(StringBuilder sb)
    {
        sb.AppendLine("<ul>");
    }

    public void Start(StringBuilder sb)
    {
        sb.AppendLine("</ul>");
    }
}

internal class MarkdownListStrategy : IListStrategy
{
    public void AddListItem(StringBuilder sb, string item)
    {
        sb.AppendLine($" * {item}");
    }

    public void End(StringBuilder sb)
    {
    }

    public void Start(StringBuilder sb)
    {
    }
}

public class TextProcessor
{
    private StringBuilder sb = new StringBuilder();
    private IListStrategy listStrategy;

    public void SetOutputFormat(OutputFormat outputFormat)
    {
        listStrategy = outputFormat switch
        {
            OutputFormat.Markdown => new MarkdownListStrategy(),
            OutputFormat.Html => new HtmlListStrategy(),
            _ => throw new System.Exception($"Format {outputFormat} is not supported"),
        };
    }

    public void AppendList(IEnumerable<string> items)
    {
        listStrategy.Start(sb);
        foreach (string item in items)
            listStrategy.AddListItem(sb, item);
        listStrategy.End(sb);
    }

    public override string ToString()
    {
        return sb.ToString();
    }

    public void Clear()
    {
        sb.Clear();
    }
}

public static class OutputFormatExample
{
    public static void Execute()
    {
        var tp = new TextProcessor();
        tp.SetOutputFormat(OutputFormat.Markdown);
        tp.AppendList(new[] { "foo", "bar", "baz" });
        Console.Write(tp.ToString());

        tp.Clear();

        tp.SetOutputFormat(OutputFormat.Html);
        tp.AppendList(new[] { "foo", "bar", "baz" });
        Console.Write(tp.ToString());

    }
}
