using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


//http://localhost:1031/
namespace AgendaWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IAgendaWs
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        /* http://localhost:1031/Service1.svc/getTodosEstados */
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "getTodosEstados")]
        List<wsEstado> GetTodosEstados();

        /* http://localhost:1031/Service1.svc/getEstadoPorID/{estadoID} */
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "getEstadoPorID/{estadoID}")]
        wsEstado GetEstadoPorID(string estadoID);

        /* http://localhost:1031/Service1.svc/getEstadoPorSigla/{estadoSigla} */
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "getEstadoPorSigla/{estadoSigla}")]
        wsEstado GetEstadoPorSigla(string estadoSigla);

        /* http://localhost:1031/Service1.svc/getTodosContatos */
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "getTodosContatos")]
        List<wsContato> GetTodosContatos();

        /* http://localhost:1031/Service1.svc/getContatoPorID/{contatoID} */
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "getContatoPorID/{contatoID}")]
        wsContato GetContatoPorID(string contatoID);

        /* http://localhost:1031/Service1.svc/alterarContato */
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "alterarContato")]
        int AlterarContato(Stream JSONdataStream);

        /* http://localhost:1031/Service1.svc/removerContato/{contatoID} */
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "removerContato/{contatoID}")]
        int RemoverContato(string contatoID);

        /* http://localhost:1031/Service1.svc/criarContato */
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "criarContato")]
        int CriarContato(Stream JSONdataStream);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
