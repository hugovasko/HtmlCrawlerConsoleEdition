using System.Text.RegularExpressions;

namespace HtmlCrawlerConsoleEdition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string htmlDocPath = @"C:\Users\hugov\source\repos\HtmlCrawlerConsoleEdition\resources\htmlTemplate.html";
            string htmlDoc = File.ReadAllText(htmlDocPath);

            var builder = new HtmlTreeBuilder();
            var htmlTree = builder.BuildTree(htmlDoc);

            builder.DisplayTree(htmlTree);
        }
    }

    class HtmlTreeNode
    {
        public string Tag { get; set; }
        public List<HtmlTreeNode> Children { get; set; }

        public HtmlTreeNode(string tag)
        {
            Tag = tag;
            Children = new List<HtmlTreeNode>();
        }
    }

    class HtmlTreeBuilder
    {
        private HtmlTreeNode root;
        private Stack<HtmlTreeNode> nodeStack;

        public HtmlTreeBuilder()
        {
            nodeStack = new Stack<HtmlTreeNode>();
        }

        public HtmlTreeNode BuildTree(string html)
        {
            root = null;
            ParseHtml(html);
            return root;
        }

        public void DisplayTree(HtmlTreeNode node)
        {
            DisplayTree(node, 0);
        }

        private void DisplayTree(HtmlTreeNode node, int depth)
        {
            if (node == null)
                return;

            // Extract the tag name without attributes for closing tag
            var tagName = node.Tag.Split(' ')[0];

            Console.WriteLine($"{new string(' ', depth * 2)}<{node.Tag}>");

            foreach (var child in node.Children)
            {
                DisplayTree(child, depth + 1);
            }

            if (!IsSelfClosingTag(tagName))
            {
                Console.WriteLine($"{new string(' ', depth * 2)}</{tagName}>");
            }
        }

        private void ParseHtml(string html)
        {
            var tagRegex = new Regex("<(?<tag>[^>]+)>");
            var matches = tagRegex.Matches(html);

            foreach (Match match in matches)
            {
                string tag = match.Groups["tag"].Value;
                HandleTag(tag);
            }
        }

        private void HandleTag(string tag)
        {
            if (tag.StartsWith("/"))
            {
                nodeStack.Pop();
            }
            else
            {
                var node = new HtmlTreeNode(tag);
                if (root == null)
                {
                    root = node;
                    nodeStack.Push(root);
                }
                else
                {
                    HtmlTreeNode current = nodeStack.Peek();
                    current.Children.Add(node);

                    if (!tag.EndsWith("/") && !IsSelfClosingTag(tag))
                    {
                        nodeStack.Push(node);
                    }
                }
            }
        }
        private bool IsSelfClosingTag(string tag)
        {
            // List of self-closing tags
            var selfClosingTags = new List<string> { "area", "base", "br", "col", "command", "embed", "hr", "img", "input", "keygen", "link", "meta", "param", "source", "track", "wbr" };

            // Extract the tag name without attributes
            var tagName = tag.Split(' ')[0];

            Console.WriteLine($"Is {tagName} self-closing? {selfClosingTags.Contains(tagName)} the tag is {tag}");

            return selfClosingTags.Contains(tagName);
        }
        /*private bool IsSelfClosingTag(string tag)
        {
            // Check if the tag ends with "/>"
            return tag.EndsWith("/");
        }*/


    }
}
