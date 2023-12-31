using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace HtmlCrawlerConsoleEdition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"C:\Users\hugov\source\repos\HtmlCrawlerConsoleEdition\resources\simpleHtml.html";
            HtmlParser parser = new HtmlParser();
            Node tree = parser.Parse(filePath);

            // Print the tree structure
            // Console.WriteLine(tree);

            // Print the path
            string input = Console.ReadLine();
            if (input.StartsWith("PRINT"))
            {
                string path = input.Substring(6);
                PrintPath(tree, path);
            }
        }

        static public void PrintPath(Node node, string path)
        {
            string[] parts = path.Split('/');
            PrintPath(node, parts, 0);
        }

        private static void PrintPath(Node node, string[] parts, int index)
        {
            if (node == null || index >= parts.Length)
            {
                return;
            }

            if (node.Tag.Equals(parts[index], StringComparison.OrdinalIgnoreCase) || parts[index] == "")
            {
                Console.WriteLine(node);

                foreach (var childNode in node.ChildNodes)
                {
                    PrintPath(childNode, parts, index + (node.Tag.Equals(parts[index], StringComparison.OrdinalIgnoreCase) ? 1 : 0));
                }
            }

            return;
        }
        public class Node
        {
            public string Tag;
            public string Attributes;
            public bool IsSelfClosing;
            public string Content;
            public List<Node> ChildNodes;

            public Node(string tag, string attributes = "", bool isSelfClosing = false)
            {
                Tag = tag;
                Attributes = attributes;
                IsSelfClosing = isSelfClosing;
                Content = "";
                ChildNodes = new List<Node>();
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                PrintNode(sb, this, 0);
                return sb.ToString();
            }

            private void PrintNode(StringBuilder sb, Node node, int depth)
            {
                string attributes = string.IsNullOrEmpty(node.Attributes) ? "" : $" {node.Attributes}";
                sb.AppendLine($"{new string(' ', depth * 2)}<{node.Tag}{attributes}{(node.IsSelfClosing ? "/" : "")}>");

                if (!string.IsNullOrEmpty(node.Content))
                {
                    sb.AppendLine($"{new string(' ', (depth + 1) * 2)}{node.Content}");
                }

                foreach (var childNode in node.ChildNodes)
                {
                    PrintNode(sb, childNode, depth + 1);
                }

                if (!node.IsSelfClosing)
                {
                    sb.AppendLine($"{new string(' ', depth * 2)}</{node.Tag}>");
                }
            }
        }

        public class HtmlParser
        {
            public Node Parse(string filePath)
            {
                Node root = null;
                Stack<Node> openTags = new Stack<Node>();
                Node currentNode = null;

                try
                {
                    string html = File.ReadAllText(filePath);
                    HtmlTokenizer tokenizer = new HtmlTokenizer(html);

                    string token;
                    while ((token = tokenizer.NextToken()) != null)
                    {
                        if (token.StartsWith("<"))
                        {
                            if (token.StartsWith("</"))
                            {
                                // Closing tag
                                string tagName = token.Substring(2, token.Length - 3);
                                if (currentNode.Tag == tagName)
                                {
                                    currentNode = openTags.Count > 0 ? openTags.Pop() : null;
                                }
                                else
                                {
                                    Console.WriteLine("currentNode.Tag: " + currentNode.Tag);
                                    Console.WriteLine("tagName: " + tagName);
                                    throw new Exception("Invalid HTML structure");
                                }
                            }
                            else
                            {
                                // Opening or self-closing tag
                                bool isSelfClosing = token.EndsWith("/>");
                                string tagName = token.Substring(1, token.Length - (isSelfClosing ? 3 : 2));
                                string attributes = "";
                                int spaceIndex = tagName.IndexOf(" ");
                                if (spaceIndex > -1)
                                {
                                    attributes = tagName.Substring(spaceIndex + 1);
                                    tagName = tagName.Substring(0, spaceIndex);
                                }

                                Node node = new Node(tagName, attributes, isSelfClosing);

                                if (currentNode == null)
                                {
                                    root = node;
                                }
                                else
                                {
                                    currentNode.ChildNodes.Add(node);
                                }

                                if (!isSelfClosing)
                                {
                                    openTags.Push(currentNode);
                                    currentNode = node;
                                }
                            }
                        }
                        else
                        {
                            // Content
                            if (currentNode != null)
                            {
                                currentNode.Content += token;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error parsing HTML file: " + e.Message);
                }

                return root;
            }
        }

        public class HtmlTokenizer
        {
            private readonly string _html;
            private int _position;

            public HtmlTokenizer(string html)
            {
                _html = html;
                _position = 0;
            }

            public string NextToken()
            {
                if (_position >= _html.Length)
                {
                    return null;
                }

                if (_html[_position] == '<')
                {
                    int end = _html.IndexOf('>', _position);
                    if (end == -1)
                    {
                        throw new Exception("Invalid HTML structure");
                    }

                    string token = _html.Substring(_position, end - _position + 1);
                    _position = end + 1;
                    return token.Trim();
                }
                else
                {
                    int start = _position;
                    while (_position < _html.Length && _html[_position] != '<')
                    {
                        _position++;
                    }

                    return _html.Substring(start, _position - start).Trim();
                }
            }
        }
    }

}
