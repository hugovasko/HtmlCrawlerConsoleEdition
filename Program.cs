using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HtmlCrawlerConsoleEdition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Make a program that extracts content from an HTML file.
            // The program must have a console interface.
            // The program must get its parameters from the console.
            // It must allow searching and modifying parts of the HTML document
            // Building a tree model of a user-submitted document
            // Note that some tags may not have a corresponding closing tag (e.g. <img>).
            // If there is an error in the document, the program should report it.

            string htmlDocumentPath = @"C:\Users\hugov\source\repos\HtmlCrawlerConsoleEdition\resources\htmlTemplate.html";
            string htmlDocument = File.ReadAllText(htmlDocumentPath);

            HtmlDocument htmlDoc = new HtmlDocument(htmlDocument);

            htmlDoc.PrintDocument();
        }
    }

    public class HtmlDocument
    {
        public HtmlDocument(string htmlDocument)
        {
            this.HtmlDocumentString = htmlDocument;
            this.HtmlDocumentTree = new HtmlDocumentTree(htmlDocument);
        }

        public string HtmlDocumentString { get; set; }
        public HtmlDocumentTree HtmlDocumentTree { get; set; }

        public void PrintDocument()
        {
            Console.WriteLine(this.HtmlDocumentString);
        }
    }

    public class HtmlDocumentTree
    {
        public HtmlDocumentTree(string htmlDocument)
        {
            this.HtmlDocument = htmlDocument;
            this.HtmlDocumentTreeNodes = new List<HtmlDocumentTreeNode>();
            this.BuildTree();
        }

        public string HtmlDocument { get; set; }
        public List<HtmlDocumentTreeNode> HtmlDocumentTreeNodes { get; set; }

        public void BuildTree()
        {

        }
    }

    public class HtmlDocumentTreeNode
    {
        public HtmlDocumentTreeNode(string htmlDocument)
        {
            this.HtmlDocument = htmlDocument;
            this.HtmlDocumentTreeNodes = new List<HtmlDocumentTreeNode>();
        }

        public string HtmlDocument { get; set; }
        public List<HtmlDocumentTreeNode> HtmlDocumentTreeNodes { get; set; }
    }

}
