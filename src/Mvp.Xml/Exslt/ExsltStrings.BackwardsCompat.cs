namespace Mvp.Xml.Exslt;

public partial class ExsltStrings
{
    public string encodeUri(string str, bool encodeReserved) => EncodeUri(str, encodeReserved);

    public string encodeUri(string str, bool encodeReserved, string encoding) => EncodeUri(str, encodeReserved, encoding);

    public string decodeUri(string str) => DecodeUri(str);

    public string decodeUri(string str, string encoding) => DecodeUri(str, encoding);
}