using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace AgendaWS
{
    [DataContract]
    [Serializable]
    public class wsContato
    {

        [DataMember]
		public int id { get; set; }
		
        [DataMember]
        public string nome { get; set; }
		
        [DataMember]
        public string endereco { get; set; }
		
        [DataMember]
        public string complemento { get; set; }
		
        [DataMember]
        public string bairro { get; set; }
		
        [DataMember]
        public string municipio { get; set; }
		
        [DataMember]
        public string cep { get; set; }

        [DataMember]
        public string telefone { get; set; }

        [DataMember]
        public int estadoid { get; set; }
		
        [DataMember]
        public wsEstado estado { get; set; }
    }
}