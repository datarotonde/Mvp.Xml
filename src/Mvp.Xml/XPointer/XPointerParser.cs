using System.Collections.Generic;

namespace Mvp.Xml.XPointer;

/// <summary>
/// XPointer parser.
/// </summary>
class XPointerParser
{
    static readonly IDictionary<string, XPointerSchema.SchemaType> schemas = XPointerSchema.Schemas;

    static XPointerSchema.SchemaType GetSchema(XPointerLexer lexer, IList<PointerPart> parts)
    {
        string schemaNsuri;
        if (lexer.Prefix != string.Empty)
        {
            schemaNsuri = null;
            //resolve prefix
            for (var i = parts.Count - 1; i >= 0; i--)
            {
                var part = parts[i];
                if (part is XmlnsSchemaPointerPart xmlnsPart &&
                    xmlnsPart.Prefix == lexer.Prefix)
                {
                    schemaNsuri = xmlnsPart.Uri;
                    break;
                }
            }
            if (schemaNsuri == null)
            //No binding for the prefix - ignore pointer part
            {
                return XPointerSchema.SchemaType.Unknown;
            }
        }
        else
        {
            schemaNsuri = string.Empty;
        }

        var schemaQName = schemaNsuri + ':' + lexer.NcName;
        return schemas.ContainsKey(schemaQName) ? schemas[schemaQName] : XPointerSchema.SchemaType.Unknown;
    }


    public static Pointer ParseXPointer(string xpointer)
    {
        var lexer = new XPointerLexer(xpointer);
        lexer.NextLexeme();
        if (lexer.Kind == XPointerLexer.LexKind.NcName && !lexer.CanBeSchemaName)
        {
            //Shorthand pointer
            Pointer ptr = new ShorthandPointer(lexer.NcName);
            lexer.NextLexeme();
            if (lexer.Kind != XPointerLexer.LexKind.Eof)
                throw new XPointerSyntaxException(Properties.Resources.InvalidTokenAfterShorthandPointer);

            return ptr;
        }

        //SchemaBased pointer
        IList<PointerPart> parts = new List<PointerPart>();
        while (lexer.Kind != XPointerLexer.LexKind.Eof)
        {
            if ((lexer.Kind == XPointerLexer.LexKind.NcName ||
                 lexer.Kind == XPointerLexer.LexKind.QName) &&
                lexer.CanBeSchemaName)
            {
                var schemaType = GetSchema(lexer, parts);
                //Move to '('
                lexer.NextLexeme();
                switch (schemaType)
                {
                    case XPointerSchema.SchemaType.Element:
                        var elemPart = ElementSchemaPointerPart.ParseSchemaData(lexer);
                        if (elemPart != null)
                        {
                            parts.Add(elemPart);
                        }

                        break;
                    case XPointerSchema.SchemaType.Xmlns:
                        var xmlnsPart = XmlnsSchemaPointerPart.ParseSchemaData(lexer);
                        if (xmlnsPart != null)
                        {
                            parts.Add(xmlnsPart);
                        }

                        break;
                    case XPointerSchema.SchemaType.XPath1:
                        var xpath1Part = XPath1SchemaPointerPart.ParseSchemaData(lexer);
                        if (xpath1Part != null)
                        {
                            parts.Add(xpath1Part);
                        }

                        break;
                    case XPointerSchema.SchemaType.XPointer:
                        var xpointerPart = XPointerSchemaPointerPart.ParseSchemaData(lexer);
                        if (xpointerPart != null)
                        {
                            parts.Add(xpointerPart);
                        }

                        break;
                    default:
                        //Unknown scheme
                        lexer.ParseEscapedData();
                        break;
                }
                //Skip ')'
                lexer.NextLexeme();
                //Skip possible whitespace
                if (lexer.Kind == XPointerLexer.LexKind.Space)
                {
                    lexer.NextLexeme();
                }
            }
            else
            {
                throw new XPointerSyntaxException(Properties.Resources.InvalidToken);
            }
        }
        return new SchemaBasedPointer(parts, xpointer);
    }
}
