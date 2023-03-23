using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;

namespace Mvp.Xml.Exslt;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Implements the functions in the http://exslt.org/strings namespace 
/// </summary>
public class ExsltStrings
{
    static readonly char[] hexdigit = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

    /// <summary>
    /// Implements the following function 
    ///		node-set tokenize(string, string)
    /// </summary>
    /// <param name="str"></param>
    /// <param name="delimiters"></param>				
    /// <returns>This function breaks the input string into a sequence of strings, 
    /// treating any character in the list of delimiters as a separator. 
    /// The separators themselves are not returned. 
    /// The tokens are returned as a set of 'token' elements.</returns>
    public XPathNodeIterator Tokenize(string str, string delimiters)
    {

        var doc = new XmlDocument();
        doc.LoadXml("<tokens/>");

        if (delimiters == string.Empty)
        {
            foreach (var c in str)
            {
                var elem = doc.CreateElement("token");
                elem.InnerText = c.ToString();
                doc.DocumentElement.AppendChild(elem);
            }
        }
        else
        {
            foreach (var token in str.Split(delimiters.ToCharArray()))
            {

                var elem = doc.CreateElement("token");
                elem.InnerText = token;
                doc.DocumentElement.AppendChild(elem);
            }
        }

        return doc.CreateNavigator().Select("//token");
    }

    public XPathNodeIterator tokenize(string str, string delimiters) => Tokenize(str, delimiters);


    /// <summary>
    /// Implements the following function 
    ///		node-set tokenize(string)
    /// </summary>
    /// <param name="str"></param>		
    /// <returns>This function breaks the input string into a sequence of strings, 
    /// using the whitespace characters as a delimiter. 
    /// The separators themselves are not returned. 
    /// The tokens are returned as a set of 'token' elements.</returns>
    public XPathNodeIterator Tokenize(string str)
    {
        var regex = new Regex("\\s+");
        var doc = new XmlDocument();
        doc.LoadXml("<tokens/>");

        foreach (var token in regex.Split(str))
        {
            var elem = doc.CreateElement("token");
            elem.InnerText = token;
            doc.DocumentElement.AppendChild(elem);
        }

        return doc.CreateNavigator().Select("//token");
    }

    public XPathNodeIterator tokenize(string str) => Tokenize(str);

    /// <summary>
    /// Implements the following function 
    ///		string replace(string, string, string) 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    /// <returns></returns>
    /// <remarks>This function has completely diffeerent semantics from the EXSLT function. 
    /// The description of the EXSLT function is confusing and furthermore no one has implemented
    /// the described semantics which implies that others find the method problematic. Instead
    /// this function is straightforward, it replaces all occurrences of oldValue with 
    /// newValue</remarks>
    public string Replace(string str, string oldValue, string newValue) => str.Replace(oldValue, newValue);

    public string replace(string str, string oldValue, string newValue) => Replace(str, oldValue, newValue);

    /// <summary>
    /// Implements the following function 
    ///		string padding(number)
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public string Padding(int number)
    {
        var s = string.Empty;
        if (number < 0)
            return s;

        return s.PadLeft(number);
    }

    public string padding(int number) => Padding(number);

    /// <summary>
    /// Implements the following function 
    ///		string padding(number, string)
    /// </summary>		
    public string Padding(int number, string s)
    {
        if (number < 0 || s == string.Empty)
            return string.Empty;

        var sb = new StringBuilder(s);
        while (sb.Length < number)
        {
            sb.Append(s);
        }

        if (sb.Length > number)
            return sb.Remove(number, sb.Length - number).ToString();
        else
            return sb.ToString();
    }

    public string padding(int number, string s) => Padding(number, s);

    /// <summary>
    /// Implements the following function 
    ///		node-set split(string)
    /// </summary>
    /// <param name="str"></param>
    /// <remarks>This function breaks the input string into a sequence of strings, 
    /// using the space character as a delimiter. 
    /// The space character itself is never returned not even when there are 
    /// adjacent space characters. 
    /// </remarks>
    /// <returns>The tokens are returned as a set of 'token' elements</returns>
    public XPathNodeIterator Split(string str)
    {
        var doc = new XmlDocument();
        doc.LoadXml("<tokens/>");

        foreach (var match in str.Split(new char[] { ' ' }))
        {
            if (!match.Equals(string.Empty))
            {
                var elem = doc.CreateElement("token");
                elem.InnerText = match;
                doc.DocumentElement.AppendChild(elem);
            }
        }

        return doc.CreateNavigator().Select("//token");

    }

    public XPathNodeIterator split(string str) => Split(str);

