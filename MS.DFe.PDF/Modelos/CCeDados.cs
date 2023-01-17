using MS.DFe.PDF.Extensoes;
using System;
using System.Collections.Generic;

namespace MS.DFe.PDF.Modelos
{
    /// <summary>
    /// Dados gerais para impressão da CC-e
    /// </summary>
    public class CCeDados
    {
        /// <summary>
        /// Dados da nota emitida
        /// </summary>
        public CCeDadosNota Nota { get; }
        /// <summary>
        /// Protocolo da CC-e
        /// </summary>
        public string Protocolo { get; }
        /// <summary>
        /// Ambiente de emissão da CC-e
        /// </summary>
        public EAmbiente Ambiente { get; }
        /// <summary>
        /// Data e hora do evento
        /// </summary>
        public DateTimeOffset DataHoraEvento { get; }
        /// <summary>
        /// Data e hora do registro
        /// </summary>
        public DateTimeOffset DataHoraRegistro { get; }
        
        /// <summary>
        /// Lista de correções da CC-e
        /// </summary>
        public IEnumerable<string> Correcoes { get; }   

        /// <summary>
        /// Texto literal PRODUÇÃO ou HOMOLOGAÇÃO
        /// </summary>
        public string AmbienteLiteral { get => Ambiente.FormataAmbiente(); }
        /// <summary>
        /// Data e hora do evento formatado
        /// Ex.: dd/MM/yyyy HH:mm:SS
        /// </summary>
        public string DataHoraEventoFormatado { get => DataHoraEvento.FormataDataHora(); }
        /// <summary>
        /// Data e hora do registro formatado
        /// Ex.: dd/MM/yyyy HH:mm:SS
        /// </summary>
        public string DataHoraRegistroFormatado { get => DataHoraRegistro.FormataDataHora(); }


        public CCeDados(CCeDadosNota nota, string protocolo, EAmbiente ambiente, DateTimeOffset dataHoraEvento, DateTimeOffset dataHoraRegistro, IEnumerable<string> correcoes)
        {
            Nota = nota;
            Protocolo = protocolo;
            Ambiente = ambiente;
            DataHoraEvento = dataHoraEvento;
            DataHoraRegistro = dataHoraRegistro;
            Correcoes = correcoes;
        }
    }
}
