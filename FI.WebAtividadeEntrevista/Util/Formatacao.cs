using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebAtividadeEntrevista.Util
{
    public class Formatacao
    {
        public static string FormatarCpfCnpj(string strCpfCnpj)
        {
            if (string.IsNullOrEmpty(strCpfCnpj))
                return null;

            strCpfCnpj = Regex.Replace(strCpfCnpj, "[^0-9]", "");
            try
            {
                if (strCpfCnpj.Length == 11)
                {
                    MaskedTextProvider mtpCpf = new MaskedTextProvider(@"000\.000\.000-00");
                    mtpCpf.Set(strCpfCnpj);
                    return mtpCpf.ToString();
                }
                else if (strCpfCnpj.Length == 14)
                {
                    MaskedTextProvider mtpCnpj = new MaskedTextProvider(@"00\.000\.000/0000-00");
                    mtpCnpj.Set(strCpfCnpj);
                    return mtpCnpj.ToString();
                }
                return strCpfCnpj;
            }
            catch
            {
                return strCpfCnpj;
            }
        }

        public static string RemocePontucaoCpfCnpj(string cpfCnpj)
        {
            if (string.IsNullOrEmpty(cpfCnpj))
                return string.Empty;

            return Regex.Replace(cpfCnpj, "[^0-9]", "");
        }
    }
}