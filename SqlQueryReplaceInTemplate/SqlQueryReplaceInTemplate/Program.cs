using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System;
using System.IO;
using DinkToPdf;
class Program
{
    static void Main()
    {
        // Read HTML template from file
        string templateFilePath = "template.html";
        string htmlTemplate = ReadFileContent(templateFilePath);
        var sqlQueries = ExtractSqlQueries(htmlTemplate);

        // Print the extracted SQL queries
        Console.WriteLine("Extracted SQL Queries:");
        string[] results=new string[sqlQueries.Length];
        int i=0;
        foreach (var query in sqlQueries)
        {
            results[i] = ExecuteSqlQuery(query);
            htmlTemplate = ReplacePlaceholders(htmlTemplate, query, results[i]);

            i++;
        }


        string pdfFilePath = "output.pdf";
        GeneratePdfFromHtml(htmlTemplate, pdfFilePath);

        Console.WriteLine($"PDF generated successfully at: {pdfFilePath}");
    
    // Print or use the final HTML content
    Console.WriteLine("Generated HTML:");
        Console.WriteLine(htmlTemplate);
    }

    static void GeneratePdfFromHtml(string html, string pdfFilePath)
    {
        var converter = new BasicConverter(new PdfTools());

        var doc = new HtmlToPdfDocument
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
            },
            Objects = {
                new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = html,
                }
            }
        };

        var pdfBytes = converter.Convert(doc);
        File.WriteAllBytes(pdfFilePath, pdfBytes);
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
    // Replace SQL query placeholders with results
   
    }

static string ReadFileContent(string filePath)
    {
        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
            return string.Empty;
        }
    }

    static string ExecuteSqlQuery(string sqlQuery)
    {
        // Replace this connection string with your database connection string
        string databasePath = "d:\\mini_projet\\database.db";
        string connectionString = $"Data Source={databasePath};";
       

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            using (var command = new SQLiteCommand(sqlQuery, connection))
            {
                var result = command.ExecuteScalar();
                return result?.ToString() ?? string.Empty;
            }
        }
    }

    static string ReplacePlaceholders(string template, params string[] keyValues)
    {
        if (keyValues.Length % 2 != 0)
        {
            throw new ArgumentException("Key-value pairs are not balanced.");
        }

        for (int i = 0; i < keyValues.Length; i += 2)
        {
            string key = keyValues[i];
            string value = keyValues[i + 1];

            // Replace placeholders in the template with actual values
            template = template.Replace("{{SQL: " + key + "}}", value);
        }

        return template;
    }
}
