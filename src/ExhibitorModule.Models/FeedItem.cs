/* 
    Licensed under the Apache License, Version 2.0
    
    http://www.apache.org/licenses/LICENSE-2.0
    */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ExhibitorModule.Models
{
    [XmlRoot(ElementName="item")]
    public class FeedItem
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }
        [XmlElement(ElementName="title")]
        public string Title { get; set; }
        [XmlElement(ElementName="link")]
        public string Link { get; set; }
        [XmlElement(ElementName="comments")]
        public List<string> Comments { get; set; }
        [XmlElement(ElementName="pubDate")]
        public DateTime PublicationDate { get; set; }
        [XmlElement(ElementName="creator", Namespace="http://purl.org/dc/elements/1.1/")]
        public string Creator { get; set; }
        [XmlElement(ElementName="description")]
        public string Description { get; set; }
        [XmlElement(ElementName="commentRss", Namespace="http://wellformedweb.org/CommentAPI/")]
        public string CommentRss { get; set; }
        [XmlElement(ElementName = "category")]
        public string Category { get; set; }
    }
}
