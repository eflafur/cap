using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestioneRimborsi.Web
{
    public class IBANHelper
    {

        public IBANHelper()
        {
        }


        private string mAbi = String.Empty;
        private string mCab = String.Empty;
        private string mContoCorrente = String.Empty;
        private string mCin = String.Empty;
        private const int L_CONTO = 12;
        private const int L_ABI = 5;
        private const int L_CAB = 5;
        private bool mNormalizzaConto = true;
        private string mIBAN = String.Empty;
        private string mBBAN = String.Empty;
        private string mCheckDigitIBAN = String.Empty;
        private string mPaese = String.Empty;
        private int mDivisore = 97;
        public string Abi
        {
            get
            {
                return mAbi;
            }
            set
            {
                mAbi = NormalizzaDati(value, L_ABI);
            }
        }

        public string Cab
        {
            get
            {
                return mCab;
            }
            set
            {
                mCab = NormalizzaDati(value, L_CAB); ;
            }
        }

        public string ContoCorrente
        {
            get
            {
                return mContoCorrente;
            }
            set
            {
                mContoCorrente = value;
            }
        }

        public string Cin
        {
            get
            {
                return mCin;
            }
            set
            {
                mCin = value;
            }
        }

        public string BBAN
        {
            get
            {
                return mBBAN;
            }
            set
            {
                mBBAN = value;
            }
        }

        public string IBAN
        {
            get
            {
                return mIBAN;
            }
            set
            {
                mIBAN = value;
            }
        }

        public string CheckDigitIBAN
        {
            get
            {
                return mCheckDigitIBAN;
            }
            set
            {
                mCheckDigitIBAN = value;
            }
        }

        public string Paese
        {
            get
            {
                return mPaese;
            }
            set
            {
                mPaese = value;
            }
        }

        public bool NormalizzaConto
        {
            get
            {
                return mNormalizzaConto;
            }
            set
            {
                mNormalizzaConto = value;
            }
        }

        public int Divisore
        {
            get
            {
                return mDivisore;
            }
            set
            {
                mDivisore = value;
            }
        }

        private string NormalizzaDati(string codice, int lunghezza)
        {
            codice = codice.Trim();
            int k = codice.Length;
            if (k < lunghezza)
            {
                codice = "".PadLeft(lunghezza, '0') + codice;
                k += lunghezza;
            }
            k -= lunghezza;
            if (k < 0)
                k = 0;
            codice = codice.Substring(k);
            return codice;
        }

        public string NormalizzaContoCorrente(string contoCorrenteValue)
        {
            contoCorrenteValue = contoCorrenteValue.Trim();

            int k = contoCorrenteValue.IndexOf(' ');
            while (k >= 0)
            {
                contoCorrenteValue = contoCorrenteValue.Remove(k, 1);
                k = contoCorrenteValue.IndexOf(' ');
            }
            System.Text.StringBuilder sb = new StringBuilder();
            for (k = 0; k<contoCorrenteValue.Length;k++)
            {
                byte result;
                if (byte.TryParse(contoCorrenteValue.Substring(k,1),out result)){
                    sb.Append(result.ToString());
                }
                else if ((int)(contoCorrenteValue.Substring(k, 1).ToCharArray()[0]) >= 65 && (int)(contoCorrenteValue.Substring(k, 1).ToCharArray()[0]) <= 90)
                {
                    sb.Append(contoCorrenteValue.Substring(k, 1));
                }
            }
            return NormalizzaDati(sb.ToString(), L_CONTO);

        }

        public bool VerificaCin(string cinCode)
        {
            return (cinCode == CalcolaCin());
        }

        public string CalcolaCin()
        {
            // costanti e variabili per calcolo pesi
            const string numeri = "0123456789";
            const string lettere = "ABCDEFGHIJKLMNOPQRSTUVWXYZ-. ";
            const int DIVISORE = 26;
            int[] listaPari = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 };
            int[] listaDispari = { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23, 27, 28, 26 };

            // normalizzazione dati            
            if (this.Abi.Length != L_ABI)
                mAbi = NormalizzaDati(mAbi, L_ABI);
            if (this.Cab.Length != L_CAB)
                mCab = NormalizzaDati(mCab, L_CAB);
            if (this.NormalizzaConto)
                this.ContoCorrente = NormalizzaContoCorrente(this.ContoCorrente);
            if (this.ContoCorrente.Length != L_CONTO)
                this.ContoCorrente = this.ContoCorrente.PadRight(L_CONTO);

            // codice normalizzato
            string codice = this.Abi + this.Cab + this.ContoCorrente;

            // calcolo valori caratteri
            int somma = 0;
            char[] c = codice.ToUpper().ToCharArray();
            for (int k = 0; k < (L_CONTO + L_ABI + L_CAB); k++)
            {
                int i = numeri.IndexOf(c[k]);
                if (i < 0)
                    i = lettere.IndexOf(c[k]);

                // se ci sono caratteri errati usciamo con un valore 
                // impossibile da trovare sul cin
                if (i < 0)
                    return Environment.NewLine;

                if ((k % 2) == 0)
                {
                    // valore dispari
                    somma += listaDispari[i];
                }
                else
                {
                    // valore pari
                    somma += listaPari[i];
                }
            }
            var result = lettere.Substring(somma % DIVISORE, 1);
            if (string.IsNullOrEmpty(result))
            {
                string a = "";
            }
            return result;
        }

        //public bool CheckIBAN(string codice)
        //{
        //    if (mIBAN != "")
        //        return CheckIBAN(codice);
        //    else

        //        return false;

        //}

        public bool CheckIBAN(string pIBAN)
        {
            string codice = NormalizzaIBAN(pIBAN);
            if (!CheckLength(codice))
                return false;
            codice = codice.Substring(4) + codice.Substring(0, 4);
            string[] r = Funzioni.DivisioneIntera(AlfaToNumber(codice), Divisore.ToString());
            int resto = int.Parse(r[1]);
            return (resto == 1);
        }

        public string CalcolaBBAN()
        {
            string codice;
            if (mIBAN != "")
                codice = mIBAN;
            else
            {
                string s = mCin;
                if (s == "")
                    s = CalcolaCin();
                codice = s + NormalizzaDati(mAbi, L_ABI) +
                      NormalizzaDati(mCab, L_CAB) +
                      NormalizzaContoCorrente(mContoCorrente);
            }
            return codice;
        }

        public string CalcolaCIN(string ABI, string CAB, string ContoCorrente)
        {
            this.Abi = ABI;
            this.Cab = CAB;
            this.ContoCorrente = ContoCorrente;
            if (ContoCorrente.Trim() == "399/34")
            {
                string a = "";
            }
            return CalcolaCin();
        }
        public string CalcolaIBAN(string ABI, string CAB, string ContoCorrente, string CIN)
        {
            if (!string.IsNullOrEmpty(ABI))
            {
                this.Abi = ABI;
                this.Cab = CAB;
                this.ContoCorrente = ContoCorrente;
                if (ContoCorrente.Trim() == "399/34")
                {
                    string a = "";
                }
                if (string.IsNullOrEmpty(CIN))
                    this.Cin = this.CalcolaCin();
                else
                    this.Cin = CIN;
                this.Paese = "IT";
                return CalcolaIBAN();
            }
            return string.Empty;
        }

        public string CalcolaIBAN()
        {
            string codice;
            if (mBBAN != "")
                codice = mBBAN;
            else
            {
                codice = CalcolaBBAN();
            }
            return CalcolaIBAN(mPaese, codice);
        }

        public string CalcolaCheckIBAN(string pPaese, string pBBAN)
        {
            return CalcolaIBAN(pPaese, pBBAN).Substring(2, 2);
        }

        public string CalcolaIBAN(string pPaese, string pBBAN)
        {
            pBBAN = NormalizzaIBAN(pBBAN);
            string codice = pPaese + "00" + pBBAN;
            codice = codice.Substring(4) + codice.Substring(0, 4);
            string numcode = AlfaToNumber(codice);
            string[] r = Funzioni.DivisioneIntera(numcode, Divisore.ToString());
            int resto = int.Parse(r[1]);
            resto = (Divisore + 1) - resto;
            return pPaese + resto.ToString("00") + pBBAN;
        }


        public string NormalizzaIBAN(string pCodice)
        {
            const string alfanum = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sb = new StringBuilder();
            foreach (char c in pCodice)
            {
                if (alfanum.IndexOf(c) != -1)
                    sb.Append(c);
            }
            return sb.ToString();
        }

        private bool CheckLength(string pCodice)
        {
            return true;
        }

        private string AlfaToNumber(string pCodice)
        {
            const string alfachars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            foreach (char c in pCodice)
            {
                int k = alfachars.IndexOf(c);
                if (k != -1)
                    sb.Append(k + 10);
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

    }

    public sealed class Funzioni
    {
        public Funzioni()
        {
        }

        public static string[] DivisioneIntera(string pDividendo, string pDivisore)
        {
            StringBuilder Intero = new StringBuilder();
            StringBuilder Resto = new StringBuilder();
            double divisore;
            if (!double.TryParse(pDivisore, System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out divisore))
                throw new Exception("Divisore errato");
            for (int x = 0; x < pDividendo.Length; x++)
            {
                Resto.Append(pDividendo.Substring(x, 1));
                string s = Resto.ToString();
                double dividendo = 0;
                if (!double.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out dividendo))
                    throw new Exception("Dividendo Errato");
                int volte = 0;
                while (dividendo >= divisore)
                {
                    dividendo -= divisore;
                    volte++;
                }
                Intero.Append(volte);
                string r = dividendo.ToString("0");
                Resto = new StringBuilder();
                Resto.Append(r);
            }
            string[] result = new string[2];
            result[1] = Resto.ToString();
            result[0] = Intero.ToString();
            while (result[0].StartsWith("0"))
                result[0] = result[0].Substring(1);
            if (result[0] == "")
                result[0] = "0";
            return result;
        }
    }
}

