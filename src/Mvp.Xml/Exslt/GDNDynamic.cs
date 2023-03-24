using System.Text.RegularExpressions;
using System.Xml.XPath;

namespace Mvp.Xml.Exslt;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
///   This class implements additional functions in the 
///   http://gotdotnet.com/exslt/dynamic namespace.
/// </summary>		
public class GdnDynamic
{
    /// <summary>
    /// Implements the following function 
    ///    object dyn2:evaluate(node-set, string, string?) 
    /// </summary>
    /// <param name="contextNode">Context node</param>
    /// <param name="expression">Expression to evaluate</param>
    /// <returns>Result of evaluating given Xpath expression WRT to context node.</returns>
    /// <remarks>THIS FUNCTION IS NOT PART OF EXSLT!!!</remarks>    
    public object Evaluate(XPathNodeIterator contextNode, string expression) => Evaluate(contextNode, expression, "");

    public object evaluate(XPathNodeIterator contextNode, string expression) => Evaluate(contextNode, expression);

    /// <summary>
    /// Implements the following function 
    ///    object dyn2:evaluate(node-set, string, string?) 
    /// </summary>
    /// <param name="contextNode">Context node</param>
    /// <param name="expression">Expression to evaluate</param>
    /// <param name="namespaces">Namespace bindings</param>
    /// <returns>Result of evaluating given Xpath expression WRT to context node.</returns>
    /// <remarks>THIS FUNCTION IS NOT PART OF EXSLT!!!</remarks>
    public object Evaluate(XPathNodeIterator contextNode, string expression, string namespaces)
    {
        if (expression == string.Empty || contextNode == null)
            return string.Empty;

        if (contextNode.MoveNext())
        {
            try
            {
                var expr = contextNode.Current.Compile(expression);
                var context = new ExsltContext(contextNode.Current.NameTable);
                var node = contextNode.Current.Clone();
                if (node.NodeType != XPathNodeType.Element)
                    node.MoveToParent();

                if (node.MoveToFirstNamespace())
                {
                    do
                    {
                        context.AddNamespace(node.Name, node.Value);
                    } while (node.MoveToNextNamespace());
                }
                if (namespaces != string.Empty)
                {
                    try
                    {
                        var regexp = new Regex(@"xmlns:(?<p>\w+)\s*=\s*(('(?<n>.+)')|(""(?<n>.+)""))\s*");
                        var m = regexp.Match(namespaces);
                        while (m.Success)
                        {
                            try
                            {
                                context.AddNamespace(m.Groups["p"].Value,
                                    m.Groups["n"].Value);
                            }
                            catch { }
                            m = m.NextMatch();
                        }
                    }
                    catch { }
                }
                expr.SetContext(context);
                return contextNode.Current.Evaluate(expr, contextNode);
            }
            catch
            {
                //Any exception such as syntax error in XPath
                return string.Empty;
            }
        }
        //Empty nodeset as context node
        return string.Empty;
    }

    public object evaluate(XPathNodeIterator contextNode, string expression, string namespaces) => Evaluate(contextNode, expression, namespaces);
}
