using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using AgendaWS.Database;

namespace AgendaWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class AgendaService : IAgendaWs
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }


        public List<wsEstado> GetTodosEstados()
        {
            try
            {
                AgendaDBDataContext dc = new AgendaDBDataContext();

                List<wsEstado> results = new List<wsEstado>();

                foreach (ESTADO est in dc.ESTADOs)
                {
                    results.Add(new wsEstado()
                    {
                        id = est.ID,
                        descricao = est.DESCRICAO,
                        sigla = est.SIGLA
                    });
                }
                return results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                return null;
            }
        }


        public List<wsContato> GetTodosContatos()
        {
            try
            {
                AgendaDBDataContext dc = new AgendaDBDataContext();

                List<wsContato> results = new List<wsContato>();

                foreach (CONTATO cont in dc.CONTATOs)
                {
                    results.Add(new wsContato()
                    {
		                id = cont.ID,
		                nome = cont.NOME,
		                endereco = cont.ENDERECO,
		                complemento = cont.COMPLEMENTO,
		                bairro = cont.BAIRRO,
		                municipio = cont.MUNICIPIO,
		                cep = cont.CEP,
		                telefone = cont.TELEFONE,
                        estado = UtilConversao.converterEstado(cont.ESTADO)
                    });
                }
                return results;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                return null;
            }
        }

        public wsEstado GetEstadoPorID(string estadoID)
        {
            wsEstado wsEstado = null;

            try
            {

                int nEstadoId = int.Parse(estadoID);
                
                AgendaDBDataContext dc = new AgendaDBDataContext();

                ESTADO estado = dc.ESTADOs.Where(e => e.ID == nEstadoId).FirstOrDefault();

                if (estado == null)
                {
                    return null;
                }
                else
                {
                    wsEstado = new wsEstado()
                    {
                        id = estado.ID,
                        descricao = estado.DESCRICAO,
                        sigla = estado.SIGLA
                    };
                }

                return wsEstado;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                return null;
            }
        }

        public wsEstado GetEstadoPorSigla(string estadoSigla)
        {
            wsEstado wsEstado = null;
            try
            {
                AgendaDBDataContext dc = new AgendaDBDataContext();

                ESTADO estado = dc.ESTADOs.Where(e => e.SIGLA == estadoSigla).FirstOrDefault();

                if (estado == null)
                {
                    return null;
                }
                else
                {
                    wsEstado = new wsEstado()
                    {
                        id = estado.ID,
                        descricao = estado.DESCRICAO,
                        sigla = estado.SIGLA
                    };
                }

                return wsEstado;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                return null;
            }
        }

        public wsContato GetContatoPorID(string contatoID)
        {
            wsContato wsContato = null;

            try
            {
                int nContatoId = int.Parse(contatoID);

                AgendaDBDataContext dc = new AgendaDBDataContext();

                CONTATO contato = dc.CONTATOs.Where(c => c.ID == nContatoId).FirstOrDefault();

                if (contato == null)
                {
                    return null;
                }
                else
                {
                    wsContato = new wsContato()
                    {
                        id = contato.ID,
                        nome = contato.NOME,
                        endereco = contato.ENDERECO,
                        complemento = contato.COMPLEMENTO,
                        bairro = contato.BAIRRO,
                        municipio = contato.MUNICIPIO,
                        cep = contato.CEP,
                        telefone = contato.TELEFONE,
                        estado = UtilConversao.converterEstado(contato.ESTADO)
                    };
                }

                return wsContato;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                return null;
            }

        }

        public int AlterarContato(Stream JSONdataStream)
        {
            try
            {
                // Read in our Stream into a string...
                StreamReader reader = new StreamReader(JSONdataStream);
                string JSONdata = reader.ReadToEnd();

                // ..then convert the string into a single "wsOrder" record.
                JavaScriptSerializer jss = new JavaScriptSerializer();
                wsContato wscontato = jss.Deserialize<wsContato>(JSONdata);
                if (wscontato == null)
                {
                    // Error: Couldn't deserialize our JSON string into a "wsOrder" object.
                    return -2;
                }

                AgendaDBDataContext dc = new AgendaDBDataContext();
                CONTATO contato = dc.CONTATOs.Where(c => c.ID == wscontato.id).FirstOrDefault();

                if (contato == null)
                {
                    return -3;
                }

                contato.NOME = wscontato.nome;
                contato.ENDERECO = wscontato.endereco;
                contato.COMPLEMENTO = wscontato.complemento;
                contato.BAIRRO = wscontato.bairro;
                contato.MUNICIPIO = wscontato.municipio;
                contato.CEP = wscontato.cep;
                contato.TELEFONE = wscontato.telefone;
                contato.ESTADOID = wscontato.estado.id;
                //contato.ESTADO = UtilConversao.converterEstado(wscontato.estado);

                dc.SubmitChanges();

                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int RemoverContato(string contatoID)
        {
            try
            {
                AgendaDBDataContext dc = new AgendaDBDataContext();

                int nContatoId = int.Parse(contatoID);

                //wsContato contato = GetContatoPorID(contatoID);

                CONTATO contato = dc.CONTATOs.Where(c => c.ID == nContatoId).FirstOrDefault();

                if (contato != null)
                {
                    dc.CONTATOs.DeleteOnSubmit(contato);
                    dc.SubmitChanges();

                }


                return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                return -1;
            }
        }

        public int CriarContato(Stream JSONdataStream)
        {
            try
            {
                StreamReader reader = new StreamReader(JSONdataStream);
                string JSONdata = reader.ReadToEnd();

                JavaScriptSerializer jss = new JavaScriptSerializer();
                wsContato wscontato = jss.Deserialize<wsContato>(JSONdata);
                if (wscontato == null)
                {
                    return -1;
                }

                AgendaDBDataContext dc = new AgendaDBDataContext();
                CONTATO contato = UtilConversao.converterContato(wscontato);

                dc.CONTATOs.InsertOnSubmit(contato);
                dc.SubmitChanges();

                return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                return -1;
            }
        }
    }
}