    /// <summary>
    /// Implements the following function 
    ///		node-set split(string, string)
    /// </summary>
    /// <param name="str"></param>
    /// <param name="delimiter"></param>
    /// <remarks>This function breaks the input string into a sequence of strings, 
    /// using the space character as a delimiter. 
    /// The space character itself is never returned not even when there are 
    /// adjacent space characters. 
    /// </remarks>
    /// <returns>The tokens are returned as a set of 'token' elements</returns>
    public XPathNodeIterator Split(string str, string delimiter)
    {
        var doc = new XmlDocument();
        doc.LoadXml("<tokens/>");

        if (delimiter.Equals(string.Empty))
        {
            foreach (var match in str)
            {
                var elem = doc.CreateElement("token");
                elem.InnerText = match.ToString();
                doc.DocumentElement.AppendChild(elem);
            }
        }
        else
        {
            //since there is no String.Split(string) method we use the Regex class 
            //and escape special characters. 
            //. $ ^ { [ ( | ) * + ? \
            delimiter = delimiter.Replace("\\", "\\\\").Replace("$", "\\$").Replace("^", "\\^");
            delimiter = delimiter.Replace("{", "\\{").Replace("[", "\\[").Replace("(", "\\(");
            delimiter = delimiter.Replace("*", "\\*").Replace(")", "\\)").Replace("|", "\\|");
            delimiter = delimiter.Replace("+", @"\+").Replace("?", "\\?").Replace(".", "\\.");

            var regex = new Regex(delimiter);

            foreach (var match in regex.Split(str))
            {
                if ((!match.Equals(string.Empty)) && (!match.Equals(delimiter)))
                {
                    var elem = doc.CreateElement("token");
                    elem.InnerText = match;
                    doc.DocumentElement.AppendChild(elem);
                }
            }
        }

        return doc.CreateNavigator().Select("//token");
    }

    public XPathNodeIterator split(string str, string delimiter) => Split(str, delimiter);

    /// <summary>
    /// Implements the following function 
    ///		string concat(node-set)
    /// </summary>
    /// <param name="nodeset"></param>
    /// <returns></returns>
    public string Concat(XPathNodeIterator nodeset)
    {
        var sb = new StringBuilder();
        while (nodeset.MoveNext())
        {
            sb.Append(nodeset.Current.Value);
        }

        return sb.ToString();
    }

    public string concat(XPathNodeIterator nodeset) => Concat(nodeset);

    /// <summary>
    /// Implements the following function
    ///     string str:align(string, string, string)
    /// </summary>
    /// <param name="str">String to align</param>
    /// <param name="padding">String, within which to align</param>
    /// <param name="alignment">left/right/center</param>
    /// <returns>Aligned string.</returns>
    public string Align(string str, string padding, string alignment)
    {
        if (str.Length > padding.Length)
            return str.Substring(0, padding.Length);

        if (str.Length == padding.Length)
            return str;

        switch (alignment)
        {
            case "right":
                return padding.Substring(0, padding.Length - str.Length) + str;
            case "center":
                var space = (padding.Length - str.Length) / 2;
                return padding.Substring(0, space) + str +
                        padding.Substring(str.Length + space);
            default:
                //Align to left by default
                return str + padding.Substring(str.Length);
        }
    }

    public string align(string str, string padding, string alignment) => Align(str, padding, alignment);

    /// <summary>
    /// Implements the following function
    ///     string str:align(string, string)
    /// </summary>
    /// <param name="str">String to align</param>
    /// <param name="padding">String, within which to align</param>
    /// <returns>Aligned to left string.</returns>
    public string Align(string str, string padding) => Align(str, padding, "left");

    public string align(string str, string padding) => Align(str, padding);

    /// <summary>
    /// Implements the following function
    ///      string str:encode-uri(string, string)
    /// </summary>
    /// <param name="str">String to encode</param>
    /// <param name="encodeReserved">If true, will encode even the [RFC 2396] 
    /// and [RFC 2732] "reserved characters".</param>
    /// <returns>The encoded string</returns>
    public string EncodeUri(string str, bool encodeReserved) => EncodeUriImpl(str, encodeReserved, Encoding.UTF8);

    /// <summary>
    /// This wrapper method will be renamed during custom build
    /// to provide conformant EXSLT function name.
    /// </summary>    
    public string encodeUri_RENAME_ME(string str, bool encodeReserved) => EncodeUri(str, encodeReserved);

    /// <summary>
    /// Implements the following function
    ///      string str:encode-uri(string, string, string)
    /// </summary>
    /// <param name="str">String to encode</param>
    /// <param name="encodeReserved">If true, will encode even the 
    /// [RFC 2396] and [RFC 2732] "reserved characters"</param>
    /// <param name="encoding">A character encoding to use</param>
    /// <returns>The encoded string</returns>
    public string EncodeUri(string str, bool encodeReserved, string encoding)
    {
        Encoding enc;
        try
        {
            enc = Encoding.GetEncoding(encoding);
        }
        catch
        {
            //Not supported encoding, return empty string
            return string.Empty;
        }
        return EncodeUriImpl(str, encodeReserved, enc);
    }

