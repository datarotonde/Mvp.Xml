using System.Xml.XPath;

namespace Mvp.Xml.Exslt;

public partial class ExsltSets
{
    public XPathNodeIterator difference2(XPathNodeIterator nodeset1, XPathNodeIterator nodeset2) => Difference2(nodeset1, nodeset2);

    public XPathNodeIterator distinct2(XPathNodeIterator nodeset) => Distinct2(nodeset);

    public bool hasSameNode(XPathNodeIterator nodeset1, XPathNodeIterator nodeset2) => HasSameNode(nodeset1, nodeset2);

    public bool hasSameNode2(XPathNodeIterator nodeset1, XPathNodeIterator nodeset2) => HasSameNode2(nodeset1, nodeset2);

    public XPathNodeIterator intersection3(XPathNodeIterator nodeset1, XPathNodeIterator nodeset2) => Intersection3(nodeset1, nodeset2);
}