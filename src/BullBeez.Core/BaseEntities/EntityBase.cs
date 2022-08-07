using BullBeez.Core.Enums;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BullBeez.Core.BaseEntities
{
    public class EntityBase
    {
        /// <summary>
        /// Tüm entitylerimizde bulunması gereken propertyleri bu base class ile tanımlıyoruz.
        /// Bu sınıfımızı inherit alacak derived class'ımız dğrudan aşağıdaki propertylere sahip olacak ve biz tekrarlı kod yazmak zorunda kalmayacağız
        /// Dto ve data arasındaki farklılık eminim dikkatinizi çekmiştir burada örnek olması açısından küçük bir business kod ekledim
        /// Eğer bu sınıf türetildiğinde Id null ise default olarak yeni bir guid oluşturularak atanacak ve aynı şey createDate alanı içinde geçerli olacak
        /// Data sınıfları business içerebilir ancak dto sınıfları sadece propertyler içermektedir, çünkü amaçları datayı taşımaktır.
        /// Data ve dto arasındaki value transferlerini auto Mapper kullanarak yapacağız.
        /// </summary>
      
        public int Id { get; set; }
        public EnumRowStatusType? RowStatu { get; set; }

        public EntityBase()
        {
            this.RowStatu = EnumRowStatusType.Active;
        }

        public void Delete(
            bool persistent = false)
        {
            this.RowStatu = persistent
                            ? EnumRowStatusType.PersistentDeleted
                            : EnumRowStatusType.SoftDeleted;
        }

        public void StatuChange(
            EnumRowStatusType rowStatuType)
        {
            this.RowStatu = rowStatuType;
        }

    }
}