    /// <summary>
    /// This wrapper method will be renamed during custom build
    /// to provide conformant EXSLT function name.
    /// </summary>    
    public string encodeUri_RENAME_ME(string str, bool encodeReserved, string encoding) => EncodeUri(str, encodeReserved, encoding);

    /// <summary>
    /// Implements the following function
    ///      string str:encode-uri(string, string, string)
    /// </summary>
    /// <param name="str">String to encode</param>
    /// <param name="encodeReserved">If true, will encode even the 
    /// [RFC 2396] and [RFC 2732] "reserved characters"</param>
    /// <param name="enc">A character encoding to use</param>
    /// <returns>The encoded string</returns>
    string EncodeUriImpl(string str, bool encodeReserved, Encoding enc)
    {
        if (str == string.Empty)
            return str;

        var res = new StringBuilder(str.Length);
        var chars = str.ToCharArray();
        if (encodeReserved)
        {
            for (var i = 0; i < chars.Length; i++)
            {
                var c = chars[i];
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
                {
                    res.Append(c);
                }
                else
                {
                    switch (c)
                    {
                        case '-':
                        case '_':
                        case '.':
                        case '!':
                        case '~':
                        case '*':
                        case '\'':
                        case '(':
                        case ')':
                            res.Append(c);
                            break;
                        case '%':
                            if (i < chars.Length - 2 && IsHexDigit(chars[i + 1]) && IsHexDigit(chars[i + 2]))
                            {
                                res.Append(c);
                            }
                            else
                            {
                                EncodeChar(res, enc, chars, i);
                            }

                            break;
                        default:
                            EncodeChar(res, enc, chars, i);
                            break;
                    }
                }
            }
        }
        else
        {
            for (var i = 0; i < chars.Length; i++)
            {
                var c = chars[i];
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
                {
                    res.Append(c);
                }
                else
                {
                    switch (c)
                    {
                        case '-':
                        case '_':
                        case '.':
                        case '!':
                        case '~':
                        case '*':
                        case '\'':
                        case '(':
                        case ')':
                        case ';':
                        case '/':
                        case '?':
                        case ':':
                        case '@':
                        case '&':
                        case '=':
                        case '+':
                        case '$':
                        case ',':
                        case '[':
                        case ']':
                            res.Append(c);
                            break;
                        case '%':
                            if (i < chars.Length - 2 && IsHexDigit(chars[i + 1]) && IsHexDigit(chars[i + 2]))
                            {
                                res.Append(c);
                            }
                            else
                            {
                                EncodeChar(res, enc, chars, i);
                            }

                            break;
                        default:
                            EncodeChar(res, enc, chars, i);
                            break;
                    }
                }
            }

        }
        return res.ToString();
    }

    void EncodeChar(StringBuilder res, Encoding enc, char[] str, int index)
    {
        foreach (var b in enc.GetBytes(str, index, 1))
        {
            res.AppendFormat("%{0}{1}", hexdigit[b >> 4], hexdigit[b & 15]);
        }
    }

    bool IsHexDigit(char c) => (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');

    /// <summary>
    /// Implements the following function
    ///      string str:decode-uri(string)
    /// </summary>
    /// <param name="str">String to decode</param>        
    /// <returns>The decoded string</returns>
    public string DecodeUri(string str) => DecodeUriImpl(str, Encoding.UTF8);

    /// <summary>
    /// This wrapper method will be renamed during custom build
    /// to provide conformant EXSLT function name.
    /// </summary>    
    public string decodeUri_RENAME_ME(string str) => DecodeUri(str);

    /// <summary>
    /// Implements the following function
    ///      string str:decode-uri(string, string)
    /// </summary>
    /// <param name="str">String to decode</param>        
    /// <param name="encoding">A character encoding to use</param>
    /// <returns>The decoded string</returns>
    public string DecodeUri(string str, string encoding)
    {
        if (encoding == string.Empty)
            return string.Empty;

        Encoding enc;
        try
        {
            enc = Encoding.GetEncoding(encoding);
        }
        catch
        {
            //Not supported encoding, return empty string
            return string.Empty;
        }
        return DecodeUriImpl(str, enc);
    }


    /// <summary>
    /// This wrapper method will be renamed during custom build
    /// to provide conformant EXSLT function name.
    /// </summary>    
    public string decodeUri_RENAME_ME(string str, string encoding) => DecodeUri(str, encoding);

    /// <summary>
    /// Implementation for 
    ///   string str:decode-uri(string, string)
    /// </summary>
    /// <param name="str">String to decode</param>        
    /// <param name="enc">A character encoding to use</param>
    /// <returns>The decoded string</returns>
    string DecodeUriImpl(string str, Encoding enc)
    {
        if (str == string.Empty)
            return str;

        return HttpUtility.UrlDecode(str, enc);
    }
}
