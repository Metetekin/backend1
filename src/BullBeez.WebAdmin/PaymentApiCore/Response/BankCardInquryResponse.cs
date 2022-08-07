using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BullBeez.WebAdmin.PaymentApiCore.Base;


namespace BullBeez.WebAdmin.PaymentApiCore.Response
{
    /// <summary>
    /// Cüzdanda bulunan kartları getirmek için kullanılan servis çıktı parametrelerini temsil etmektedir.
    /// </summary>
    public class BankCardInquryResponse: BaseResponse
    {
        public List<BankCard> cards { get; set; }

    }
}
