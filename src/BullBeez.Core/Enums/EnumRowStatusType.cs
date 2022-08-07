using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BullBeez.Core.Enums
{
    [DataContract]
    public enum EnumRowStatusType
    {
        /// <summary>
        /// SaveChanges işleminde kalıcı olarak silmek için kullanılır.
        /// </summary>
        [EnumMember]
        PersistentDeleted = -2,

        /// <summary>
        /// Satırda -1 olarak işaretlenir ve mantıksal silindiği anlamına gelir.
        /// </summary>
        [EnumMember]
        SoftDeleted = -1,

        /// <summary>
        /// Aktif bir kayıttır.
        /// </summary>
        [EnumMember]
        Active = 1,

        /// <summary>
        /// Kullanıcı tarafından görülüp tekrar aktif yapılabilecek bir kayıttır.
        /// </summary>
        [EnumMember]
        Passive = 0
    }
}