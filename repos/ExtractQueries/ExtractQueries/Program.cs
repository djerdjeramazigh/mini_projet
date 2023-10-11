using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        // HTML template with embedded SQL queries
        string htmlTemplate = @"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <title>HTML Template</title>
            </head>
            <body>
                <h1>Results</h1>
                <div>
                    <p>Query 1: {{SQL: SELECT Column1 FROM Table1}}</p>
                    <p>Query 2: {{SQL: SELECT Column2 FROM Table2}}</p>
                </div>
            </body>
            </html>";

        // Extract SQL queries from the HTML template
        var sqlQueries = ExtractSqlQueries(htmlTemplate);

        // Print the extracted SQL queries
        Console.WriteLine("Extracted SQL Queries:");
        foreach (var query in sqlQueries)
        {
            Console.WriteLine(query);
        }
    }

    static string[] ExtractSqlQueries(string html)
    {
        // Define a regular expression pattern for extracting SQL queries
        string pattern = @"\{\{SQL:\s*(.*?)\}\}";

        // Use Regex to match the pattern in the HTML
        var matches = Regex.Matches(html, pattern);

        // Extract and return the matched SQL queries
        var queries = new string[matches.Count];
        for (int i = 0; i < matches.Count; i++)
        {
            queries[i] = matches[i].Groups[1].Value;
        }

        return queries;
    }
}
