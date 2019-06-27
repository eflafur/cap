using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneRimborsi.Core.Process
{
    class DbQuery
    {


//select a.COD_CLIENTE, a.COD_CLIENTE_INTEGRA, a.DES_RAGIONE_SOCIALE, a.COD_CODICE_FISCALE,
//a.COD_PARTITA_IVA, c.COD_ID_CONTRATTO, 
//NVL((Select case when ca.utenza_domestica = 'Y' and ca.utenza_residente = 'Y' then 1 else 0 end from COM_CLASSIFICAZIONE_ARERA ca
//where ca.ID_CLASSIFICAZIONE = cd.CLASSIFICAZIONE_ARERA), 0) Is_DomesticoResidente,
//com.DENOMINAZIONE comune_puf, s.DENOMINAZIONE indirizzo_puf, puf.DES_NUMERO_CIVICO_PUF civico_puf, com.CAP cap_puf, com.PROVINCIA provincia_puf
///*anagrafica clienti*/
//from COM_ANAGRAFICA_TERZI a
///*contratto*/
//inner join COM_CONTRATTO c on a.COD_CLIENTE = c.COD_INTESTATARIO and c.FLG_LAST = 'Y' and c.COD_STATO_CONTRATTO = 'CA00' -- <-- stato contratto Attivo
//inner join COM_CONTRATTO_DETT cd on c.COD_ID_CONTRATTO = cd.COD_ID_CONTRATTO and cd.FLG_LAST = 'Y'
///*anagrafica punti di fornitura*/
//inner join COM_PUNTO_FORNITURA puf on c.COD_PUNTO_FORNITURA = puf.COD_PUNTO_FORNITURA and puf.FLG_LAST = 'Y'
///*anagrafica comuni*/
//inner join AQUE.COMUNI com on com.COMUNE = puf.COD_COMUNE_PUF
///*anagrafica strade*/
//inner join AQUE.STRADE s on s.STRADA = puf.COD_STRADA_PUF
//where a.FLG_LAST = 'Y' and a.FLG_TIPO = 'I'

    }
}
