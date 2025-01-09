using Domain.Organizations;
using Domain.Primitives;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Domain.Articles
{
    public class Article : Entity
    {
        [NonSerialized]
        private string _articleNumber = string.Empty;
        [NonSerialized]
        private string _name = string.Empty;
        [NonSerialized]
        private bool _isActive;
        [NonSerialized]
        private Guid? _supplierId;
        [NonSerialized]
        private Supplier _supplier;


        [XmlAttribute("ArticleNumber")]
        [DataMember]
        public string ArticleNumber
        {
            get => _articleNumber;
            set
            {
                if (_articleNumber != value)
                {
                    _articleNumber = value;
                }
            }
        }

        [XmlAttribute("Name")]
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                }
            }
        }

        [XmlAttribute("IsActive")]
        [DataMember]
        [XmlIgnore]
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                }
            }
        }

        [XmlElement("SupplierId")]
        [DataMember]
        public Guid? SupplierId
        {
            get => _supplierId;
            set
            {
                if (_supplierId != value)
                {
                    _supplierId = value;
                }
            }
        }

        [XmlElement("Supplier")]
        [DataMember]
        [XmlIgnore]
        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier ParentSupplier
        {
            get => _supplier;
            set
            {
                if (_supplier != value)
                {
                    _supplier = value;
                }
            }
        }
    }
}
