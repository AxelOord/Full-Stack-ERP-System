using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Domain.Primitives.Interfaces;

namespace Domain.Primitives
{
    [Serializable]
    public class Entity : IEntity, ISerializable
    {
        #region Fields
        int baseValue;

        private Guid _id = Guid.NewGuid();
        [NonSerialized]
        private DateTime _insertDate = DateTime.UtcNow;
        [NonSerialized]
        private DateTime _updateDate = DateTime.UtcNow;

        #endregion

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("baseValue", baseValue);
        }

        #region Properties

        /// <summary>
        /// Holds the Object Id
        /// </summary>
        [XmlAttribute("Id")]
        [DataMember]
        public Guid Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                }
            }
        }

        /// <summary>
        /// Holds the InsertDate
        /// </summary>
        [DataType(DataType.DateTime)]
        [XmlAttribute("InsertDate")]
        public DateTime InsertDate
        {
            get => _insertDate;
            set
            {
                if (_insertDate != value)
                {
                    _insertDate = value;
                }
            }
        }

        /// <summary>
        /// Holds the UpdateDate
        /// </summary>
        [DataType(DataType.DateTime)]
        [XmlAttribute("UpdateDate")]
        public DateTime UpdateDate
        {
            get => _updateDate;
            set
            {
                if (_updateDate != value)
                {
                    _updateDate = value;
                }
            }
        }

        #endregion
    }
}
