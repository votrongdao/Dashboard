using DashboardSite.Core;
using System;
using DashboardSite.Core.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DashboardSite.Model
{
    //[XmlRoot(ElementName = "Comment")
    [Serializable]
    public class CommentModel
    {
        [XmlAttribute]
        public string Username { get; set; }
        [XmlAttribute]
        public string UserRole { get; set; }
        [XmlAttribute]
        public bool IsCommentPublic { get; set; }
        [XmlIgnore]
        public int? PropertyId { get; set; }

        protected Guid m_NodeId;
        [XmlAttribute]
        public Guid NodeId
        {
            get
            { return m_NodeId; }
            set { m_NodeId = value; }
        }
        [XmlIgnore]
        public string FHANumber { get; set; }

        /// <summary>
        /// this should be displayed in user preferred time
        /// </summary>
        [XmlAttribute]
        public DateTime CommentUtcTime { get; set; }

        /// <summary>
        /// serialized xml field, don't re-serialize comments
        /// </summary>
        [XmlIgnore]
        public string Comments { get; set; }

        public string Comment { get; set; }
        public string Subject { get; set; }

        public List<CommentModel> ChildComments { get; set; }

        public CommentModel() : this(UserPrincipal.Current)
        {            
        } 

        public CommentModel(IUserPrincipal userPrincipal)
        {
            CommentUtcTime = DateTime.UtcNow;
            ChildComments = new List<CommentModel>();
            Username = userPrincipal.UserName;
            UserRole = userPrincipal.UserRole;
            m_NodeId = Guid.NewGuid();
        }
    }
}
