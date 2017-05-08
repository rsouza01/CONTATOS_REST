using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AgendaWS
{
    [DataContract]
    [Serializable]
    public class wsEstado
    {

        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string descricao { get; set; }
        [DataMember]
        public string sigla { get; set; }
    }
}