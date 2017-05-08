using AgendaWS.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgendaWS
{
    public class UtilConversao
    {

        public static wsEstado converterEstado(ESTADO est)
        {
            return new wsEstado()
            {
                id = est.ID,
                descricao = est.DESCRICAO,
                sigla = est.SIGLA
            };
        }

        public static ESTADO converterEstado(wsEstado estado)
        {
            return new ESTADO()
            {
                ID = estado.id,
                DESCRICAO = estado.descricao,
                SIGLA = estado.sigla
            };
        }

        public static CONTATO converterContato(wsContato contato)
        {
            return new CONTATO()
            {
                ID = contato.id,
                NOME = contato.nome,
                ENDERECO = contato.endereco,
                COMPLEMENTO = contato.complemento,
                BAIRRO = contato.bairro,
                MUNICIPIO = contato.municipio,
                CEP = contato.cep,
                TELEFONE = contato.telefone,
                ESTADOID = contato.estadoid
            };
        }

    }
}