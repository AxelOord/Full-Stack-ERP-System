using Domain.Primitives;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Domain.Organizations
{
    public class Supplier : Entity
    {
        [NonSerialized]
        private string _name = string.Empty;
        [NonSerialized]
        private int _sbnId;
        [NonSerialized]
        private bool _isActive;


        [XmlAttribute("SbnId")]
        [DataMember]
        public int SbnId
        {
            get => _sbnId;
            set
            {
                if (_sbnId != value)
                {
                    _sbnId = value;
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
    }
}
